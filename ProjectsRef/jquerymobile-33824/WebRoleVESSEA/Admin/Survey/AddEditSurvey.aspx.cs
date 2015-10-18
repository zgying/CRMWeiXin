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

namespace Web.Admin.Survey
{
    public partial class AddEditSurvey : System.Web.UI.Page
    {

        string RadGridKey = "EntityKey";
        int EntityKey;

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = BLL.Survey.Items.DataSource();
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            EntityKey = int.Parse(item.GetDataKeyValue(RadGridKey).ToString());
            Hashtable values = new Hashtable();
            item.ExtractValues(values);
            BLL.Survey.Items.Update(EntityKey, values);
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            Hashtable values = new Hashtable();
            item.ExtractValues(values);
            BLL.Survey.Items.Insert(values);
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            EntityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            BLL.Survey.Items.Delete(EntityKey);
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {
                GridEditFormItem editItem = (GridEditFormItem)e.Item;
                //RadTextBox txtbx = (RadTextBox)editItem["Name"].Controls[0];
                //txtbx.Width = Unit.Pixel(500);
            }
        }

    }
}