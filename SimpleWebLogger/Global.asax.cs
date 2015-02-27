using System.IO;
using System.Web.Http;

namespace SimpleWebLogger
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/log4net.config")));

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
