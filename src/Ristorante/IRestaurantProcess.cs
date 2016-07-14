namespace Ristorante
{
    using Infrastructure;
    using Messages;

    public interface IRestaurantProcess :
        IHandle<OrderDelivered>,
        IHandle<OrderTaken>,
        IHandle<OrderCooked>,
        IHandle<PaymentTaken>

    {
    }
}