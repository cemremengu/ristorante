namespace Ristorante.Demo
{
    using System.Linq;
    using Messages;

    public class Kitchen : IHandle<CookOrder>
    {
        private readonly Chef[] _chefs;
        private readonly IHandle<CookOrder> _loadBalancer;


        public Kitchen(IPublish publisher, Chef[] chefs)
        {
            _chefs = chefs;

            var queues = _chefs.Select(x => new QueuedHandler<CookOrder>(x)).ToArray();

            foreach (var queue in queues)
            {
                new ThreadBasedProcessor(queue);
            }

            _loadBalancer = new CompetingConsumer<CookOrder>(chefs);
        }

        public void Handle(CookOrder message)
        {
            _loadBalancer.Handle(message);
        }
    }
}