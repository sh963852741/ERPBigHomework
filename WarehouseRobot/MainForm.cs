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

namespace WarehouseRobot
{
    public partial class MainForm : Form
    {
        private static Random r = new();
        private CancellationTokenSource tokenSource;
        private ControlCenter cc = null;
        private int maxRow = 0;
        private int maxCol = 0;

        public MainForm()
        {
            //CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
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
            drawButton.Enabled = false;
            tokenSource = new();

            CancellationToken token = tokenSource.Token;
            Task task = new (() => {
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
            task.Start();
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            maxRow = (int)rowCountUpDown.Value;
            maxCol = (int)colCountUpDown.Value;
            cc = new ControlCenter(maxRow, maxCol);
            cc.OnOneTaskFinished += ReflashInfo;

            Graphics g = simulatePanel.CreateGraphics();
            Pen p = new(Brushes.Blue);
            for (int i = 0; i < cc.Size.Item1; i++)
            {
                for (int j = 0; j < cc.Size.Item2; j++)
                {
                    g.DrawRectangle(p, j * 20, i * 20, 20, 20);
                }
            }
            beginSimulateButton.Enabled = true;
            addTaskButton.Enabled = true;
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
            Action a = () => {
                queuingTaskCountLabel.Text = $"排队任务数：{cc.QueuingTaskCount}";
                runningTaskCountLabel.Text = $"执行任务数：{cc.RunningTasks.Count}";
            };
            this.Invoke(a);
        }
    }
}
