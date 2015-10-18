using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Web.Admin
{
    public partial class RadEditor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadEditor1.ImageManager.ContentProviderTypeName = typeof(BLL.ContentFileDB).AssemblyQualifiedName;
            RadEditor1.ImageManager.ViewPaths = new string[] { "ROOT" };
            RadEditor1.ImageManager.UploadPaths = new string[] { "ROOT" };
            RadEditor1.ImageManager.DeletePaths = new string[] { "ROOT" };
        }
    }
}