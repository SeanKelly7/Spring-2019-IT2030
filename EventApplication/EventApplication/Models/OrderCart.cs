using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventApplication.Models
{
    public class OrderCart
    {
        public string OrderCartId;

        private EventApplicationDB db = new EventApplicationDB();

        public static OrderCart GetCart(HttpContextBase context)
        {
            OrderCart cart = new OrderCart();
            cart.OrderCartId = cart.GetCartId(context);
            return cart;
        }

        private string GetCartId(HttpContextBase context)
        {
            const string CartSessionId = "CartId";

            context.Session[CartSessionId] = context.User.Identity.Name;

            return context.Session[CartSessionId].ToString();
        }

        public List<Order> GetCartItems()
        {
            return db.Orders.Where(a => a.OrderId == OrderCartId).ToList();
        }

        public int AddToCart(int eventId, int count)
        {
            Event @event = db.Events.Find(eventId);

            if (count > @event.TicAvail || count < 0 || count > 10 || @event.StartDate < DateTime.Today)
            {
                return 0;
            }

            Order order = new Order()
            {
                OrderId = OrderCartId,
                EventId = eventId,
                Count = count,
                DateCreated = DateTime.Now,
                Status = "Processed"
            };

            db.Orders.Add(order);

            @event.TicAvail -= count;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            db.Configuration.ValidateOnSaveEnabled = true;

            return order.RecordId;
        }

        public string CancelOrder(int recordId)
        {
            Order order = db.Orders.SingleOrDefault(c => c.OrderId == OrderCartId && c.RecordId == recordId);

            if (order == null)
            {
                throw new NullReferenceException();
            }

            order.EventSelected.TicAvail += order.Count;

            order.Status = "Canceled";
            string newStatus = order.Status;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            db.Configuration.ValidateOnSaveEnabled = true;

            return newStatus;
        }
    }
}