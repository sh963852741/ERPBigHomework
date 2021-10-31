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
        private Point[] taskPoints = new Point[2];
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

        /// <summary>
        /// 画出地图的基础架构
        /// </summary>
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
        }

        private void PrintFinalMap()
        {
            PrintMap();
            int halfGap = gridGap / 2;
            /* 画出所有的机器人 */
            Brush strBrush = new SolidBrush(Color.White);
            Brush idleRobotBrush = new SolidBrush(Color.LightSalmon);
            Brush robotBrush = new SolidBrush(Color.Red);
            Pen robotPen = new (Color.Blue, 2);
            int i = 1;
            foreach (Robot robot in cc.Robots)
            {
                Point pos = robot.CurrentPosition;
                if (robot.State == RobotState.Idle)
                {
                    graphics.FillRectangle(idleRobotBrush, pos.X * gridGap + 1, pos.Y * gridGap + 1, gridGap - 1, gridGap - 1);

                }
                else if (robot.State == RobotState.Running)
                {
                    graphics.FillRectangle(robotBrush, pos.X * gridGap + 1, pos.Y * gridGap + 1, gridGap - 1, gridGap - 1);
                    if (robot.Route.Count > 0)
                        graphics.DrawLine(robotPen,
                            robot.CurrentPosition.X * gridGap + halfGap,
                            robot.CurrentPosition.Y * gridGap + halfGap,
                            robot.Route[0].X * gridGap + halfGap,
                            robot.Route[0].Y * gridGap + halfGap);
                }
                graphics.DrawString(i.ToString(), Font, strBrush, pos.X * gridGap + 1F, pos.Y * gridGap + 1F);
                ++i;
            }

            /* 画出所有的出生点 */
            Brush beginPointBrush = new SolidBrush(Color.LightGreen);
            foreach (Point pos in beginPoints)
            {
                graphics.FillRectangle(beginPointBrush, pos.X * gridGap + 1, pos.Y * gridGap + 1, gridGap - 1, gridGap - 1);
            }

            DrawTaskPoint();
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

            if (cc == null)
            {
                cc = new ControlCenter(grid);
                cc.OnOneTaskFinished += ReflashInfo;
            }
            cc.SetGird(grid);
            drawButton.Enabled = false;
            addTaskButton.Enabled = true;
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
                        TransportTask transportTask = new TransportTask()
                        {
                            from = new Point(r.Next(0, maxCol), r.Next(0, maxRow)),
                            to = new Point(r.Next(0, maxCol), r.Next(0, maxRow))
                        };
                        cc.AssignTask(new TransportTask()
                        {
                            from = new Point(r.Next(0, maxCol), r.Next(0, maxRow)),
                            to = new Point(r.Next(0, maxCol), r.Next(0, maxRow))
                        });
                    }
                    cc.NextTick();
                    PrintFinalMap();

                    Thread.Sleep(1000);
                }
            });

            beginSimulateButton.Enabled = false;
            setBeginButton.Enabled = false;
            setObstacleButton.Enabled = false;
            stopSimulateButton.Enabled = true;
            mapState = MapState.Unknown;
            addTaskButton.Enabled = true;
            allRobotCountLabel.Text = $"总共机器人数：{cc.Robots.Count}";
            task.Start();
        }

        private void SetCellColor(Point point, Color color)
        {
            Brush b = new SolidBrush(color);
            graphics.FillRectangle(b, point.X * gridGap + 1, point.Y * gridGap + 1, gridGap - 1, gridGap - 1);
        }

        /// <summary>
        /// 清除地图
        /// </summary>
        private void ClearMap()
        {
            grid = null;
            beginPoints.Clear();
            obstaclePoints.Clear();
            graphics.Clear(defaultColor);
        }

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

            targetRowUpDown.Maximum = beginRowUpDown.Maximum = _maxRow;
            targetColUpDown.Maximum = beginColUpDown.Maximum = _maxCol;
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

            if (genObstacleCheckBox.Checked)
            {
                grid = GridGenerator.GetGrid(out IList<Point> obstacle, maxRow, maxCol);
                obstaclePoints = obstacle.ToHashSet();
                PrintMap();
            }
            else
            {
                grid = GridGenerator.GetGrid(obstaclePoints.ToList(), maxRow, maxCol);
            }

            beginSimulateButton.Enabled = true;
            setBeginButton.Enabled = true;
            setObstacleButton.Enabled = true;
            setTaskPointButton.Enabled = true;
            mapState = MapState.Unknown;
        }

        private void StopSimulateButton_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
            drawButton.Enabled = true;
            beginSimulateButton.Enabled = true;
            setObstacleButton.Enabled = true;
            setBeginButton.Enabled = true;
            setTaskPointButton.Enabled = true;
            stopSimulateButton.Enabled = false;
            //addTaskButton.Enabled = false;
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            if (taskPoints[0].IsEmpty || taskPoints[1].IsEmpty)
            {
                MessageBox.Show("任务的起点或终点不在地图内。", "无效输入", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (taskPoints[0] == taskPoints[1])
            {
                MessageBox.Show("任务的起点和终点不能是同一个点。", "无效输入", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            cc.AssignTask(new TransportTask()
            {
                from = taskPoints[0] - new Size(1, 1),
                to = taskPoints[1] - new Size(1, 1)
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
            simulatePanel.Focus();
            MouseEventArgs @event = (MouseEventArgs)e;
            int x = @event.X / gridGap;
            int y = @event.Y / gridGap;

            if (x >= maxCol || y >= maxRow) return;

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
            else if (mapState == MapState.SettingTaskPoint)
            {
                Point p = new(x, y);
                if (obstaclePoints.Contains(p))
                {
                    return;
                }

                Point p1 = new(x + 1, y + 1);
                if (taskPoints[0] == p1)
                {
                    // 取消当前点
                    taskPoints[0] = Point.Empty;
                    SetCellColor(p, defaultColor);
                }
                else if (taskPoints[1] == p1)
                {
                    // 取消当前点
                    taskPoints[1] = Point.Empty;
                    SetCellColor(p, defaultColor);
                }
                else if (taskPoints[0] == Point.Empty)
                {
                    taskPoints[0] = p1;
                }
                else if (taskPoints[1] == Point.Empty)
                {
                    taskPoints[1] = p1;
                }
                else
                {
                    SetCellColor(taskPoints[0] - new Size(1, 1), defaultColor);
                    taskPoints[0] = taskPoints[1];
                    taskPoints[1] = p1;
                }
                DrawTaskPoint();
                SetUpDownCount();
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
            SettingObstaclePoint,
            SettingTaskPoint
        }

        private void SetBeginButton_Click(object sender, EventArgs e)
        {
            setBeginButton.Enabled = false;
            setObstacleButton.Enabled = true;
            setTaskPointButton.Enabled = true;
            toolStripStatusLabel.Text = string.Empty;
            mapState = MapState.SettingBeginPoint;
        }

        private void SetObstacleButton_Click(object sender, EventArgs e)
        {
            setObstacleButton.Enabled = false;
            setBeginButton.Enabled = true;
            setTaskPointButton.Enabled = true;
            toolStripStatusLabel.Text = string.Empty;
            mapState = MapState.SettingObstaclePoint;
        }

        private void SimulatePanel_SizeChanged(object sender, EventArgs e)
        {
            graphics = simulatePanel.CreateGraphics();
        }

        private void SetTaskPointButton_Click(object sender, EventArgs e)
        {
            setTaskPointButton.Enabled = false;
            setObstacleButton.Enabled = true;
            setBeginButton.Enabled = true;
            toolStripStatusLabel.Text = string.Empty;
            mapState = MapState.SettingTaskPoint;
        }

        /// <summary>
        /// 画出任务起讫点（如果有）
        /// </summary>
        private void DrawTaskPoint()
        {
            if (!taskPoints[0].IsEmpty)
                SetCellColor(taskPoints[0] - new Size(1, 1), Color.LightBlue);
            if (!taskPoints[1].IsEmpty)
                SetCellColor(taskPoints[1] - new Size(1, 1), Color.DarkBlue);
        }

        /// <summary>
        /// 从画面中清除任务起讫点
        /// </summary>
        private void UnDrawTaskPoint()
        {
            if (!taskPoints[0].IsEmpty)
                SetCellColor(taskPoints[0] - new Size(1, 1), defaultColor);
            if (!taskPoints[1].IsEmpty)
                SetCellColor(taskPoints[1] - new Size(1, 1), defaultColor);
        }

        private void GetUpDownCount()
        {
            taskPoints[0].X = (int)beginColUpDown.Value;
            taskPoints[0].Y = (int)beginRowUpDown.Value;
            taskPoints[1].X = (int)targetColUpDown.Value;
            taskPoints[1].Y = (int)targetRowUpDown.Value;
        }

        private void SetUpDownCount()
        {
            beginColUpDown.Value = taskPoints[0].X;
            beginRowUpDown.Value = taskPoints[0].Y;
            targetColUpDown.Value = taskPoints[1].X;
            targetRowUpDown.Value = taskPoints[1].Y;
        }

        private void UpDown_ValueChanged(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            if (ctrl != null && ctrl.Focused && mapState == MapState.SettingTaskPoint)
            {
                UnDrawTaskPoint();
                GetUpDownCount();
                DrawTaskPoint();
            }
        }
    }
}
