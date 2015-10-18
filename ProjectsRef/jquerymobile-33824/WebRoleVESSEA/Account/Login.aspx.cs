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
using System.Web.UI;
using BLL;

namespace Web.Account
{

    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Login";
            BLL.Users.GraphUser();

            if (string.IsNullOrWhiteSpace(BLL.Facebook.GetFacebookAppID()))
            {
                DivBtn2.Visible = false;
            }

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Request.Form["email"].ToString();
                string pass = Request.Form["password"].ToString();
                bool LoggedIn = BLL.Users.Login(email, pass);

                if (LoggedIn == false)
                {
                    lblErrorMessage.Text = "* Sorry wrong user name or password please try again.";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }

        protected void btnFBLogin_Click(object sender, EventArgs e)
        {
            Facebook oFB = new Facebook();
            Response.Redirect(oFB.AuthorizationLinkGet());
        }

    }

}