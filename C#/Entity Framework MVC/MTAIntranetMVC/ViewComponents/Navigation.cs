using Microsoft.AspNetCore.Mvc;
namespace MTAIntranet.MVC.ViewComponents
{
    public class Navigation : ViewComponent
    {
        public Navigation()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View("Navigation");
        }
    }
}
