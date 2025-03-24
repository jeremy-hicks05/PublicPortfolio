namespace MTAIntranetMVC.Models
{
    using MTAIntranet.Shared;
    using System.Security.Claims;

    public class HomeIndexViewModel
    {
        public System.Security.Claims.ClaimsPrincipal? _user;

        public HomeIndexViewModel(ClaimsPrincipal? user)
        {
            _user = user;
        }
    }
}