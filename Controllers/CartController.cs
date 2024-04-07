using ApiAppBangHang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiAppBangHang.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public IActionResult AddToCart([FromBody] List<CartItem> cartItems )
        {
            return Ok();
        }
    }
}
