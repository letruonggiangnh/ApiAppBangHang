using ApiAppBangHang.Models;
using ApiAppBanSach.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ApiAppBangHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBookController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        public ProductBookController(AppBanSachDbContext context)
        {
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
        [Route("book")]
        public IActionResult Get()
        {  
            var user = unitOfWork.ProductBookRepository.Get(filter: p => p.CategoryChildId == 10);
            return Ok(user);
        }
    }
}
