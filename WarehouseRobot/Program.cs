using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using WarehouseRobot;

namespace ERPBigHomework
{
    class Program
    {
        static Random r = new();
        static void Main(string[] args)
        {
            #region A星算法测试
            /* AStarRoutePlanner aStarRoutePlanner = new AStarRoutePlanner();
             IList<Point> obstaclePoints = new List<Point>();
             obstaclePoints.Add(new Point(2, 4));
             obstaclePoints.Add(new Point(3, 4));
             obstaclePoints.Add(new Point(4, 4));
             obstaclePoints.Add(new Point(5, 4));
             obstaclePoints.Add(new Point(6, 4));
             aStarRoutePlanner.Initialize(obstaclePoints);

             IList<Point> route = aStarRoutePlanner.Plan(new Point(3, 3), new Point(4, 6));
             foreach(Point item in route)
             {
                 Console.WriteLine(item);
             }*/
            #endregion
            ControlCenter cc = new();

            while (!(Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape))
            {
                if (cc.runningTasks.Count < cc.GetRobotsNum())
                {
                    int maxCol = cc.GetSize().Item1;
                    int maxRow = cc.GetSize().Item2;
                    cc.AssignTask(new TransportTask()
                    {
                        
                        from = new Point((int)r.Next(0, maxCol), (int)r.Next(0, maxRow)),
                        to = new Point((int)r.Next(0, maxCol), (int)r.Next(0, maxRow))
                    });
                    ;
                }
                cc.NextTick();
                cc.Print();

                Thread.Sleep(1000);
            }
        }
    }
}
