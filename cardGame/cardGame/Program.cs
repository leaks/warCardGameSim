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
            ManualResetEvent[] doneEvents = new ManualResetEvent[6];
            gameThread[] games = new gameThread[6];
            Random rand = new Random();

            for (int i = 0; i < 3; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                gameThread game = new gameThread(true, rand, doneEvents[i]);
                ThreadPool.QueueUserWorkItem(game.run, i);
            }

            for (int i = 3; i < 6; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                gameThread game = new gameThread(false, rand, doneEvents[i]);
                ThreadPool.QueueUserWorkItem(game.run, i);
            }

            WaitHandle.WaitAll(doneEvents);

            Console.ReadKey();
        }
    }
}
