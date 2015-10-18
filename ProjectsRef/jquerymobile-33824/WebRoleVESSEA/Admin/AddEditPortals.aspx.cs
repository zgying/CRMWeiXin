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
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using BLL;
using Telerik.Web.UI;

namespace Web.Admin
{
    public partial class AddEditPortals : System.Web.UI.Page
    {
        private Portals portals;
        private string RadGridKey { get { return "EntityKey"; } }
        private int entityKey { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current user is a super user.
        /// The default value is false.
        /// </summary>
        private bool IsSuperUser
        {
            get
            {
                object temp = Session["IsSuperUser"];
                return temp == null ? false : (bool)temp;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            this.portals = new Portals(Utils.DbContext);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsSuperUser)
            {
                DivSuperUser.Visible = true;
            }
        }

        public void RadAsyncUpload_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            e.File.SaveAs(BLL.Theme.Path + e.File.FileName);    // Save the file
            BLL.Theme.ExtractZippedData(e.File.FileName);       // Extract the contents
            BLL.Theme.DeleteZippedFile(e.File.FileName);        // Delete the zipped file

            // Refresh the Theme combo box
            GridEditFormItem gridItem = (sender as RadAsyncUpload).NamingContainer as GridEditFormItem;
            RadComboBox ddTheme = (RadComboBox)gridItem.FindControl("ddTheme");
            entityKey = (int)gridItem.GetDataKeyValue(this.RadGridKey);

            SetddThemeItems(ddTheme);
            SetddThemeSelectedValue(ddTheme);
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = portals.DataSource();
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            InsertUpdateRecord(false, sender, e);
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            InsertUpdateRecord(true, sender, e);
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            entityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            portals.Delete(entityKey);
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            GridEditableItem item = e.Item as GridEditableItem;

            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {
                Hashtable values = portals.GetPortal(entityKey);

                GridEditFormItem editform = (GridEditFormItem)e.Item;
                CheckBox chkPayPalEnvironment = (CheckBox)editform.FindControl("chkPayPalEnvironment");
                CheckBox chkEmailSSL = (CheckBox)editform.FindControl("chkEmailSSL");
                RadComboBox ddTheme = (RadComboBox)editform.FindControl("ddTheme");

                // Populate the them combo box
                SetddThemeItems(ddTheme);
                Session["GridSender"] = sender;
                Session["GridEvents"] = e;

                if (!e.Item.OwnerTableView.IsItemInserted) //Skip on inserts
                {
                    EntityKey temp = item.GetDataKeyValue(this.RadGridKey) as EntityKey;
                    entityKey = (int)temp.EntityKeyValues.First(k => k.Key == "PortalID").Value;                  

                    if (values["PayPalEnvironment"] != null)
                    {
                        chkPayPalEnvironment.Checked = bool.Parse(values["PayPalEnvironment"].ToString());
                    }

                    if (values["EmailSSL"] != null)
                    {
                        chkEmailSSL.Checked = bool.Parse(values["EmailSSL"].ToString());
                    }

                    // Set the selected item for the theme combo box
                    SetddThemeSelectedValue(ddTheme);
                }
            }
        }

        protected void InsertUpdateRecord(bool Update, object sender, GridCommandEventArgs e)
        {
            GridEditableItem items = e.Item as GridEditableItem;
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            Hashtable values = new Hashtable();
            CheckBox chkPayPalEnvironment = (CheckBox)editform.FindControl("chkPayPalEnvironment");
            CheckBox chkEmailSSL = (CheckBox)editform.FindControl("chkEmailSSL");
            RadComboBox ddTheme = (RadComboBox)editform.FindControl("ddTheme");
            
            items.ExtractValues(values);            
            values["PayPalEnvironment"] = chkPayPalEnvironment.Checked;
            values["EmailSSL"] = chkEmailSSL.Checked;
            values["Theme"] = ddTheme.SelectedValue;

            if (!Update)
            {
                EntityKey temp = items.GetDataKeyValue(this.RadGridKey) as EntityKey;
                entityKey = (int)temp.EntityKeyValues.First(k => k.Key == "PortalID").Value;
                portals.Update(entityKey, values);
            }
            else
            {
                portals.Insert(values);
            }
        }

        private void SetddThemeItems(RadComboBox ddTheme)
        {
            DirectoryInfo di = new DirectoryInfo(BLL.Theme.Path);
            IEnumerable rgFiles = di.GetFiles("*.min.css", SearchOption.AllDirectories).OrderByDescending(f => f.Name);

            // Clear existing items
            ddTheme.Items.Clear();

            foreach (FileInfo fi in rgFiles)
            {
                RadComboBoxItem item = new RadComboBoxItem(fi.Name.Replace(".min", ""), fi.Name.Replace(".min", ""));

                // Only add an item if it is not already in the list
                if (!ddTheme.Items.Contains(item))
                {
                    ddTheme.Items.Add(item); 
                }
            }
        }

        private void SetddThemeSelectedValue(RadComboBox ddTheme)
        {
            if (this.entityKey > 0)
            {
                string theme = portals.GetTheme(this.entityKey);

                if (ddTheme.Items.Any(i => string.Equals(theme, i.Value, StringComparison.OrdinalIgnoreCase)))
                {
                    ddTheme.SelectedValue = theme;
                }
                else
                {
                    ddTheme.SelectedIndex = -1;
                }
            }
        }
    }
}



