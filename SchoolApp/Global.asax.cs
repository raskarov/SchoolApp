using Isg.EntityFramework.Interceptors;
using Isg.EntityFramework.Interceptors.SoftDelete;
using Isg.EntityFramework.Interceptors.Auditable;
using SchoolApp.DAL;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;
using System.Security.Principal;
using System.Web;
using System;

namespace SchoolApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            InterceptorProvider.SetInterceptorProvider(new DefaultInterceptorProvider(new SoftDeleteChangeInterceptor()));
            //InterceptorProvider.SetInterceptorProvider(new DefaultInterceptorProvider(new AuditableChangeInterceptor(HttpContext.Current.User, new MyClock())));
        }
        //public class MyClock : IClock
        //{
        //    public DateTime Now { get { return DateTime.Now; } }
        //    public DateTime Today { get { return DateTime.Today; } }
        //}
    }
}