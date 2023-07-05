using ECommerceApp.Application.Interfaces;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ECommerceApp.Persistance.Repositories
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<DeliveryMethod> _deliveryRepository;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<DeliveryMethod> deliveryRepo,
            IGenericRepository<Product> productRepo,IBasketRepository basketRepo)
        {
            _orderRepository = orderRepo;
            _productRepository = productRepo;
            _deliveryRepository = deliveryRepo;
            _basketRepository = basketRepo;
            
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            //get basket from the repo
            var basket = await _basketRepository.GetBasketAsync(basketId);
            //get items from the product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _productRepository.GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name,productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered,productItem.Price,item.Quantity);
                items.Add(orderItem);
            }
            // get delivery method from repo
            var deliveryMethod = await _deliveryRepository.GetByIdAsync(deliveryMethodId);
            //calc subtotel
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            // create order
            var order = new Order(items,buyerEmail,shippingAddress,deliveryMethod,subtotal);
            //TODO: save to db

            // return order
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
