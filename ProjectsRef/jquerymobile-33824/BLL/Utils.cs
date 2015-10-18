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
 * URLS
 * http://blog.pedroliska.com/2012/02/09/linq-query-to-csv/
 * http://forums.asp.net/t/1559861.aspx/1
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using DAL;

namespace BLL
{
    public static class Utils
    {
        /// <summary>
        /// Gets the the EntitiesContext from the http collection or creates a new instance
        /// if one does not already exist. This makes sure that all instances of the db context
        /// are the same for the current request, and that the same context object is not used
        /// across multiple requests.
        /// </summary>
        public static EntitiesContext DbContext
        {
            get
            {
                EntitiesContext _dbContext = null;
                object temp = HttpContext.Current.Items["EntitiesContext"];
                if (temp == null)
                {
                    _dbContext = new EntitiesContext();
                    HttpContext.Current.Items["EntitiesContext"] = _dbContext;
                }
                else
                {
                    _dbContext = temp as EntitiesContext;
                }

                return _dbContext;
            }
        }

        /// <summary>
        ///     Gets the website root URL
        /// </summary>
        /// 
        
        public static string ReturnRootUrl()
        {
            //AppHarbor has a bug that is attaching a port number setting port to false will remove the port
            //ToDo: Verify all sections in the application that calls this method
            //http://stackoverflow.com/questions/7674850/get-original-url-without-non-standard-port-c
            //http://support.appharbor.com/kb/getting-started/workaround-for-generating-absolute-urls-without-port-number
            //http://blogs.msdn.com/b/davidlem/archive/2009/01/27/windows-azure-accessing-the-request-url-property.aspx

            string host = System.Web.HttpContext.Current.Request.Headers["Host"];

            string protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
            {
                protocol = "http://";
            }
            else
            {
                protocol = "https://";
            }

            string returnUrl = String.Format("{0}{1}/", protocol, host);

            //ElmahExtension.LogToElmah(new ApplicationException("ReturnURL=" + returnUrl));

            return returnUrl;

        }
        
        /// <summary>
        /// This method generates CSV data out an array of anything. The array
        /// elements could be of anonymous type
        /// </summary>
        public static string GenerateCsv(Array items)
        {
            var rowType = items.GetType().GetElementType();
            var colNames = rowType.GetProperties().Select(p => p.Name).ToArray();

            // create the header row
            var retVal = new StringBuilder();
            retVal.AppendLine(string.Join(",", colNames));

            // now create the body
            foreach (var row in items)
            {
                var rowItems = new string[colNames.Length];
                for (int i = 0; i < colNames.Length; i++)
                    rowItems[i] = FormatCell(rowType.GetProperty(colNames[i]).GetValue(row, null));
                retVal.AppendLine(string.Join(",", rowItems));
            }

            return retVal.ToString();
        }

        private static string FormatCell(object item)
        {
            if (item == null)
                return string.Empty;

            return item.ToString().Replace("\"", "\"\"");
        }

        /// <summary>
        /// Converts an IEnumerable list into a DataTable object
        /// </summary>
        /// <typeparam name="T">The type of the objects in the list</typeparam>
        /// <param name="varlist">The list of objects</param>
        /// <returns>Returns a DataTable object</returns>
        public static DataTable LINQToDataTable<T>(this IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow

                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }


                DataRow dr = dtReturn.NewRow();


                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        /// <summary>
        /// Applies the XSL transform file to an XML document
        /// </summary>
        /// <param name="doc">The XML document</param>
        /// <param name="xslFile">The XSL file</param>
        /// <returns>Returns a string containing the transformed XML</returns>
        public static string TransformXSLT(this XmlDocument doc, string xslFile)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            XslCompiledTransform xslt = new XslCompiledTransform();

            xslt.Load(ReturnRootUrl() + "XSLT/" + xslFile);

            using (XmlTextWriter xtw = new XmlTextWriter(new StringWriter(sb)))
            {
                xslt.Transform(doc, xtw);
                xtw.Flush();
            }

            return sb.ToString();
        }

        public static bool ApprovedAjaxMethods(string METHOD)
        {
            bool IsApproved = false;

            switch (METHOD)
            {
                case "ReadContent":
                    IsApproved = true;
                    break;

                case "ReturnSurveyXML":
                    IsApproved = true;
                    break;

                case "PostSurveyResults":
                    IsApproved = true;
                    break;

                case "StoreCategoryList":
                    IsApproved = true;
                    break;

                case "ReturnStoreProducts":
                    IsApproved = true;
                    break;

                case "ReturnStoreProduct":
                    IsApproved = true;
                    break;
                case "AddStoreProduct":
                    IsApproved = true;
                    break;
            }

            return IsApproved;

        }

        public static bool AppDebug()
        {
            return bool.Parse(ConfigurationManager.AppSettings.Get("AppDebug"));
        }

        public static string AjaxResponse()
        {
            string HTML = null;
            string PROJECT = null;
            string NAMESPACE = null;
            string CLASS = null;
            string METHOD = null;
            string PARAMS = null;

            if (HttpContext.Current.Request.Form["PROJECT"] != null & HttpContext.Current.Request.Form["NAMESPACE"] != null & HttpContext.Current.Request.Form["CLASS"] != null & HttpContext.Current.Request.Form["METHOD"] != null)
            {
                PROJECT = HttpContext.Current.Request.Form["PROJECT"];
                NAMESPACE = HttpContext.Current.Request.Form["NAMESPACE"];
                CLASS = HttpContext.Current.Request.Form["CLASS"];
                METHOD = HttpContext.Current.Request.Form["METHOD"];
                PARAMS = HttpContext.Current.Request.Form["PARAMS"];

                if (BLL.Utils.ApprovedAjaxMethods(METHOD)) //Only allow approved methods to secure the request
                {

                    HTML = BLL.TypeUtils.InvokeStringMethod5(PROJECT, NAMESPACE, CLASS, METHOD, PARAMS);

                    if (BLL.Utils.AppDebug()) //Create Ajax debug link
                    {
                        HTML = HTML + (@"<a href=""/Requests.aspx?PROJECT=" + PROJECT) + (@"&NAMESPACE=" + NAMESPACE) + (@"&CLASS=" + CLASS) + (@"&METHOD=" + METHOD) + (@"&PARAMS=" + PARAMS) + (@"&VIEWHTML=YES""target=""_blank"">DEBUG</a>");
                    }
                }
            }

            if (HttpContext.Current.Request.QueryString["PROJECT"] != null & HttpContext.Current.Request.QueryString["NAMESPACE"] != null & HttpContext.Current.Request.QueryString["CLASS"] != null & HttpContext.Current.Request.QueryString["METHOD"] != null & HttpContext.Current.Request.QueryString["VIEWHTML"] != null) //To allow debug of AJAX HTML
            {
                PROJECT = HttpContext.Current.Request.QueryString["PROJECT"];
                NAMESPACE = HttpContext.Current.Request.QueryString["NAMESPACE"];
                CLASS = HttpContext.Current.Request.QueryString["CLASS"];
                METHOD = HttpContext.Current.Request.QueryString["METHOD"];
                PARAMS = HttpContext.Current.Request.QueryString["PARAMS"];

                if (BLL.Utils.ApprovedAjaxMethods(HttpContext.Current.Request.QueryString["METHOD"]) & HttpContext.Current.Request.QueryString["VIEWHTML"] == "YES") //Only allow approved methods to secure the request
                {
                    HTML = (@"<textarea style=""height:100%; width:100%;"">") + BLL.TypeUtils.InvokeStringMethod5(PROJECT, NAMESPACE, CLASS, METHOD, PARAMS) + (@"</textarea>");
                }
            }

            return HTML;
        }

        public static void AppDataRemoveReadOnly()
        {
            string Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("bin", "App_Data").Replace(@"file:\", "");
            DirectoryInfo di = new DirectoryInfo(Path);
            di.Attributes = FileAttributes.Normal;

            foreach (string fileName in System.IO.Directory.GetFiles(Path))
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
                fileInfo.Attributes = FileAttributes.Normal;
            }
        }
    }
}
