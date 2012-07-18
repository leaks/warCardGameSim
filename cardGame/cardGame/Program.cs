using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace cardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            ManualResetEvent[] doneEvents = new ManualResetEvent[20];
            gameThread[] games = new gameThread[20];

            for (int i = 0; i < 20; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                gameThread game = new gameThread(true, doneEvents[i]);
                ThreadPool.QueueUserWorkItem(game.run, i);
            }

            WaitHandle.WaitAll(doneEvents);

            Console.ReadKey();
        }
    }
}
