﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                    result += "<h1>" + d.Date.ToLongTimeString() + "</h1><p>" + HttpUtility.HtmlEncode(d.Info) + "<p>";
                }

                return result;
            };
            Get["/tomtom/clear"] = _ =>
            {
                Data.Clear();
                return "cleared";
            };
        }

        string getRaw()
        {
            var dict = ((DynamicDictionary)Request.Query);
            string q = string.Join("&", dict.ToDictionary().Select(x => x.Key + "=" + x.Value));


            string raw = Request.Method + " " + Request.Path + (string.IsNullOrEmpty(q) ? string.Empty : "?" + q) + " " + Request.ProtocolVersion + Environment.NewLine;
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