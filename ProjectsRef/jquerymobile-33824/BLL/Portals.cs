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
    public class Portals
    {
        private int _portalID;
        private EntitiesContext context;

        public Portals(EntitiesContext context)
        {
            this.context = context;
        }

        #region CRUD OPERATIONS

        /// <summary>
        /// Fetches an anonymous list of Portal entities
        /// </summary>
        /// <returns>Returns IQueryable</returns>
        public IQueryable DataSource()
        {
            var query = from portal in this.context.Portals
                        select portal;
            return query;
        }

        /// <summary>
        /// Deletes a portal entity
        /// </summary>
        /// <param name="EntityKey">The portal's entity key</param>
        public void Delete(int EntityKey)
        {
            DAL.Portal r = this.context.Portals.Single(p => p.PortalID == EntityKey);
            this.context.DeleteObject(r);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Updates a portals entity with new values
        /// </summary>
        /// <param name="EntityKey">The portal's entity key</param>
        /// <param name="values">A hashtable of values used to update the portal entity</param>
        public void Update(int EntityKey, Hashtable values)
        {
            float taxRate = 0;

            if (!string.IsNullOrWhiteSpace(values["TaxRate"].ToString()))
            {
                taxRate = float.Parse(values["TaxRate"].ToString());
            }

            DAL.Portal r = this.context.Portals.Single(p => p.PortalID == EntityKey);
            r.Name = values["Name"].ToString();
            r.MasterPage = values["MasterPage"].ToString();
            r.Theme = values["Theme"].ToString();
            r.UrlRedirect = values["UrlRedirect"].ToString();
            r.FacebookAppID = values["FacebookAppID"].ToString();
            r.FacebookSecret = values["FacebookSecret"].ToString();
            r.PayPalEnvironment = bool.Parse(values["PayPalEnvironment"].ToString());
            r.PayPalBusiness = values["PayPalBusiness"].ToString();
            r.EmailHost = values["EmailHost"].ToString();
            r.EmailPort = values["EmailPort"].ToString();
            r.EmailUser = values["EmailUser"].ToString();
            r.EmailPass = values["EmailPass"].ToString();
            r.EmailSSL = bool.Parse(values["EmailSSL"].ToString());
            r.RobotsText = values["RobotsText"].ToString();
            r.GoogleAnalytics = values["GoogleAnalytics"].ToString();
            r.LastUpdated = DateTime.Now;
            r.StartMethod = values["StartMethod"].ToString();
            r.TaxRate = taxRate;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Creates a new portal and inserts it into the database
        /// </summary>
        /// <param name="values">A hashtable of values used to create the Portals entity</param>
        public void Insert(Hashtable values)
        {
            float taxRate = 0;

            if (!string.IsNullOrWhiteSpace(values["TaxRate"].ToString()))
            {
                taxRate = float.Parse(values["TaxRate"].ToString());
            }

            DAL.Portal r = new DAL.Portal
            {
                Name = values["Name"].ToString(),
                Theme = values["Theme"].ToString(),
                MasterPage = values["MasterPage"].ToString(),
                UrlRedirect = values["UrlRedirect"].ToString(),
                FacebookAppID = values["FacebookAppID"].ToString(),
                FacebookSecret = values["FacebookSecret"].ToString(),
                PayPalEnvironment = bool.Parse(values["PayPalEnvironment"].ToString()),
                PayPalBusiness = values["PayPalBusiness"].ToString(),
                EmailHost = values["EmailHost"].ToString(),
                EmailPort = values["EmailPort"].ToString(),
                EmailUser = values["EmailUser"].ToString(),
                EmailPass = values["EmailPass"].ToString(),
                EmailSSL = bool.Parse(values["EmailSSL"].ToString()),
                RobotsText = values["RobotsText"].ToString(),
                GoogleAnalytics = values["GoogleAnalytics"].ToString(),
                CreateDate = DateTime.Now,
                CreatedByUserID = (int)HttpContext.Current.Session["UserID"],
                StartMethod = values["StartMethod"].ToString(),
                TaxRate = taxRate
            };
            this.context.AddToPortals(r);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Fetches the portal values by the portal's entity key
        /// </summary>
        /// <param name="EntityKey">The entity key value</param>
        /// <returns>Returns a hashtable with the portal values</returns>
        public Hashtable GetPortal(int EntityKey)
        {
            var values = new Hashtable();

            try
            {
                var portal = context.Portals.Single(q => q.PortalID == EntityKey);

                if (portal != null)
                {
                    values["PortalID"] = portal.PortalID;
                    values["Name"] = portal.Name;
                    values["MasterPage"] = portal.MasterPage;
                    values["Theme"] = portal.Theme;
                    values["UrlRedirect"] = portal.UrlRedirect;
                    values["FacebookAppID"] = portal.FacebookAppID;
                    values["FacebookSecret"] = portal.FacebookSecret;
                    values["PayPalEnvironment"] = portal.PayPalEnvironment;
                    values["PayPalBusiness"] = portal.PayPalBusiness;
                    values["EmailHost"] = portal.EmailHost;
                    values["EmailUser"] = portal.EmailUser;
                    values["EmailPass"] = portal.EmailPass;
                    values["EmailPassSalt"] = portal.EmailPassSalt;
                    values["EmailSSL"] = portal.EmailSSL;
                    values["RobotsText"] = portal.RobotsText;
                    values["GoogleAnalytics"] = portal.GoogleAnalytics;
                    values["UrlRedirect"] = portal.UrlRedirect;
                    values["StartMethod"] = portal.StartMethod;
                    values["TaxRate"] = portal.TaxRate;
                }
            }
            catch
            {
                ElmahExtension.LogToElmah(new ApplicationException("No portal found setting default settings"));
                values["PortalID"] = 0;
                values["Name"] = "Default";
                values["MasterPage"] = "Mobile.Master";
                values["Theme"] = "BlackAndWhite.css";
            }
            return values;
        }

        /// <summary>
        /// Gets the current theme for the chosen portal
        /// </summary>
        /// <param name="EntityKey">The portal's entity key value</param>
        /// <returns>Returns a string</returns>
        public string GetTheme(int EntityKey)
        {
            try
            {
                return this.context.Portals.Single(portal => portal.PortalID == EntityKey).Theme;
            }
            catch
            {
                ElmahExtension.LogToElmah(new ApplicationException("No portal found setting default settings"));
                return string.Empty;
            }
        }

        #endregion

        /// <summary>
        /// Fetches the values for the site's MasterPage and StartMethod
        /// </summary>
        /// <returns>Returns a hashtable with keys: "MasterPage", "StartMethod"</returns>
        public Hashtable GetSiteConfig()
        {
            var values = new Hashtable();

            try
            {
                var query = from _Portals in this.context.Portals
                            where _Portals.PortalID == this.PortalID
                            select new
                            {
                                _Portals.MasterPage,
                                _Portals.StartMethod
                            };

                foreach (var c in query)
                {
                    values["MasterPage"] = c.MasterPage;
                    values["StartMethod"] = c.StartMethod;
                }
            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
            }

            return values;
        }

        /// <summary>
        /// Fetches the saved google analytics data for the current portal
        /// </summary>
        /// <returns>Returns a string</returns>
        public string GetGoogleAnalytics()
        {
            return context.Portals.Single(p => p.PortalID == this.PortalID).GoogleAnalytics;
        }

        /// <summary>
        /// Gets or sets the value of the portal id
        /// </summary>
        public int PortalID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("PortalID")))
                {
                    try
                    {
                        bool HostChanged = false;
                        var http = HttpContext.Current;

                        if (http.Session["PortalHost"] != null && http.Session["PortalHost"].ToString() != http.Request.Url.Host)
                        {
                            HostChanged = true;
                        }                        

                        if (http.Session["PortalID"] == null | HostChanged == true)
                        {
                             var P = this.context.PortalAlias.Single(p => p.HTTPAlias == http.Request.Url.Host);

                             if (P != null)
                             {
                                 http.Session["PortalID"] = P.PortalID;
                                 http.Session["PortalHost"] = http.Request.Url.Host;
                                 _portalID = P.PortalID;
                             }
                        }
                        else
                        {
                            _portalID = (int)http.Session["PortalID"];
                        }
                    }
                    catch (Exception ex)
                    {
                        _portalID = 1;
                        ElmahExtension.LogToElmah(ex);
                    }
                }
                else
                {
                    _portalID = int.Parse(ConfigurationManager.AppSettings.Get("PortalID"));
                }

                return _portalID;
            }

            set { _portalID = value; HttpContext.Current.Session["PortalID"] = value; }
        }
    }
}
