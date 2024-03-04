using Microsoft.AspNetCore.Mvc;

namespace ApiAppBangHang.Controllers
{
    public class ProductBookController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
