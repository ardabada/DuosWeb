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
                var h = Request.Headers["X-Forwarded-For"];
                List<string> s = new List<string>(h);
                string r = s.FirstOrDefault();
                string result = "XFF:" + r + "; UHA: " + Request.UserHostAddress + "; UA: " + Request.Headers.UserAgent;
                return result;
            };
        }
    }
}