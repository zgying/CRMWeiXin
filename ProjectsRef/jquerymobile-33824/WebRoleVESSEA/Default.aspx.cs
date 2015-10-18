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
using BLL;
using DAL;

namespace Web
{

    public partial class _Default : System.Web.UI.Page
    {
        string StartMethod;
        private EntitiesContext context;
        private Portals portals;

        void Page_PreInit(object sender, EventArgs e)
        {
            context = new EntitiesContext();
            portals = new Portals(context);
            //////Set PortalID override
            //Session["PortalID"] = 2;
            //PROJECT=BLL&NAMESPACE=BLL.Store&CLASS=Category&METHOD=StoreCategoryList
            //PROJECT=BLL&NAMESPACE=BLL&CLASS=Content&METHOD=ReadContent&PARAMS=Home.aspx,IsAjax

            //////Auto login
            if (Session["UserID"] == null)
            {
                BLL.Users.Login("admin@localhost.com", "admin@localhost.com");
            }

            var values = new Hashtable();
            values = portals.GetSiteConfig();

            if (values["MasterPage"] != null)
            {
                MasterPageFile = "~/App_Master/" + values["MasterPage"].ToString();
            }

            if (values["StartMethod"] != null)
            {
                StartMethod = values["StartMethod"].ToString();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LiteralStartScript.Text = (@"<script type=""text/javascript"">" + @"makePOSTRequest(""section_body"", """ + StartMethod + @""");" + @"</script>");
        }

    }

}
