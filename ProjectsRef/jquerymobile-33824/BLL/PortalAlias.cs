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

using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;


namespace BLL
{

    public class PortalAlias
    {
        #region CRUD OPERATIONS

        /// <summary>
        /// Fetches Portal Alias information
        /// </summary>
        /// <returns>Returns a list with EntityKey, HttpAlias, PortalID, and the Portal Name</returns>
        public static IQueryable DataSource()
        {
            using (var context = new EntitiesContext())
            {
                var query = from _PortalAlias in context.PortalAlias
                            join _Portals in context.Portals 
                            on _PortalAlias.PortalID equals _Portals.PortalID
                            select new
                            {
                                EntityKey = _PortalAlias.PortalAliasID,
                                _PortalAlias.HTTPAlias,
                                _PortalAlias.PortalID,
                                PortalName = _Portals.Name
                            };
                return query;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityKey"></param>
        public static void Delete(int entityKey)
        {
            using (var context = new EntitiesContext())
            {
                DAL.PortalAlia r = context.PortalAlias.Single(p => p.PortalAliasID == entityKey);
                context.DeleteObject(r);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a PortalAlias entity by its entiy key using a hashtable of values.
        /// </summary>
        /// <param name="entityKey">The entity key of the PortalAlias entity</param>
        /// <param name="values">A hashtable of values</param>
        public static void Update(int entityKey, Hashtable values)
        {
            using (var context = new EntitiesContext())
            {
                DAL.PortalAlia r = context.PortalAlias.Single(p => p.PortalAliasID == entityKey);
                r.PortalID = int.Parse(values["PortalID"].ToString());
                r.HTTPAlias = values["HTTPAlias"].ToString();
                r.LastUpdated = DateTime.Now;
                r.LastModifiedByUserID = (int)HttpContext.Current.Session["UserID"];
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Uses a hashtable of values to create and insert a new PortalAlias entity into the database
        /// </summary>
        /// <param name="values">The hashtable</param>
        public static void Insert(Hashtable values)
        {
            using (var context = new EntitiesContext())
            {
                DAL.PortalAlia r = new DAL.PortalAlia
                {
                    CreateDate = DateTime.Now,
                    PortalID = int.Parse(values["PortalID"].ToString()),
                    HTTPAlias = values["HTTPAlias"].ToString(),
                    LastModifiedByUserID = (Int32)HttpContext.Current.Session["UserID"]
                };
                context.AddToPortalAlias(r);
                context.SaveChanges();
            }
        }

        #endregion

        /// <summary>
        /// Gets a hashtable of PortAlias data for the provided entity key
        /// </summary>
        /// <param name="EntityKey">The entity key</param>
        /// <returns>Returns hashtable</returns>
        public static Hashtable GetPortalAlias(int EntityKey)
        {
            var values = new Hashtable();

            using (EntitiesContext context = new EntitiesContext())
            {
                var query = from q in context.PortalAlias
                            where q.PortalAliasID == EntityKey
                            select new 
                            {
                                q.PortalAliasID,
                                q.PortalID,
                                q.HTTPAlias
                            };

                foreach (var i in query)
                {
                    values["PortalAliasID"] = i.PortalAliasID;
                    values["PortalID"] = i.PortalID;
                    values["HTTPAlias"] = i.HTTPAlias;
                }
            }
            return values;
        }
    }

}
