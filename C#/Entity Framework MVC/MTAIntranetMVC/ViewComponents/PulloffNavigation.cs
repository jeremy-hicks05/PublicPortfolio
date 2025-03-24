using Microsoft.AspNetCore.Mvc;

namespace MTAIntranet.MVC.ViewComponents
{
    public class PulloffNavigation : ViewComponent
    {
        public PulloffNavigation()
        {

        }

        public IViewComponentResult Invoke()
        {
            List<int> months = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            return View("PulloffNavigation", months);
        }
    }
}
