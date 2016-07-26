namespace Ristorante.Demo
{
    using Messages;

    public interface IRestaurantProcess :
        IHandle<OrderDelivered>,
        IHandle<OrderTaken>,
        IHandle<OrderCooked>,
        IHandle<PaymentTaken>,
        IHandle<FreeDrinkTimeout>

    {
    }
}