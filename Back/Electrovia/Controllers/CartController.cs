using AutoMapper;
using Electrovia.DTOs;
using Electrovia.Errors;
using Electrovia_Core.Entities;
using Electrovia_Core.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Electrovia.Controllers
{
    public class CartController : BaseApiController
    { 
        #region Constructor
        private readonly ICartRepositories _cart_Repo;
        private readonly IMapper _mapper;

        public CartController(ICartRepositories cart_Repo, IMapper mapper)
        {
            _cart_Repo = cart_Repo;
            _mapper = mapper;
        }
        #endregion


        [HttpGet]
        public async Task<ActionResult<Customer_Cart>> Get_Cart_by_id(string id )
        {
            var cart = await _cart_Repo.GetCart(id);
            return cart is null ? Ok( new Customer_Cart(id)) : Ok(cart);
        }
        
        [HttpPost]
        public async Task<ActionResult<Customer_Cart>> Update_Cart (Customer_Cart_DTO cus_cart)
        {
            var map = _mapper.Map<Customer_Cart_DTO , Customer_Cart>(cus_cart);
            var updated  = await _cart_Repo.UpdateCart(map);
            if (updated is null) return BadRequest(new APIsResponse(400));
            return Ok(updated);
        }

        [HttpDelete]
        public async Task Delete_Cart (string id)
        {
            await _cart_Repo.DeleteCart(id);
        }
    }
}
