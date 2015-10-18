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
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using DAL;

namespace BLL.Store
{
    public static class Product
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
            var query = from _StoreProducts in Utils.DbContext.StoreProducts
                        join _StoreCategories in Utils.DbContext.StoreCategories
                              on _StoreProducts.CategoryID equals _StoreCategories.CategoryID
                        where _StoreProducts.PortalID == Product.PortalID
                        select new
                        {
                            EntityKey = _StoreProducts.ProductID,
                            _StoreProducts.ProductID,
                            _StoreProducts.CategoryID,
                            _StoreProducts.Name,
                            _StoreProducts.Description,
                            _StoreProducts.Published,
                            _StoreProducts.Deleted,
                            _StoreProducts.Price,
                            CategoryName = _StoreCategories.Name
                        };

            return query;
        }

        public static void Delete(int EntityKey)
        {
            DAL.StoreProduct Product = Utils.DbContext.StoreProducts.Where(p => p.ProductID == EntityKey).Single();
            Utils.DbContext.DeleteObject(Product);
            Utils.DbContext.SaveChanges();
        }

        public static void Update(int EntityKey, Hashtable values)
        {
            DAL.StoreProduct Product = Utils.DbContext.StoreProducts.Where(p => p.ProductID == EntityKey).Single();
            Product.Name = values["Name"].ToString();
            Product.Description = values["Description"].ToString();
            Product.Price = decimal.Parse(values["Price"].ToString());
            Product.CategoryID = int.Parse(values["CategoryID"].ToString());
            Product.Deleted = bool.Parse(values["Deleted"].ToString());
            Utils.DbContext.SaveChanges();
        }

        public static void Insert(Hashtable values)
        {

            DAL.StoreProduct R = new DAL.StoreProduct
            {
                Name = values["Name"].ToString(),
                Description = values["Description"].ToString(),
                CategoryID = int.Parse(values["CategoryID"].ToString()),
                Price = decimal.Parse(values["Price"].ToString()),
                Deleted = bool.Parse(values["Deleted"].ToString()),
                CreateDate = DateTime.Now,
                PortalID = Product.PortalID
            };


            Utils.DbContext.AddToStoreProducts(R);
            Utils.DbContext.SaveChanges();
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public static Hashtable GetProduct(int EntityKey)
        {
            var values = new Hashtable();

            using (EntitiesContext context = new EntitiesContext())
            {
                var query = from q in Utils.DbContext.StoreProducts
                            where q.PortalID == Product.PortalID & q.ProductID == EntityKey
                            select new
                            {
                                q.ProductID,
                                q.CategoryID,
                                q.Name,
                                q.Description,
                                q.Published,
                                q.Deleted,
                                q.Price
                            };

                foreach (var i in query)
                {
                    values["ProductID"] = i.ProductID;
                    values["CategoryID"] = i.CategoryID;
                    values["Name"] = i.Name;
                    values["Description"] = i.Description;
                    values["Published"] = i.Published;
                    values["Deleted"] = i.Deleted;
                    values["Price"] = i.Price;
                }

            }
            return values;
        }

        #endregion

        public static string ReturnStoreProducts(string PARAMS = null)
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

            sb.Append(@"<h1>");
            sb.Append(PARAM1);
            sb.Append(@"</h1><hr />");

            using (EntitiesContext context = new EntitiesContext())
            {
                var query = from _StoreProducts in Utils.DbContext.StoreProducts
                            join _StoreCategories in Utils.DbContext.StoreCategories
                                  on _StoreProducts.CategoryID equals _StoreCategories.CategoryID
                            where _StoreCategories.Name == PARAM1 & _StoreCategories.PortalID == Product.PortalID
                            select new
                            {
                                _StoreProducts.ProductID,
                                _StoreProducts.CategoryID,
                                _StoreProducts.Name,
                                _StoreProducts.Description,
                                _StoreProducts.Published,
                                _StoreProducts.Deleted,
                                _StoreProducts.Price
                            };

                foreach (var i in query)
                {
                    string[] array = { i.ProductID.ToString(), "IsAjax" };
                    string _PARAMS = String.Join(",", array);

                    //sb.Append(@"<div style=""font-weight: bold"">");
                    //sb.Append(@"<a href=""#"" onclick=""makePOSTRequest('section_body', 'PROJECT=BLL&NAMESPACE=BLL.Store&CLASS=Product&METHOD=ReturnStoreProduct&PARAMS=" + _PARAMS + @"');"">");
                    //sb.Append(i.Name + " - " + String.Format("{0:C}", i.Price));
                    //sb.Append(@"</a></div><div>" + i.Description + @"</div><hr />");

                    sb.Append(@"<div style=""font-weight: bold"">");
                    sb.Append(i.Name + " - " + String.Format("{0:C}", i.Price));
                    sb.Append(@"</div><div>" + i.Description + @"</div><hr />");

                    sb.Append(@"<div data-role=""fieldcontain"">");

                    //sb.Append(@"<a href=""#"" data-role=""button"" data-icon=""plus"">Add to Cart</a>");

                    sb.Append(@"<a href=""#"" data-role=""button"" data-icon=""plus"" onclick=""makePOST('PROJECT=BLL&NAMESPACE=BLL.Store&CLASS=Product&METHOD=AddStoreProduct&PARAMS=" + _PARAMS + @"');"">");

                    sb.Append(i.Name);

                    sb.Append(@"</a>");

                    sb.Append(@"</div>");


                }

            }

            return sb.ToString();

        }

        public static string ReturnStoreProduct(string PARAMS = null)
        {
            StringBuilder sb = new StringBuilder();

            string[] VALUES = null;
            string PARAM1 = null;
            int ProductID = 0;

            if (PARAMS != null)
            {
                VALUES = PARAMS.Split(',');

                if (VALUES[0] != null)
                {
                    PARAM1 = VALUES[0].ToString();
                    ProductID = int.Parse(PARAM1);
                }
            }


            using (EntitiesContext context = new EntitiesContext())
            {
                var R = context.StoreProducts.Single(q => q.ProductID == ProductID & q.PortalID == Product.PortalID);

                sb.Append(@"<div style=""font-weight: bold"">");
                sb.Append(R.Name + " - " + String.Format("{0:C}", R.Price));
                sb.Append(@"</div><div>" + R.Description + @"</div><hr />");

                //sb.Append(@"<div data-role=""fieldcontain"">");
                //sb.Append(@"<a href=""#"" data-role=""button"" data-icon=""plus"">Add to Cart</a>");
                //sb.Append(@"</div>"); 

            }

            return sb.ToString();

        }

        public static int GetCartID()
        {
            int CartID = 0;

            if (HttpContext.Current.Request.Cookies["CartID"] == null)
            {
                DAL.StoreCart R = new DAL.StoreCart
                {
                    CreateDate = DateTime.Now,
                    PortalID = Product.PortalID
                };
                Utils.DbContext.AddToStoreCarts(R);
                Utils.DbContext.SaveChanges();
                CartID = R.CartID;
                HttpCookie CookieCartID = new HttpCookie("CartID");
                CookieCartID.Value = Crypto.Encrypt(CartID.ToString(), ConfigurationManager.AppSettings.Get("PASSPHRASESALT"));
                CookieCartID.Expires = DateTime.Now.AddMinutes(15);
                HttpContext.Current.Response.AppendCookie(CookieCartID);
            }
            else
            {
                CartID = int.Parse(Crypto.Decrypt(HttpContext.Current.Request.Cookies["CartID"].Value.ToString(), ConfigurationManager.AppSettings.Get("PASSPHRASESALT")));
            }

            return CartID;

        }

        public static void AddStoreProduct(string PARAMS = null)
        {
            string[] VALUES = null;
            int _CartID = GetCartID();
            int _ProductID = 0;

            if (PARAMS != null)
            {
                VALUES = PARAMS.Split(',');
                if (VALUES[0] != null)
                {
                    _ProductID = int.Parse(VALUES[0].ToString());
                }
            }

            using (EntitiesContext context = new EntitiesContext())
            {
                context.usp_StoreInsertCartItem(_CartID, 1, _ProductID, Product.PortalID);
            }

        }

    }

}
