using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Nancy;

namespace DuosWeb
{
    public class MainModule : Nancy.NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ =>
            {
                string full = "HXFF: ";
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        full += addresses[0] + "; ";
                    }
                }

                full += "RA: " + context.Request.ServerVariables["REMOTE_ADDR"];
                var h = Request.Headers["X-Forwarded-For"];
                List<string> s = new List<string>(h);
                string r = s.FirstOrDefault();
                full  += "; XFF:" + r + "; UHA: " + Request.UserHostAddress + "; UA: " + Request.Headers.UserAgent;
                return full;
            };
        }
    }
}