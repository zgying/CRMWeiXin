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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;


namespace Web.App_Master
{

    public partial class Mobile : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] file = Request.CurrentExecutionFilePath.Split('/');
            string fileName = file[file.Length - 1];

            DivListView.InnerHtml = BLL.Content.ReadContentListView();

            if (fileName != "Default.aspx" & fileName != "default.aspx")
            {
                ContentPrimary.Visible = false;
                ContentSecondary.Visible = false;
                ContentSingle.Visible = true;
            }
            else
            {
                ContentPrimary.Visible = true;
                ContentSecondary.Visible = true;
                ContentSingle.Visible = false;
            }

        }

    }

}