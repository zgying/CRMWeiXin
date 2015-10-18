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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections;
using DAL;
using BLL;

namespace Web.Admin
{
    public partial class AddEditPortalAlias : System.Web.UI.Page
    {
        private EntitiesContext context;
        private Portals portals;
        string RadGridKey = "EntityKey";
        int EntityKey;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.context = new EntitiesContext();
            this.portals = new Portals(context);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((bool)Session["IsSuperUser"] == true)
            {
                DivSuperUser.Visible = true;
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = BLL.PortalAlias.DataSource();
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            EntityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            BLL.PortalAlias.Delete(EntityKey);
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var items = e.Item as GridEditableItem;
            EntityKey = int.Parse(items.GetDataKeyValue(RadGridKey).ToString());
            var values = new Hashtable();
            items.ExtractValues(values);
            GridEditableItem item = e.Item as GridEditableItem;
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            RadComboBox ddPortalName = (RadComboBox)editform.FindControl("ddPortalName");
            values["PortalID"] = ddPortalName.SelectedValue;
            BLL.PortalAlias.Update(EntityKey, values);
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            var items = e.Item as GridEditableItem;
            var values = new Hashtable();
            items.ExtractValues(values);
            GridEditableItem item = e.Item as GridEditableItem;
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            RadComboBox ddPortalName = (RadComboBox)editform.FindControl("ddPortalName");
            values["PortalID"] = ddPortalName.SelectedValue;
            BLL.PortalAlias.Insert(values);
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

            var values = new Hashtable();
            GridEditableItem item = e.Item as GridEditableItem;

            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {

                GridEditFormItem editform = (GridEditFormItem)e.Item;
                RadComboBox ddPortalName = (RadComboBox)editform.FindControl("ddPortalName");
                RadTextBox HTTPAlias = (RadTextBox)editform.FindControl("HTTPAlias");
                ddPortalName.DataSource = this.portals.DataSource();
                ddPortalName.DataTextField = "Name";
                ddPortalName.DataValueField = "PortalID";
                ddPortalName.DataBind();
                HTTPAlias.Width = Unit.Pixel(500);
                ddPortalName.Width = Unit.Pixel(500);

                if (!e.Item.OwnerTableView.IsItemInserted) //Skip on inserts
                {

                    EntityKey = (int)item.GetDataKeyValue("EntityKey");
                    values = BLL.PortalAlias.GetPortalAlias(EntityKey);

                    if (values["PortalID"] != null)
                    {
                        ddPortalName.SelectedValue = values["PortalID"].ToString();
                    }

                }

            }

        }

    }

}