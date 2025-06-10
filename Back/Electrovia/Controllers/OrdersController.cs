using AutoMapper;
using Electrovia.DTOs;
using Electrovia.Errors;
using StackExchange.Redis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Electrovia_Core.Services;
using Microsoft.AspNetCore.Authorization;
using Electrovia_Core.Entities.Order_Aggregate;
using Order = Electrovia_Core.Entities.Order_Aggregate.Order;
using Electrovia.Helpers;

namespace Electrovia.Controllers
{

    [Authorize]
    public class OrdersController : BaseApiController
    {
        #region Constructor 
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        public OrdersController(IOrderServices orderServices, IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }

        #endregion

        #region Create_Order 
        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIsResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDTO2>> CreateOrder(OrderDTO orderdto)
        {
            var User_Email = User.FindFirstValue(ClaimTypes.Email);
            var shipping_address = _mapper.Map<AddressDTO, Address>(orderdto.Shipping_Address!);
            var order = await _orderServices.CreateOrderAsync(User_Email!, orderdto.Cartid!, orderdto.DeliveryMedthodid, shipping_address);
            if (order is null) return BadRequest(new APIsResponse(400));
            return Ok(_mapper.Map<Order, OrderDTO2>(order));
        }

        #endregion

        #region Get_Order_For_User   
        [HttpGet]
        [CachedAttribute(600)]
        public async Task<ActionResult<IReadOnlyList<OrderDTO2>>> GetOrdersForUser()
        {
            var User_Email = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderServices.GetOrderForUser(User_Email!);
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderDTO2>>(Orders));
        }


        #endregion

        #region Get_Order_For_User_ById
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIsResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]

        public async Task<ActionResult<OrderDTO2>> GetOrderByIdForUser(int id)
        {
            var User_Email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderServices.GetOrderByIdForUser(User_Email!, id);
            if (order is null) return NotFound(new APIsResponse(404));
            return Ok(_mapper.Map<Order, OrderDTO2>(order));
        }

        #endregion

        #region Get_Delivery_Method 
        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderServices.GetDeliveryMethod();
            return Ok(deliveryMethods);
        } 

        #endregion 
    }
}
