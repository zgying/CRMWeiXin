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
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using DAL;

namespace BLL
{

    public class Roles
    {
        private EntitiesContext context;

        public Roles(EntitiesContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the value of the Portal ID for the current session
        /// </summary>
        public int PortalID
        {
            get
            {
                object temp = HttpContext.Current.Session["PortalID"];
                return temp == null ? 1 : (int)temp;
            }
        }

        public IQueryable DataSource()
        {
            return from q in this.context.Roles
                   where q.PortalID == this.PortalID
                   select new { q.RoleID, q.RoleName, q.Description };
        }

        public void Delete(int EntityKey)
        {
            DAL.Role r = this.context.Roles.FirstOrDefault(p => p.RoleID == EntityKey);

            if (r != null)
            {
                this.context.DeleteObject(r);
                this.context.SaveChanges();
            }
        }

        public void Update(int EntityKey, Hashtable values)
        {
            DAL.Role r = this.context.Roles.FirstOrDefault(p => p.RoleID == EntityKey);

            if (r != null)
            {
                r.PortalID = this.PortalID;
                r.RoleName = values["RoleName"].ToString();
                r.Description = values["Description"].ToString();
                this.context.SaveChanges();
            }
        }

        public void Insert(Hashtable values)
        {
            DAL.Role r = new DAL.Role
            {
                PortalID = this.PortalID,
                RoleName = values["RoleName"].ToString(),
                Description = values["Description"].ToString(),
                CreateDate = DateTime.Now
            };
            this.context.AddToRoles(r);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Content RadTable functions
        /// </summary>

        public int CreateRole(string roleName, string description = null)
        {
            if (!RoleExists(roleName))
            {
                Role R = new Role
                    {
                        CreateDate = DateTime.Now,
                        RoleName = roleName,
                        Description = description,
                        PortalID = this.PortalID
                    };
                context.Roles.AddObject(R);
                context.SaveChanges();
                return R.RoleID;
            }
            else
            {
                return 0;
            }
        }

        public void DeleteRole(int roleid)
        {
            var R = context.Roles.Single(Rl => Rl.RoleID == roleid & Rl.PortalID == this.PortalID);
            context.Roles.DeleteObject(R);
            context.SaveChanges();
        }

        public bool RoleExists(string roleName)
        {
            try
            {
                return this.context.Roles.Any(Rl => Rl.RoleName == roleName & Rl.PortalID == this.PortalID);                    
            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
                return false;
            }
        }

        public void AddUsersToRoles(int roleid, int userid, int portalID = 0)
        {
            if (portalID == 0)
            {
                portalID = this.PortalID;
            }

            if (!IsUserInRole(roleid, userid))
            {
                UsersInRole UR = new UsersInRole
                {
                    CreateDate = DateTime.Now,
                    UserID = userid,
                    RoleID = roleid,
                    PortalID = portalID
                };
                this.context.UsersInRoles.AddObject(UR);
                this.context.SaveChanges();
            }
        }

        public bool IsUserInRole(int roleid, int userid)
        {
            return context.UsersInRoles
                          .Any(Rl => 
                              Rl.RoleID == roleid 
                              & Rl.UserID == userid 
                              & Rl.PortalID == this.PortalID);
        }

        public string GetRoles(int userid)
        {
            var roles = this.context.usp_GetUserRoles(userid);

            if (roles != null)
            {
                return Crypto.Encrypt(
                    string.Join(",", roles.Select(r => r.RoleName).ToArray()),
                    ConfigurationManager.AppSettings.Get("PASSPHRASESALT"));
            }
            else
            {
                return null;
            }
        }
    }
}

