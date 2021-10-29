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
        private Button[,] buttons = null;
        private const int rowCount = 100;
        private const int colCount = 100;

        public MainForm()
        {
            InitializeComponent();
        }

        private void DrawBotton()
        {

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

            tokenSource = new();
            #region 画网格
            ControlCenter cc = new();
            Graphics g = simulatePanel.CreateGraphics();
            Pen p = new(Brushes.Blue);
            for (int i = 0; i < cc.Size.Item1; i++)
            {
                for (int j = 0; j < cc.Size.Item2; j++)
                {
                    g.DrawRectangle(p, j * 20, i * 20, 20, 20);
                }
            }
            #endregion
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
            //Button[,] buttons = new Button[rowCount, colCount];
            //for (int i = 0; i < rowCount; ++i)
            //{
            //    for (int j = 0; j < colCount; ++j)
            //    {
            //        Button button = buttons[i, j] = new Button();
            //        //button.Text = string.Empty;
            //        buttons[i,j].FlatStyle = FlatStyle.Flat;
            //        button.Visible = true;
            //        button.Location = new Point(10 * j, 10 * i);
            //        buttons[i, j].Size = new Size(10, 10);
            //        simulatePanel.Controls.Add(button);
            //    }
            //}
            //this.buttons = buttons;

            Graphics g = simulatePanel.CreateGraphics();
            Pen p = new(Brushes.Blue);
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    g.DrawRectangle(p, j * 10, i * 6, 10, 6);
                }
            }
        }

        private void StopSimulateButton_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }
    }
}
