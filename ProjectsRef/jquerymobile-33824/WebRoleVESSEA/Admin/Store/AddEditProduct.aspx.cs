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
    public partial class AddEditProduct : System.Web.UI.Page
    {

        const string RadGridKey = "EntityKey";
        int EntityKey;

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = BLL.Store.Product.DataSource();
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            EntityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            BLL.Store.Product.Delete(EntityKey);
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
            Telerik.Web.UI.RadEditor RadEditor1 = (Telerik.Web.UI.RadEditor)editform.FindControl("RadEditor1");
            RadComboBox ddCategory = (RadComboBox)editform.FindControl("ddCategory");
            CheckBox chkDeleted = (CheckBox)editform.FindControl("chkDeleted");
            values["CategoryID"] = ddCategory.SelectedValue;
            values["Description"] = RadEditor1.Content;
            values["Deleted"] = chkDeleted.Checked;

            if (Update == false)
            {
                EntityKey = int.Parse(items.GetDataKeyValue(RadGridKey).ToString());
                BLL.Store.Product.Update(EntityKey, values);
            }
            else
            {
                BLL.Store.Product.Insert(values);
            }

        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {

            var values = new Hashtable();
            GridEditableItem item = e.Item as GridEditableItem;

            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {

                GridEditFormItem editform = (GridEditFormItem)e.Item;
                Telerik.Web.UI.RadEditor RadEditor1 = (Telerik.Web.UI.RadEditor)editform.FindControl("RadEditor1");
                RadComboBox ddCategory = (RadComboBox)editform.FindControl("ddCategory");
                CheckBox chkDeleted = (CheckBox)editform.FindControl("chkDeleted");
                ddCategory.DataSource = BLL.Store.Category.DataSource();
                ddCategory.DataTextField = "Name";
                ddCategory.DataValueField = "CategoryID";
                ddCategory.DataBind();

                RadEditor1.ImageManager.ContentProviderTypeName = typeof(BLL.ContentFileDB).AssemblyQualifiedName;
                RadEditor1.ImageManager.ViewPaths = new string[] { "ROOT" };
                RadEditor1.ImageManager.UploadPaths = new string[] { "ROOT" };
                RadEditor1.ImageManager.DeletePaths = new string[] { "ROOT" };

                if (!e.Item.OwnerTableView.IsItemInserted) //Skip on inserts
                {
                    EntityKey = (int)item.GetDataKeyValue("EntityKey");
                    values = BLL.Store.Product.GetProduct(EntityKey);
                    
                    if (values["CategoryID"] != null)
                    {
                        ddCategory.SelectedValue = values["CategoryID"].ToString();
                    }

                    if (values["Description"] != null)
                    {
                        RadEditor1.Content = values["Description"].ToString();
                    }

                    chkDeleted.Checked = bool.Parse(values["Deleted"].ToString());

                }

            }

        }

    }

}