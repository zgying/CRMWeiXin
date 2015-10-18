using System;
using System.Linq;
using Telerik.Web.UI;
using System.Collections;
using DAL;

namespace Web.Admin
{
    public partial class AddEditRoles : System.Web.UI.Page
    {
        private BLL.Roles roles;
        private EntitiesContext context;
        string RadGridKey = "RoleID";
        int EntityKey;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.context = new EntitiesContext();
            this.roles = new BLL.Roles(context);
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = roles.DataSource();
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            EntityKey = int.Parse(item.GetDataKeyValue(RadGridKey).ToString());
            Hashtable values = new Hashtable();
            item.ExtractValues(values);
            roles.Update(EntityKey, values);
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            Hashtable values = new Hashtable();
            item.ExtractValues(values);
            roles.Insert(values);
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            EntityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            roles.Delete(EntityKey);
        }
    }
}