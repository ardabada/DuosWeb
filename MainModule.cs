using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Raw
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/raw"] = _ =>
            {
                return Response.AsText(getRaw());
            };
            Post["/raw"] = _ =>
            {
                return Response.AsText(getRaw());
            };
        }

        string getRaw()
        {
            var dict = ((DynamicDictionary)Request.Query);
            string q = string.Join("&", dict.ToDictionary().Select(x => x.Key + "=" + x.Value));


            string raw = Request.Method + " " + Request.Path + (string.IsNullOrEmpty(q) ? string.Empty : "?"+q) + " " + Request.ProtocolVersion + Environment.NewLine;
            raw += Request.UserHostAddress + Environment.NewLine;
            foreach (var r in Request.Headers)
            {
                foreach (var v in r.Value)
                    raw += r.Key + ": " + v + Environment.NewLine;
            }
            raw += Environment.NewLine;
            raw += new System.IO.StreamReader(Request.Body).ReadToEnd();
            return raw;
        }
    }
}