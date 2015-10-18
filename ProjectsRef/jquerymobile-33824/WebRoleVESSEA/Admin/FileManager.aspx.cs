using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin
{
    public partial class FileManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadFileExplorer1.Configuration.ContentProviderTypeName = typeof(BLL.ContentFileDB).AssemblyQualifiedName;
            RadFileExplorer1.Configuration.ViewPaths = new string[] { "ROOT" };
            RadFileExplorer1.Configuration.UploadPaths = new string[] { "ROOT" };
            RadFileExplorer1.Configuration.DeletePaths = new string[] { "ROOT" };
        }
    }
}