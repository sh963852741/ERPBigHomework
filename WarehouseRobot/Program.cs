using System;
using System.Threading;
using WarehouseRobot;

namespace ERPBigHomework
{
    class Program
    {
        static Random r = new();
        static void Main(string[] args)
        {
            ControlCenter cc = new();

            while (!(Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape))
            {
                if (cc.runningTasks.Count < 10)
                {
                    cc.AssignTask(new TransportTask()
                    {
                        from = ((uint)r.Next(0, 20), (uint)r.Next(0, 20)),
                        to = ((uint)r.Next(0, 20), (uint)r.Next(0, 20))
                    });
                }
                cc.NextTick();
                cc.Print();

                Thread.Sleep(1000);
            }
        }
    }
}
