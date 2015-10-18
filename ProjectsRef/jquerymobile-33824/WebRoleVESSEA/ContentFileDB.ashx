<%@ WebHandler Language="C#" Class="Web.ContentFileBrowserDB.Handler" %>

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
 *  http://www.telerik.com/community/forums/aspnet-ajax/file-explorer/bind-filesystemcontentprovider-to-database.aspx
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 * ###########################################################################################################################################################################
 * ###########################################################################################################################################################################
 * 
 * Todo I suspect something is wrong with file header sometimes in IE get the image with X box
 * Notice this more when testing Request.aspx?ACTION=About.aspx
 * 
 * ###########################################################################################################################################################################
 * ###########################################################################################################################################################################
 * 
 * 
 */

using System;
using System.Data;
using System.Configuration;
using System.Web;
using BLL;

namespace Web.ContentFileBrowserDB
{
    public class Handler : IHttpHandler
    {
        #region IHttpHandler Members

        private HttpContext _context;
        private HttpContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            Context = context;

            if (context.Request.QueryString["path"] == null)
            {
                return;
            }
			string path = Context.Server.UrlDecode(Context.Request.QueryString["path"]);

            DataRow itemRow = ContentFileDB.GetItemRow(path);
            
            if (itemRow == null)
            {
                return;
            }

            byte[] bytes = ContentFileDB.GetContent(path);
            
            if (bytes == null)
            {
                return;
            }

            //WriteFile1(bytes, itemRow["Name"].ToString(), itemRow["MimeType"].ToString(), context.Response);
            WriteFile2(bytes, itemRow["Name"].ToString(), itemRow["MimeType"].ToString(), context.Response);
        
        }

        /// <summary>
        /// Sends an image to the client
        /// </summary>
        /// <param name="content">binary file content</param>
        /// <param name="fileName">the filename to be sent to the client</param>
        /// <param name="contentType">the file content type</param>
        /// http://www.dotnetperls.com/response-writefile
        /// This repairs attachments in IE and Chrome
        private void WriteFile2(byte[] content, string fileName, string contentType, HttpResponse response)
        {
            response.ContentType = contentType;
            response.BinaryWrite(content);
            response.StatusCode = 200;
        }
        
        /// <summary>
        /// Sends a byte array to the client
        /// </summary>
        /// <param name="content">binary file content</param>
        /// <param name="fileName">the filename to be sent to the client</param>
        /// <param name="contentType">the file content type</param>
        /// http://www.telerik.com/community/forums/aspnet-ajax/file-explorer/bind-filesystemcontentprovider-to-database.aspx
        /// This method below if the file is to be returned as an attachment
        private void WriteFile1(byte[] content, string fileName, string contentType, HttpResponse response)
        {
            
            response.Buffer = true;
            response.Clear();
            response.ContentType = contentType;
            string extension = System.IO.Path.GetExtension(fileName);
            if (extension != ".htm" && extension != ".html" && extension != ".xml")
            {
                response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            }
            response.BinaryWrite(content);
            response.Flush();
            response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
    
}	