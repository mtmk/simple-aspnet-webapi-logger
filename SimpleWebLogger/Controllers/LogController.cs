using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using log4net;

namespace SimpleWebLogger.Controllers
{
    public class LogController : ApiController
    {

        public async Task<string> Post()
        {
            string message = await Request.Content.ReadAsStringAsync();

            string logger = GetHeaderOrDefault("X-Logger", "main");
            string logLevel = GetHeaderOrDefault("X-LogLevel", "debug");

            ILog log = LogManager.GetLogger("client." + logger);

            if (logLevel == "error") log.Error(message);
            else if (logLevel == "warn") log.Warn(message);
            else if (logLevel == "info") log.Info(message);
            else log.Debug(message);

            return "OK " + DateTime.Now;
        }

        string GetHeaderOrDefault(string name, string defaultValue)
        {
            IEnumerable<string> xLogLevel;
            Request.Headers.TryGetValues(name, out xLogLevel);

            string value = defaultValue;

            if (xLogLevel != null)
                value = xLogLevel.FirstOrDefault() ?? defaultValue;

            return value.Trim().ToLowerInvariant();
        }

        public IEnumerable<string> Get()
        {
            var entry = new StringBuilder();

            foreach (var line in File.ReadAllLines(HostingEnvironment.MapPath("~/logs/client.log")))
            {
                //2015-02-28 14:05:25,099
                if (Regex.IsMatch(line, @"^\d{4}-\d\d-\d\d\s\d\d:\d\d:\d\d,\d{3}\s"))
                {
                    if (entry.Length > 0)
                    {
                        yield return entry.ToString();
                        entry.Clear();
                    }
                }

                entry.AppendLine(line);
            }

            if (entry.Length > 0)
                yield return entry.ToString();
        }

        public string Delete()
        {
            File.Delete(HostingEnvironment.MapPath("~/logs/client.log"));
            return "deleted";
        }

    }
}