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
 *  URLS: 
 *  http://www.thomsonchemmanoor.com/10-common-mistakes-using-robotstxt-on-your-website.html
 *  http://tool.motoricerca.info/robots-checker.phtml
 *  http://googlewebmastercentral.blogspot.com/2008/03/speaking-language-of-robots.html
 *  
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

//Todo test to see if Google will download the robots.txt as an attachment

namespace BLL
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Web;
    using DAL;

    public class Robots : IHttpHandler
    {
        #region Properties

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IHttpHandler

        public void ProcessRequest(HttpContext _context)
        {

            StringBuilder sb = new StringBuilder();
            
            Int32 PortalID;
            
            string RobotsText;

            using (EntitiesContext context = new EntitiesContext())
            {
                var P = context.PortalAlias.Single(p => p.HTTPAlias == HttpContext.Current.Request.Url.Host);
                PortalID = P.PortalID;
            }

            using (EntitiesContext context = new EntitiesContext())
            {
                var P = context.Portals.Single(p => p.PortalID == PortalID);
                RobotsText = P.RobotsText;
            }

            byte[] bytes = StrToByteArray(RobotsText);
            WriteFile(bytes, "text/plain", _context.Response);

        }

        /// <summary>
        /// Returns the file in the browser
        /// </summary>
        private static void WriteFile(byte[] content, string contentType, HttpResponse response)
        {
            response.ContentType = contentType;
            //response.AddHeader("content-disposition", "filename=" + "robots.txt");
            response.BinaryWrite(content);
        }


        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        #endregion

        #endregion
    }

}