using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using Telerik.Web.UI;
using System.Collections;

namespace WebRoleVESSEA.Admin.Store
{
    public partial class AddEditCategories : System.Web.UI.Page
    {

        const string RadGridKey = "EntityKey";
        int EntityKey;

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            
            RadGrid1.DataSource = BLL.Store.Category.DataSource();
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            EntityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            BLL.Store.Category.Delete(EntityKey);
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            InsertUpdateRecord(false, sender, e);
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            InsertUpdateRecord(true, sender, e);
        }

        protected void InsertUpdateRecord(bool Update, object sender, GridCommandEventArgs e)
        {
            GridEditableItem items = e.Item as GridEditableItem;
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            Hashtable values = new Hashtable();
            items.ExtractValues(values);
            RadComboBox ddParentCategory = (RadComboBox)editform.FindControl("ddParentCategory");
            CheckBox chkDeleted = (CheckBox)editform.FindControl("chkDeleted");
            CheckBox chkIsMenu = (CheckBox)editform.FindControl("chkIsMenu");
            values["ParentCategoryId"] = ddParentCategory.SelectedValue;
            values["Deleted"] = chkDeleted.Checked;
            values["IsMenu"] = chkIsMenu.Checked;

            if (Update == false)
            {
                EntityKey = int.Parse(items.GetDataKeyValue(RadGridKey).ToString());
                BLL.Store.Category.Update(EntityKey, values);
            }
            else
            {
                BLL.Store.Category.Insert(values);
            }

        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {

            var values = new Hashtable();
            GridEditableItem item = e.Item as GridEditableItem;

            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {

                GridEditFormItem editform = (GridEditFormItem)e.Item;
                RadComboBox ddParentCategory = (RadComboBox)editform.FindControl("ddParentCategory");
                CheckBox chkDeleted = (CheckBox)editform.FindControl("chkDeleted");
                CheckBox chkIsMenu = (CheckBox)editform.FindControl("chkIsMenu");
                ddParentCategory.DataSource = BLL.Store.Category.DataSource();
                ddParentCategory.DataTextField = "Name";
                ddParentCategory.DataValueField = "CategoryID";
                ddParentCategory.DataBind();
                ddParentCategory.Items.Insert(0, new RadComboBoxItem("--Select---", string.Empty));
                ddParentCategory.SelectedIndex = 0;

                if (!e.Item.OwnerTableView.IsItemInserted) //Skip on inserts
                {
                    EntityKey = (int)item.GetDataKeyValue("EntityKey");
                    values = BLL.Store.Category.GetStoreCategory(EntityKey);
                    if (values["ParentCategoryId"] != null)
                    {
                        ddParentCategory.SelectedValue = values["ParentCategoryId"].ToString();
                    }
                    if (values["IsMenu"] != null)
                    {
                        chkIsMenu.Checked = bool.Parse(values["IsMenu"].ToString());
                    }
                    if (values["Deleted"] != null)
                    {
                        chkDeleted.Checked = bool.Parse(values["Deleted"].ToString());
                    }   
                }

            }

        }

        protected void MoveItem(object sender, EventArgs e)
        {
            string cmd = ((RadButton)sender).CommandName;
            string CommandArgument = ((RadButton)sender).CommandArgument;

            if (cmd == "Up")
            {
                BLL.Store.Category.MoveRecord(int.Parse(CommandArgument.ToString()), true);
            }

            if (cmd == "Down")
            {
                BLL.Store.Category.MoveRecord(int.Parse(CommandArgument.ToString()), false);
            }

            RadGrid1.DataSource = BLL.Store.Category.DataSource();
            RadGrid1.Rebind();
        }

    }

}