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
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

namespace BLL
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using DAL;

    /// <summary>
    /// A blog sitemap suitable for Google Sitemap as well as
    ///     other big search engines such as MSN/Live, Yahoo and Ask.
    /// </summary>
    public class SiteMap : IHttpHandler
    {
        #region Properties

        /// <summary>
        ///     Gets a value indicating whether another request can use the <see cref = "T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref = "T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the value of the current portal ID
        /// </summary>
        public int PortalID
        {
            get
            {
                object temp = HttpContext.Current.Session["PortalID"];
                return temp == null ? 1 : (int)temp;
            }
        }
        #endregion

        #region Implemented Interfaces

        #region IHttpHandler

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that 
        ///     implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        /// </summary>
        /// <param name="response">
        /// An <see cref="T:System.Web.HttpContext"></see> 
        ///     object that provides references to the intrinsic server objects 
        ///     (for example, Request, Response, Session, and Server) used to service HTTP requests.
        /// </param>
        public void ProcessRequest(HttpContext response)
        {
            using (var writer = XmlWriter.Create(response.Response.OutputStream))
            {
                writer.WriteStartElement("urlset", "http://www.google.com/schemas/sitemap/0.84");

                try
                {
                    using (EntitiesContext context = new EntitiesContext())
                    {
                        var query = from q in context.Contents
                                    where q.PortalID == this.PortalID & q.IsPublished == true
                                    select q;

                        foreach (var c in query)
                        {
                            writer.WriteStartElement("url");
                            writer.WriteElementString("loc", string.Format("{0}Pages/" + c.URL, Utils.ReturnRootUrl()));
                            writer.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                            writer.WriteElementString("changefreq", "monthly");
                            writer.WriteEndElement();
                        }
                    }
                }
                
                catch (Exception ex)
                {
                    ElmahExtension.LogToElmah(ex);
                }
                
                writer.WriteEndElement();

            }

            response.Response.ContentType = "text/xml";
        }

        #endregion

        #endregion
    }
}