
using StajYonetimBilgiSistemi.Validation;
using System.Web.Mvc;
using System.Web.Routing;

namespace StajYonetimBilgiSistemi
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
       
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MvcHandler.DisableMvcResponseHeader = true;
            //FluentValidationModelValidatorProvider.Configure();
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");           //Remove Server Header   
            Response.Headers.Remove("X-AspNet-Version"); //Remove X-AspNet-Version Header

        }
    }
}
