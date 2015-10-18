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
 *  http://demos.telerik.com/aspnet-ajax/grid/examples/programming/needdatasource/defaultcs.aspx
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using DAL;

namespace BLL.Store
{
    public static class Category
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

        #region CRUD

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static IQueryable DataSource()
        {
            int MaxDisplayOrder = GetMaxDisplayOrder();
            var query = from _StoreCategories in Utils.DbContext.StoreCategories
                        join _StoreCategoriesParent in Utils.DbContext.StoreCategories
                              on _StoreCategories.ParentCategoryId equals _StoreCategoriesParent.CategoryID
                        where _StoreCategories.PortalID == Category.PortalID
                        orderby _StoreCategories.DisplayOrder
                        select new
                        {
                            EntityKey = _StoreCategories.CategoryID,
                            _StoreCategories.CategoryID,
                            _StoreCategories.Name,
                            _StoreCategories.ParentCategoryId,
                            _StoreCategories.Deleted,
                            _StoreCategories.IsMenu,
                            MaxDisplayOrder,
                            ParentName = _StoreCategoriesParent.Name
                        };
            
            return query;

        }

        public static void Delete(int categoryID)
        {
            DAL.StoreCategory category = Utils.DbContext.StoreCategories.Where(p => p.CategoryID == categoryID).FirstOrDefault();
            Utils.DbContext.DeleteObject(category);
            Utils.DbContext.SaveChanges();
        }

        public static void Update(int categoryID, Hashtable values)
        {
            DAL.StoreCategory category = Utils.DbContext.StoreCategories.Where(p => p.CategoryID == categoryID).FirstOrDefault();
            category.Name = values["Name"].ToString();
            category.ParentCategoryId = int.Parse(values["ParentCategoryId"].ToString());
            category.Deleted = bool.Parse(values["Deleted"].ToString());
            category.IsMenu = bool.Parse(values["IsMenu"].ToString());
            Utils.DbContext.SaveChanges();
        }

        public static void Insert(Hashtable values)
        {

            int _ParentCategoryId = 0;

            if (values["ParentCategoryId"].ToString() != "")
            {
                _ParentCategoryId = int.Parse(values["ParentCategoryId"].ToString());
            }

            DAL.StoreCategory S = new DAL.StoreCategory
            {
                Name = values["Name"].ToString(),
                ParentCategoryId = _ParentCategoryId,
                Deleted = bool.Parse(values["Deleted"].ToString()),
                CreateDate = DateTime.Now,
                PortalID = Category.PortalID,
                DisplayOrder = GetMaxDisplayOrder() + 1,
                IsMenu = bool.Parse(values["IsMenu"].ToString())
            };

            Utils.DbContext.AddToStoreCategories(S);
            Utils.DbContext.SaveChanges();

            if (_ParentCategoryId == 0)
            {
                _ParentCategoryId = S.CategoryID;
                S.ParentCategoryId = _ParentCategoryId;
                Utils.DbContext.SaveChanges();
            }

        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static Hashtable GetStoreCategory(int EntityKey)
        {
            var values = new Hashtable();

            using (EntitiesContext context = new EntitiesContext())
            {
                var query = from q in Utils.DbContext.StoreCategories
                            where q.PortalID == Category.PortalID & q.CategoryID == EntityKey
                            select new
                            {
                                q.ParentCategoryId,
                                q.Deleted,
                                q.IsMenu
                            };

                foreach (var i in query)
                {
                    values["ParentCategoryId"] = i.ParentCategoryId;
                    values["Deleted"] = i.Deleted;
                    values["IsMenu"] = i.IsMenu;
                }

            }
            return values;
        }

        public static void MoveRecord(int EntityKey, bool Direction)
        {

            int MaxDisplayOrder = GetMaxDisplayOrder();
            int MinDisplayOrder = GetMinDisplayOrder();
            int SelectedDisplayOrder = GetSelectedDisplayOrder(EntityKey);
            var SelectedItem = Utils.DbContext.StoreCategories.Single(p => p.CategoryID == EntityKey);

            var query = from _StoreCategories in Utils.DbContext.StoreCategories
                        where _StoreCategories.PortalID == Category.PortalID
                        select _StoreCategories;

            if (Direction == true & SelectedDisplayOrder != MinDisplayOrder) //Move record up
            {
                var MoveItem = Utils.DbContext.StoreCategories.Single(p => p.DisplayOrder == SelectedDisplayOrder - 1 & p.PortalID == Category.PortalID);
                MoveItem.DisplayOrder = SelectedDisplayOrder;
                SelectedItem.DisplayOrder = SelectedDisplayOrder - 1;
                Utils.DbContext.SaveChanges();
            }

            if (Direction == true & SelectedDisplayOrder == MinDisplayOrder) //Move all records up 
            {
                foreach (var r in query)
                {
                    if (r.DisplayOrder != MinDisplayOrder)
                    {
                        r.DisplayOrder = r.DisplayOrder - 1;
                    }
                }
                SelectedItem.DisplayOrder = MaxDisplayOrder;
                Utils.DbContext.SaveChanges();
            }

            if (Direction == false & SelectedDisplayOrder != MaxDisplayOrder) //Move record down
            {
                var MoveItem = Utils.DbContext.StoreCategories.Single(p => p.DisplayOrder == SelectedDisplayOrder + 1 & p.PortalID == Category.PortalID);
                MoveItem.DisplayOrder = SelectedDisplayOrder;
                SelectedItem.DisplayOrder = SelectedDisplayOrder + 1;
                Utils.DbContext.SaveChanges();
            }

            if (Direction == false & SelectedDisplayOrder == MaxDisplayOrder) //Move all records down
            {
                foreach (var r in query)
                {
                    if (r.DisplayOrder != MaxDisplayOrder)
                    {
                        r.DisplayOrder = r.DisplayOrder + 1;
                    }
                }
                SelectedItem.DisplayOrder = MinDisplayOrder;
                Utils.DbContext.SaveChanges();
            }

        }

        public static int GetMaxDisplayOrder()
        {
            int MaxDisplayOrder = 0;

            var query = (from _StoreCategories in Utils.DbContext.StoreCategories
                         where _StoreCategories.PortalID == Category.PortalID
                         select _StoreCategories.DisplayOrder);

            if (query.Count() > 0)
            {
                MaxDisplayOrder = query.Max();
            }

            return (int)MaxDisplayOrder;
        }

        public static int GetMinDisplayOrder()
        {

            int MinDisplayOrder = 0;

            var query = (from _StoreCategories in Utils.DbContext.StoreCategories
                         where _StoreCategories.PortalID == Category.PortalID
                         select _StoreCategories.DisplayOrder);

            if (query.Count() > 0)
            {
                MinDisplayOrder = query.Min();
            }

            return (int)MinDisplayOrder;

        }

        public static int GetSelectedDisplayOrder(int EntityKey)
        {
            System.Nullable<int> SelectedDisplayOrder = (from q in Utils.DbContext.StoreCategories
                                                           where q.CategoryID == EntityKey
                                                           select (int)q.DisplayOrder).Single();
            return (int)SelectedDisplayOrder;
        }

        #endregion

        public static string StoreCategoryList()
        {
           
            StringBuilder sb = new StringBuilder();
            
            try
            {
                sb.Append(@"<ul data-role=""listview"" data-inset=""true"">");
                
                using (EntitiesContext context = new EntitiesContext())
                {
                    var query = from _StoreCategories in context.StoreCategories
                                where _StoreCategories.PortalID == Category.PortalID
                                orderby _StoreCategories.DisplayOrder
                                select _StoreCategories;

                    foreach (var c in query)
                    {
                        string[] array = { c.Name, "IsAjax" };
                        string PARAMS = String.Join(",", array);
                        sb.Append(@"<li><a onclick=""makePOSTRequest('section_body', 'PROJECT=BLL&NAMESPACE=BLL.Store&CLASS=Product&METHOD=ReturnStoreProducts&PARAMS=");

                        sb.Append(PARAMS);
                        sb.Append(String.Format(@"');"">{0}</a></li>", c.Name));
                    }

                }

                sb.Append(@"</ul>");

            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
            }

            return sb.ToString();

        }

    }

}
