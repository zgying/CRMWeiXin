using System;
using System.Linq;
using Telerik.Web.UI;
using System.Collections;

namespace Web.Admin
{
    public partial class SiteLog : System.Web.UI.Page
    {

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = BLL.SiteLog.DataSource();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            RadGrid1.MasterTableView.ExportToCSV();
        }
    }
}