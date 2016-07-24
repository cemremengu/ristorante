namespace Ristorante
{
    using System.Threading;

    public class ThreadBasedProcessor
    {
        public ThreadBasedProcessor(IProcessor processor)
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (!processor.TryProcess())
                        Thread.Sleep(1);
                }
            }).Start();
        }
    }

    public interface IProcessor
    {
        bool TryProcess();

    
    }
}