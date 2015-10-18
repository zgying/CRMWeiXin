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
 * URLS
 * http://demos.telerik.com/aspnet-ajax/grid/examples/dataediting/validation/defaultcs.aspx
 * http://www.telerik.com/community/forums/aspnet/editor/radeditor-imagemanager-upload-button-is-not-working.aspx
 * http://www.telerik.com/community/forums/aspnet-ajax/editor/radeditor-in-radgrid-value-not-updating.aspx
 * http://www.telerik.com/support/kb/aspnet-ajax/grid/using-radeditor-as-editor-in-template-column-of-radgrid.aspx
 * http://demos.telerik.com/aspnet-ajax/grid/examples/programming/needdatasource/defaultcs.aspx
 * 
 */

using System;
using System.Data;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Web.Admin
{
    public partial class AddEditPages : System.Web.UI.Page
    {
        const string RadGridKey = "EntityKey";
        int EntityKey;

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = BLL.Content.DataSource();
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            EntityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            BLL.Content.Delete(EntityKey);
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
            RadComboBox ddContentType = (RadComboBox)editform.FindControl("ddContentType");
            CheckBox chkPublished = (CheckBox)editform.FindControl("chkPublished");
            CheckBox chkIsMenu = (CheckBox)editform.FindControl("chkIsMenu");
            values["ContentTypeID"] = ddContentType.SelectedValue;
            values["ContentText"] = RadEditor1.Content;
            values["Published"] = chkPublished.Checked;
            values["IsMenu"] = chkIsMenu.Checked;

            if (Update == false)
            {
                EntityKey = int.Parse(items.GetDataKeyValue(RadGridKey).ToString());
                BLL.Content.Update(EntityKey, values);
            }
            else
            {
                BLL.Content.Create(values);
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
                RadComboBox ddContentType = (RadComboBox)editform.FindControl("ddContentType");
                CheckBox chkPublished = (CheckBox)editform.FindControl("chkPublished");
                CheckBox chkIsMenu = (CheckBox)editform.FindControl("chkIsMenu");

                ddContentType.DataSource = BLL.ContentType.DataSource();
                ddContentType.DataTextField = "Name";
                ddContentType.DataValueField = "ContentTypeID";
                ddContentType.DataBind();

                //Test Ajax is having an issue with images in DB test to see if file images have same issue
                RadEditor1.ImageManager.ContentProviderTypeName = typeof(BLL.ContentFileDB).AssemblyQualifiedName;
                RadEditor1.ImageManager.ViewPaths = new string[] { "ROOT" };
                RadEditor1.ImageManager.UploadPaths = new string[] { "ROOT" };
                RadEditor1.ImageManager.DeletePaths = new string[] { "ROOT" };

                //string[] imagepath = { "~/Images/Portals/1" };
                //RadEditor1.ImageManager.UploadPaths = imagepath;
                //RadEditor1.ImageManager.ViewPaths = imagepath;
                //RadEditor1.ImageManager.DeletePaths = imagepath;

                if (!e.Item.OwnerTableView.IsItemInserted) //Skip on inserts
                {

                    EntityKey = (int)item.GetDataKeyValue("EntityKey");
                    values = BLL.Content.Read(EntityKey);

                    if (values["ContentTypeID"] != null)
                    {
                        ddContentType.SelectedValue = values["ContentTypeID"].ToString();
                    }

                    if (values["ContentText"] != null)
                    {
                        RadEditor1.Content = values["ContentText"].ToString();
                    }

                    if (values["IsPublished"] != null)
                    {
                        chkPublished.Checked = bool.Parse(values["IsPublished"].ToString());
                    }

                    if (values["IsMenu"] != null)
                    {
                        chkIsMenu.Checked = bool.Parse(values["IsMenu"].ToString());
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
                BLL.Content.MoveRecord(int.Parse(CommandArgument.ToString()), true);
            }

            if (cmd == "Down")
            {
                BLL.Content.MoveRecord(int.Parse(CommandArgument.ToString()), false);
            }

            RadGrid1.DataSource = BLL.Content.DataSource();
            RadGrid1.Rebind();
        }


    }

}


