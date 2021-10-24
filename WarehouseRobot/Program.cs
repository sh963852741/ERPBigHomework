using System;
using System.Threading;
using WarehouseRobot;

namespace ERPBigHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            ControlCenter cc = new();
            
            while (!(Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape))
            {
                if (cc.runningTasks.Count == 0)
                {
                    cc.AssignTask(new TransportTask()
                    {
                        from = (0, 0),
                        to = (12, 19)
                    });
                }

                cc.Print();
                cc.NextTick();
                Thread.Sleep(1000);
            }
        }
    }
}
