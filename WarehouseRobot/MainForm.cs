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
        private CancellationTokenSource tokenSource = new();
        private ControlCenter cc = null;
        private HashSet<Point> obstaclePoints = new();
        private HashSet<Point> beginPoints = new();
        private int maxRow = 0;
        private int maxCol = 0;
        private const int gridGap = 20;
        private ZoneState[,] grid = null;
        private Graphics graphics = null;
        private MapState mapState = MapState.Unknown;
        private readonly Color defaultColor = Color.FromArgb(240, 240, 240);

        public MainForm()
        {
            //CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            graphics = simulatePanel.CreateGraphics();
        }

        private void PrintMap()
        {
            /* 清空所有的格子（含障碍物） */
            Brush defaultBrush = new SolidBrush(defaultColor);
            Brush obstacleBrush = new SolidBrush(Color.Black);
            for (uint i = 0; i < grid.GetLength(0); ++i)
            {
                for (uint j = 0; j < grid.GetLength(1); ++j)
                {
                    if (grid[i, j] == ZoneState.Empty)
                        graphics.FillRectangle(defaultBrush, j * gridGap + 1, i * gridGap + 1, gridGap - 1, gridGap - 1);
                    else if (grid[i, j] == ZoneState.Blocked)
                        graphics.FillRectangle(obstacleBrush, j * gridGap + 1, i * gridGap + 1, gridGap - 1, gridGap - 1);
                }
            }

            /* 画出所有的机器人 */
            Brush robotBrush = new SolidBrush(Color.Red);
            foreach (KeyValuePair<Robot, TransportTask> keyValuePair in cc.RunningTasks)
            {
                Point pos = keyValuePair.Key.CurrentPosition;
                graphics.FillRectangle(robotBrush, pos.X * gridGap + 1, pos.Y * gridGap + 1, gridGap - 1, gridGap - 1);
            }
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
                    PrintMap();

                    Thread.Sleep(1000);
                }
            });

            setBeginButton.Enabled = false;
            setObstacleButton.Enabled = false;
            mapState = MapState.Unknown;
            allRobotCountLabel.Text = $"总共机器人数：{cc.Robots.Count}";
            task.Start();
        }

        private void SetCellColor(Point point, Color color)
        {
            Brush b = new SolidBrush(color);
            graphics.FillRectangle(b, point.X * gridGap + 1, point.Y * gridGap + 1, gridGap - 1, gridGap - 1);
        }

        /// <summary>
        /// 清除绘图
        /// </summary>
        private void ClearMap() => graphics.Clear(defaultColor);

        private void DrawButton_Click(object sender, EventArgs e)
        {
            int _maxRow = (int)rowCountUpDown.Value;
            int _maxCol = (int)colCountUpDown.Value;

            if (simulatePanel.Height < _maxRow * gridGap || simulatePanel.Width < _maxCol * gridGap)
            {
                MessageBox.Show("窗口过小，请最大化窗口或减小地图尺寸后重试。", "窗口过小", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (grid != null)
            {
                DialogResult res = MessageBox.Show("确实要重建仓库地图吗？\n您将丢失所有数据。", "确认重建仓库地图", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes) return;
                else ClearMap();
            }

            maxRow = _maxRow;
            maxCol = _maxCol;

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
            if (mapState == MapState.Unknown)
            {
                if (grid == null)
                    toolStripStatusLabel.Text = "请先绘制仓库地图。";
                else if (mapState == MapState.Unknown)
                    toolStripStatusLabel.Text = $"请选择{setBeginButton.Text}或者{setObstacleButton.Text}";
                return;
            }
            else if (mapState == MapState.SettingObstaclePoint)
            {
                MouseEventArgs @event = (MouseEventArgs)e;
                int x = @event.X / gridGap;
                int y = @event.Y / gridGap;

                if (x >= maxCol || y >= maxRow) return;

                Point p = new(x, y);
                if (obstaclePoints.Add(p))
                {
                    SetCellColor(p, Color.Black);
                    beginPoints.Remove(p);
                }
                else
                {
                    SetCellColor(p, defaultColor);
                    if (!obstaclePoints.Remove(p))
                    {
                        throw new Exception();
                    }
                }
                grid = GridGenerator.GetGrid(obstaclePoints.ToList(), maxRow, maxCol);
            }
            else if (mapState == MapState.SettingBeginPoint)
            {
                MouseEventArgs @event = (MouseEventArgs)e;
                int x = @event.X / gridGap;
                int y = @event.Y / gridGap;

                if (x >= maxCol || y >= maxRow) return;

                Point p = new(x, y);
                if (beginPoints.Add(p))
                {
                    SetCellColor(p, Color.LightGreen);
                    obstaclePoints.Remove(p);
                }
                else
                {
                    SetCellColor(p, defaultColor);
                    if (!beginPoints.Remove(p))
                    {
                        throw new Exception();
                    }
                }
                grid = GridGenerator.GetGrid(obstaclePoints.ToList(), maxRow, maxCol);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private enum MapState
        {
            Unknown,
            SettingBeginPoint,
            SettingObstaclePoint
        }

        private void SetBeginButton_Click(object sender, EventArgs e)
        {
            setBeginButton.Enabled = false;
            setObstacleButton.Enabled = true;
            toolStripStatusLabel.Text = string.Empty;
            mapState = MapState.SettingBeginPoint;
        }

        private void SetObstacleButton_Click(object sender, EventArgs e)
        {
            setObstacleButton.Enabled = false;
            setBeginButton.Enabled = true;
            toolStripStatusLabel.Text = string.Empty;
            mapState = MapState.SettingObstaclePoint;
        }
    }
}
