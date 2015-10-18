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
 * Author: Brad Brundange
 *  
 */


using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using DAL;

namespace BLL
{

    public static class Content
    {
        /// <summary>
        /// Gets the value of the Portal ID for the current session
        /// </summary>
        public static int PortalID
        {
            get
            {
                object temp = HttpContext.Current.Session["PortalID"]; 
                return temp == null ? 1 : (int)temp;
            }
        }

        #region CRUD

        /// <summary>
        /// Content RadTable functions
        /// </summary>
        /// 

        public static IQueryable DataSource()
        {
            int MaxDisplayOrder = GetMaxDisplayOrder();
            var query = from _Contents in Utils.DbContext.Contents
                        join _ContentTypes in Utils.DbContext.ContentTypes on _Contents.ContentTypeID equals _ContentTypes.ContentTypeID
                        orderby _Contents.DisplayOrder
                        where _Contents.PortalID == Content.PortalID
                        select new
                        {
                            EntityKey = _Contents.ContentID,
                            _Contents.ContentID,
                            _Contents.Title,
                            _Contents.URL,
                            _Contents.ContentText,
                            _Contents.ContentTypeID,
                            _Contents.IsPublished,
                            _Contents.IsMenu,
                            ContentTypeName = _ContentTypes.Name,
                            _Contents.StartMethod,
                            MaxDisplayOrder
                        };
            return query;
        }

        public static IQueryable DDPagesDataSource(int _PortalID)
        {
            var query = from _Contents in Utils.DbContext.Contents
                        orderby _Contents.DisplayOrder
                        where _Contents.PortalID == _PortalID & _Contents.ContentTypeID == 1
                        select new
                        {
                            EntityKey = _Contents.ContentID,
                            _Contents.ContentID,
                            _Contents.URL
                        };
            return query;
        }

        public static void Create(Hashtable values)
        {
            DAL.Content r = new DAL.Content
            {
                PortalID = BLL.Content.PortalID,
                Title = values["Title"].ToString(),
                URL = values["Title"] + ".aspx",
                ContentText = values["ContentText"].ToString(),
                ContentTypeID = int.Parse(values["ContentTypeID"].ToString()),
                IsPublished = bool.Parse(values["Published"].ToString()),
                IsMenu = bool.Parse(values["IsMenu"].ToString()),
                UserID = (int)HttpContext.Current.Session["UserID"],
                DisplayOrder = GetMaxDisplayOrder() + 1,
                CreateDate = DateTime.Now,
                StartMethod = values["StartMethod"].ToString()
            };
            Utils.DbContext.AddToContents(r);
            Utils.DbContext.SaveChanges();
        }

        public static Hashtable Read(int EntityKey)
        {
            var values = new Hashtable();

            using (EntitiesContext context = new EntitiesContext())
            {
                var query = from q in Utils.DbContext.Contents
                            where q.ContentID == EntityKey
                            select new
                            {
                                q.ContentID,
                                q.ContentText,
                                q.ContentTypeID,
                                q.IsPublished,
                                q.IsMenu,
                                q.Title,
                                q.StartMethod
                            };

                foreach (var i in query)
                {
                    values["ContentID"] = i.ContentID;
                    values["ContentText"] = i.ContentText;
                    values["ContentTypeID"] = i.ContentTypeID;
                    values["IsPublished"] = i.IsPublished;
                    values["IsMenu"] = i.IsMenu;
                    values["Title"] = i.Title;
                    values["StartMethod"] = i.StartMethod;
                }
            }

            return values;
        }

        public static void Update(int EntityKey, Hashtable values)
        {
            DAL.Content r = Utils.DbContext.Contents.Where(p => p.ContentID == EntityKey).FirstOrDefault();
            r.PortalID = BLL.Content.PortalID;
            r.Title = values["Title"].ToString();
            r.URL = values["Title"] + ".aspx";
            r.ContentText = values["ContentText"].ToString();
            r.ContentTypeID = int.Parse(values["ContentTypeID"].ToString());
            r.IsPublished = bool.Parse(values["Published"].ToString());
            r.IsMenu = bool.Parse(values["IsMenu"].ToString());
            r.UserID = (int)HttpContext.Current.Session["UserID"];
            r.CreateDate = DateTime.Now;
            r.StartMethod = values["StartMethod"].ToString();
            Utils.DbContext.SaveChanges();
        }

        public static void Delete(int EntityKey)
        {
            DAL.Content r = Utils.DbContext.Contents.Where(p => p.ContentID == EntityKey).FirstOrDefault();
            Utils.DbContext.DeleteObject(r);
            Utils.DbContext.SaveChanges();
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        #endregion

        #region Move Record

        public static void MoveRecord(int EntityKey, bool Direction)
        {

            var query = from _Contents in Utils.DbContext.Contents
                        where _Contents.PortalID == BLL.Content.PortalID
                        orderby _Contents.DisplayOrder ascending
                        select _Contents;
            
            int MaxDisplayOrder = GetMaxDisplayOrder();
            int MinDisplayOrder = GetMinDisplayOrder();
            int SelectedDisplayOrder = GetSelectedDisplayOrder(EntityKey);
            int PreviouseID = 0;
            int OrderID = 1;
            bool IsNext = false;

            var SelectedItem = Utils.DbContext.Contents.Single(p => p.ContentID == EntityKey);

            if (Direction == true & SelectedDisplayOrder != MinDisplayOrder) //Move record up
            {
                foreach (var r in query)
                {
                    if (r.ContentID == EntityKey)
                    {
                        break;
                    }
                    PreviouseID = r.ContentID;
                }

                foreach (var r in query)
                {
                    if (r.ContentID == PreviouseID)
                    {
                        r.DisplayOrder = OrderID + 1;
                    }
                    if (r.ContentID == EntityKey)
                    {
                        r.DisplayOrder = OrderID - 1;
                    }
                    if (r.ContentID != EntityKey & r.ContentID != PreviouseID)
                    {
                        r.DisplayOrder = OrderID;
                    }
                    OrderID++;
                }
                Utils.DbContext.SaveChanges();
            }

            if (Direction == true & SelectedDisplayOrder == MinDisplayOrder) //Move all records up 
            {
                int DisplayOrder = 1;

                foreach (var r in query)
                {
                    if (r.DisplayOrder != SelectedItem.DisplayOrder)
                    {
                        r.DisplayOrder = DisplayOrder;
                        DisplayOrder++;
                    }
                }
                SelectedItem.DisplayOrder = MaxDisplayOrder;
                Utils.DbContext.SaveChanges();
            }

            if (Direction == false & SelectedDisplayOrder != MaxDisplayOrder) //Move record down
            {
                foreach (var r in query)
                {
                    if (r.ContentID != EntityKey & IsNext == false)
                    {
                        r.DisplayOrder = OrderID;
                    }
                    if (IsNext)
                    {
                        r.DisplayOrder = OrderID - 1;
                        IsNext = false;
                    }

                    if (r.ContentID == EntityKey)
                    {
                        r.DisplayOrder = OrderID + 1;
                        IsNext = true;
                    }
                    OrderID++;
                }

                Utils.DbContext.SaveChanges();
            }

            if (Direction == false & SelectedDisplayOrder == MaxDisplayOrder) //Move all records down
            {
                int DisplayOrder = 2;

                foreach (var r in query)
                {
                    if (r.DisplayOrder != SelectedItem.DisplayOrder)
                    {
                        r.DisplayOrder = DisplayOrder;
                        DisplayOrder++;
                    }
                }
                SelectedItem.DisplayOrder = 1;
                Utils.DbContext.SaveChanges();
            }

        }

        public static int GetMaxDisplayOrder()
        {
            int MaxDisplayOrder = 0;

            var query = (from _Contents in Utils.DbContext.Contents
                         where _Contents.PortalID == BLL.Content.PortalID
                         select _Contents.DisplayOrder);

            if (query.Count() > 0)
            {
                MaxDisplayOrder = query.Count();
            }

            return (int)MaxDisplayOrder;
        }

        public static int GetMinDisplayOrder()
        {

            int MinDisplayOrder = 0;

            var query = (from _Contents in Utils.DbContext.Contents
                         where _Contents.PortalID == BLL.Content.PortalID
                         select _Contents.DisplayOrder);

            if (query.Count() > 0)
            {
                MinDisplayOrder = query.Min();
            }

            return (int)MinDisplayOrder;

        }

        public static int GetSelectedDisplayOrder(int EntityKey)
        {
            System.Nullable<int> SelectedDisplayOrder = (from q in Utils.DbContext.Contents
                                                           where q.ContentID == EntityKey
                                                           select (int)q.DisplayOrder).Single();
            return (int)SelectedDisplayOrder;
        }

        #endregion

        public static string ReadContent(string PARAMS = null)
        {

            StringBuilder sb = new StringBuilder();

            string[] VALUES = null;
            string PARAM1 = null;

            if (PARAMS != null)
            {
                VALUES = PARAMS.Split(',');

                if (VALUES[0] != null)
                {
                    PARAM1 = VALUES[0].ToString();
                }
            }

            try
            {

                using (EntitiesContext context = new DAL.EntitiesContext())
                {

                    var c = context.Contents.FirstOrDefault(q => q.URL == PARAM1 & q.PortalID == Content.PortalID);

                    if (c != null)
                    {
                        sb.Append(c.ContentText);
                    }

                }

            }

            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
            }

            return HttpUtility.HtmlDecode(sb.ToString());

        }

        public static string ReadContentListView()
        {

            StringBuilder sb = new StringBuilder();
            string[] file = HttpContext.Current.Request.CurrentExecutionFilePath.Split('/');
            string fileName = file[file.Length - 1];

            try
            {
                sb.Append(@"<div data-role=""collapsible"">");
                sb.Append(@"<h3>More in this section</h3>");
                sb.Append(@"<ul data-role=""listview"">");

                using (EntitiesContext context = new EntitiesContext())
                {
                    var query = from q in context.Contents
                                where q.PortalID == Content.PortalID & q.IsPublished == true & q.IsMenu == true & q.ContentTypeID == 1
                                orderby q.DisplayOrder
                                select q;

                    foreach (var c in query)
                    {

                        if (c.StartMethod == null || c.StartMethod == "")
                        {
                            string[] array = { c.URL, "IsAjax" };
                            string PARAMS = String.Join(",", array);
                            sb.Append(@"<li><a onclick=""makePOSTRequest('section_body', 'PROJECT=BLL&NAMESPACE=BLL&CLASS=Content&METHOD=ReadContent&PARAMS=");
                            sb.Append(PARAMS);
                            sb.Append(String.Format(@"');"">{0}</a></li>", c.Title));
                        }
                        else
                        {
                            sb.Append(@"<li><a onclick=""makePOSTRequest('section_body', '");
                            sb.Append(c.StartMethod);
                            sb.Append(String.Format(@"');"">{0}</a></li>", c.Title));
                        }

                    }

                }

                sb.Append(@"</ul></div>");

            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
            }

            return sb.ToString();

        }
    }
}
