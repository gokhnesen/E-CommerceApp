using ECommerceApp.Application.Interfaces;
using ECommerceApp.Application.Specifications;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.OrderAggregate;
using ECommerceApp.Persistance.Context;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly StoreContext _storeContext;
        private readonly IBasketRepository _basketRepository;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepository;
        

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork,IGenericRepository<Product> productRepository
            ,IGenericRepository<Order> orderRepository,IGenericRepository<DeliveryMethod> deliveryMethodRepository,StoreContext context)
        {

            _basketRepository = basketRepo;
            _unitOfWork = unitOfWork;
            _deliveryMethodRepository = deliveryMethodRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _storeContext = context;
            
            
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            //get basket from the repo
            var basket = await _basketRepository.GetBasketAsync(basketId);
            //get items from the product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name,productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered,productItem.Price,item.Quantity);
                items.Add(orderItem);
            }
            // get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            //calc subtotel
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            // create order
            var order = new Order(items,buyerEmail,shippingAddress,deliveryMethod,subtotal);
            _unitOfWork.Repository<Order>().Add(order);

            //TODO: save to db
            // save to db
            try
            {
                var result = await _unitOfWork.Complete();
                if (result <= 0)
                    throw new Exception("Kaydetme işlemi başarısız oldu.");
            }
            catch (Exception ex)
            {
                // İç istisnayıyı görüntüleme
                if (ex.InnerException != null)
                {
                    Console.WriteLine("İç istisnayı: " + ex.InnerException.Message);
                }
                // Hata yönetimi veya tekrar deneme stratejileri burada ele alınabilir
                return null;
            }




            // delete basket
            //await _basketRepository.DeleteBasketAsync(basketId);

            // return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            //return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
            return await _deliveryMethodRepository.ListAllAsync();

        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            //return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            return await _orderRepository.GetEntityWithSpec(spec);

        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            //return await _unitOfWork.Repository<Order>().ListAsync(spec);
            return await _orderRepository.ListAsync(spec);

        }
    }
}
