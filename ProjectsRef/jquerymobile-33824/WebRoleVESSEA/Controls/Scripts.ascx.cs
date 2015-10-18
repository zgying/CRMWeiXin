using System;
using System.Collections;
using System.Text;
using BLL;

namespace WebRoleVESSEA.Controls
{
    public partial class Scripts : System.Web.UI.UserControl
    {
        private Portals portals;

        protected void Page_Init(object sender, EventArgs e)
        {
            // Initializes the Portals object using the EntitiesContext for the current request
            this.portals = new Portals(Utils.DbContext);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var values = new Hashtable();
            values = portals.GetPortal(portals.PortalID);

            sb.AppendLine(@"<link rel=""stylesheet"" href=""/App_Themes/Mobile/jquery.mobile-1.1.0.min.css"" />");
            sb.AppendLine(@"<link rel=""stylesheet"" href=""/App_Themes/Mobile/demos/docs/_assets/css/jqm-docs.css"" />");
            
            if (values["Theme"] != null)
            {
                sb.AppendLine(@"<link rel=""stylesheet"" href=""/App_Themes/ThemeRoller/" + values["Theme"].ToString().Replace(".css", "") + @"/themes/" + values["Theme"] + @""" />");
            }
            else
            {
                sb.AppendLine(@"<link rel=""stylesheet"" href=""/App_Themes/ThemeRoller/BlackAndWhite/themes/BlackAndWhite.css"" />");
            }

            //sb.AppendLine(@"<link rel=""stylesheet"" href=""/App_Themes/Mobile/Custom.css"" />");

            sb.AppendLine(@"<script type=""text/javascript"" src=""/App_Themes/Mobile/demos/js/jquery.js""></script>");
            sb.AppendLine(@"<script type=""text/javascript"" src=""/App_Themes/Mobile/jquery.mobile-1.1.0.min.js""></script>");
            sb.AppendLine(@"<script type=""text/javascript"" src=""/Scripts/main.js""></script>");
            sb.AppendLine(@"<script type=""text/javascript"" src=""/App_Themes/Mobile/demos/docs/_assets/js/jqm-docs.js""></script>");
            sb.AppendLine(@"<script type=""text/javascript"" src=""/Scripts/jquery.validate.min.js""></script>");
            
            if (values["GoogleAnalytics"] != null)
            {
                sb.AppendLine(values["GoogleAnalytics"].ToString());
            }

            LiteralScripts.Text = sb.ToString();
        }
    }
}