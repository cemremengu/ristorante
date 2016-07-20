namespace Ristorante.Infrastructure
{
    using System;
    using System.Threading;

    public class ThreadBasedProcessor
    {
        public ThreadBasedProcessor(IProcessor processor)
        {
            new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        if (!processor.TryProcess())
                            Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }).Start();
        }
    }

    public interface IProcessor
    {
        bool TryProcess();
    }
}