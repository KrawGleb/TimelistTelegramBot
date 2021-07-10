using System;
using NLog;

namespace TimelistBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            Bot.Start();
            logger.Trace("Press any key to exit");
            Console.ReadLine();
        }
    }
}
