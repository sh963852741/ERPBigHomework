using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseRobot.Enum;
using WarehouseRobot.Utility;

namespace WarehouseRobot
{
    public partial class MainForm : Form
    {
        private static Random r = new();
        private CancellationTokenSource tokenSource;
        private ControlCenter cc = null;
        private List<Point> obstaclePoints = new();
        private int maxRow = 0;
        private int maxCol = 0;
        private const int gridGap = 20;
        private ZoneState[,] grid = null;
        private Graphics graphics = null;

        public MainForm()
        {
            //CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            graphics = simulatePanel.CreateGraphics();
        }

        private void BeginSimulateButton_Click(object sender, EventArgs e)
        {
            #region A星算法测试
            //AStarRoutePlanner aStarRoutePlanner = new AStarRoutePlanner();
            //IList<Point> obstaclePoints = new List<Point>();
            //obstaclePoints.Add(new Point(2, 4));
            //obstaclePoints.Add(new Point(3, 4));
            //obstaclePoints.Add(new Point(4, 4));
            //obstaclePoints.Add(new Point(5, 4));
            //obstaclePoints.Add(new Point(6, 4));
            //aStarRoutePlanner.Initialize(obstaclePoints);

            //IList<Point> route = aStarRoutePlanner.Plan(new Point(3, 3), new Point(4, 6));
            //foreach (Point item in route)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            cc = new ControlCenter(grid);
            cc.OnOneTaskFinished += ReflashInfo;
            drawButton.Enabled = false;
            tokenSource = new();

            CancellationToken token = tokenSource.Token;
            Task task = new(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (cc.RunningTasks.Count < cc.RobotNum)
                    {
                        int maxRow = cc.Size.Item1;
                        int maxCol = cc.Size.Item2;
                        cc.AssignTask(new TransportTask()
                        {
                            from = new Point(r.Next(0, maxCol), r.Next(0, maxRow)),
                            to = new Point(r.Next(0, maxCol), r.Next(0, maxRow))
                        });
                    }
                    cc.NextTick();
                    cc.Print(simulatePanel.CreateGraphics());

                    Thread.Sleep(1000);
                }
            });

            allRobotCountLabel.Text = $"总共机器人数：{cc.Robots.Count}";
            task.Start();
        }

        private void SetCellColor(Point point, Color color)
        {
            Brush b = new SolidBrush(color);
            graphics.FillRectangle(b, point.X* gridGap + 1, point.Y* gridGap + 1, gridGap - 1, gridGap - 1);
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            maxRow = (int)rowCountUpDown.Value;
            maxCol = (int)colCountUpDown.Value;

            Pen p = new(Brushes.Blue);
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                {
                    graphics.DrawRectangle(p, j * gridGap, i * gridGap, gridGap, gridGap);
                }
            }
            grid = GridGenerator.GetGrid(maxRow, maxCol);

            beginSimulateButton.Enabled = true;
            addTaskButton.Enabled = true;
            setBeginButton.Enabled = true;
            setObstacleButton.Enabled = true;
        }

        private void StopSimulateButton_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
            drawButton.Enabled = true;
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            cc.AssignTask(new TransportTask()
            {
                from = new Point(r.Next(0, maxCol), r.Next(0, maxRow)),
                to = new Point(r.Next(0, maxCol), r.Next(0, maxRow))
            });

            /* 更新必要信息 */
            queuingTaskCountLabel.Text = $"排队任务数：{cc.QueuingTaskCount}";
            runningTaskCountLabel.Text = $"执行任务数：{cc.RunningTasks.Count}";
        }

        private void ReflashInfo()
        {
            Action a = () =>
            {
                queuingTaskCountLabel.Text = $"排队任务数：{cc.QueuingTaskCount}";
                runningTaskCountLabel.Text = $"执行任务数：{cc.RunningTasks.Count}";
            };
            this.Invoke(a);
        }

        private void SimulatePanel_Click(object sender, EventArgs e)
        {
            MouseEventArgs @event = (MouseEventArgs)e;
            int x = @event.X / gridGap;
            int y = @event.Y / gridGap;
            obstaclePoints.Add(new Point(x, y));
            grid = GridGenerator.GetGrid(obstaclePoints, maxRow, maxCol);
            SetCellColor(new Point(x, y), Color.Black);
        }

        private enum MapState
        {
            SettingBeginPoint,
            SettingObstaclePoint
        }
    }
}
