/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  URLS
 *  http://msdn.microsoft.com/en-us/library/system.web.httprequest.userhostaddress.aspx
 *  http://msdn.microsoft.com/en-us/library/system.web.httpbrowsercapabilities.aspx
 *  http://msdn.microsoft.com/en-us/library/3yekbd5b.aspx
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using System.Linq;
using System.Web;
using DAL;

namespace BLL
{

    public static class SiteLog
    {
        /// <summary>
        /// Gets the value of the current Portal ID from the current Session.
        /// If the session value is null it returns 1.
        /// </summary>
        public static int PortalID
        {
            get
            {
                object temp = HttpContext.Current.Session["PortalID"];
                return temp == null ? 1 : (int)temp;
            }
        }

        /// <summary>
        /// SiteLog functions
        /// </summary>
        /// <returns>Returns Linq datatable</returns>

        public static IQueryable DataSource()
        {
            var query = from q in Utils.DbContext.SiteLogs
                        orderby q.SiteLogID descending
                        select new
                        {
                            //q.PortalID,
                            Date = q.CreateDate,
                            URL = "http://" + q.Host + q.FileName,
                            q.Ajax,
                            q.UserID,
                            IP = q.RequestUserHostAddress,
                            Browser = q.BrowserType,
                            Platform = q.BrowserPlatform,
                            Height = q.ClientHeight,
                            Width = q.ClientWidth
                        };
            return query;
        }

        public static void InsertLog(int _ClientHeight, int _ClientWidth, string _FileName, bool _Ajax)
        {

            try
            {

                string _Host = HttpContext.Current.Request.Url.Host;

                int _UserID = 0;
                HttpBrowserCapabilities r = HttpContext.Current.Request.Browser;
                if (HttpContext.Current.Session["UserID"] != null)
                {
                    _UserID = (int)HttpContext.Current.Session["UserID"];
                }
                using (EntitiesContext context = new EntitiesContext())
                {
                    DAL.SiteLog L = new DAL.SiteLog
                    {
                        PortalID = SiteLog.PortalID,
                        CreateDate = DateTime.Now,
                        UserID = _UserID,
                        Ajax = _Ajax,
                        Host = _Host,
                        FileName = _FileName.Replace("ACTION=", "/"),
                        RequestRequestType = HttpContext.Current.Request.RequestType,
                        RequestUserHostAddress = HttpContext.Current.Request.UserHostAddress,
                        RequestUserHostName = HttpContext.Current.Request.UserHostName,
                        RequestHttpMethod = HttpContext.Current.Request.HttpMethod,
                        ClientHeight = _ClientHeight,
                        ClientWidth = _ClientWidth,
                        Browser = r.Browser,
                        BrowserType = r.Type,
                        BrowserVersion = decimal.Parse(r.Version),
                        BrowserMajorVersion = r.MajorVersion,
                        BrowserPlatform = r.Platform,
                        BrowserCrawler = r.Crawler,
                        BrowserFrames = r.Frames,
                        BrowserTables = r.Tables,
                        BrowserCookies = r.Cookies,
                        BrowserVBScript = r.VBScript,
                        BrowserEcmaScriptVersion = decimal.Parse(r.EcmaScriptVersion.ToString()),
                        BrowserJavaApplets = r.JavaApplets,
                        BrowserActiveXControls = r.ActiveXControls,
                        BrowserJavaScriptVersion = decimal.Parse(r["JavaScriptVersion"])
                    };
                    context.SiteLogs.AddObject(L);
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
            }

        }

    }

}
