using System;

namespace Web
{
    public partial class SiteLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ACTION = null;
            if (Request.Form["ACTION"] != null)
            {
                ACTION = Request.Form["ACTION"];
                if (ACTION == "SetClientSessionValues")
                {
                    BLL.SiteLog.InsertLog(int.Parse(Request.Form["ClientHeight"].ToString()), int.Parse(Request.Form["ClientWidth"].ToString()), Request.Form["FileName"], bool.Parse(Request.Form["Ajax"].ToString()));
                }
            }
        }
    }
}