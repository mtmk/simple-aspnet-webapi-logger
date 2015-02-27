using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using log4net;

namespace SimpleWebLogger.Controllers
{
    public class LogController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger("client.logs");

        public async Task<string> Post()
        {
            string message = await Request.Content.ReadAsStringAsync();

            IEnumerable<string> xLogLevel;
            Request.Headers.TryGetValues("X-LogLevel", out xLogLevel);

            string logLevel = "DEBUG";

            if (xLogLevel != null)
            {
                logLevel = (xLogLevel.FirstOrDefault() ?? "DEBUG").ToUpperInvariant();
            }

            if (logLevel.Contains("ERROR"))
            {
                Log.Error(message);
            }
            else if (logLevel.Contains("WARN"))
            {
                Log.Warn(message);
            }
            else if (logLevel.Contains("INFO"))
            {
                Log.Info(message);
            }
            else
            {
                Log.Debug(message);
            }

            return "OK";
        }

        public string[] Get()
        {
            return File.ReadAllLines(HostingEnvironment.MapPath("~/logs/client.log"));
        }

        public string Delete()
        {
            File.Delete(HostingEnvironment.MapPath("~/logs/client.log"));
            return "deleted";
        }

    }
}