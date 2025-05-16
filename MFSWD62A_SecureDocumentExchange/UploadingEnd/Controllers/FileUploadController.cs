using Microsoft.AspNetCore.Mvc;

namespace UploadingEnd.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
