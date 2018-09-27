using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuosWeb
{
    public class MainModule : Nancy.NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ =>
            {
                string result = "UHA: " + Request.UserHostAddress + "; UA: " + Request.Headers.UserAgent;
                return result;
            };
        }
    }
}