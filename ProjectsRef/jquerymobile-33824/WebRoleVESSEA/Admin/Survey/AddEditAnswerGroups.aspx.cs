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
    public partial class AddEditAnswerGroups : System.Web.UI.Page
    {

        string RadGridKey = "EntityKey";
        int EntityKey;

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = BLL.Survey.AnswerGroups.DataSource();
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var items = e.Item as GridEditableItem;
            EntityKey = int.Parse(items.GetDataKeyValue(RadGridKey).ToString());
            var values = new Hashtable();
            items.ExtractValues(values);
            GridEditableItem item = e.Item as GridEditableItem;
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            RadComboBox ddAnswerType = (RadComboBox)editform.FindControl("ddAnswerType");
            values["AnswerTypeID"] = ddAnswerType.SelectedValue;
            BLL.Survey.AnswerGroups.Update(EntityKey, values);
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            var items = e.Item as GridEditableItem;
            var values = new Hashtable();
            items.ExtractValues(values);
            GridEditableItem item = e.Item as GridEditableItem;
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            RadComboBox ddAnswerType = (RadComboBox)editform.FindControl("ddAnswerType");
            values["AnswerTypeID"] = ddAnswerType.SelectedValue;
            BLL.Survey.AnswerGroups.Insert(values);
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            EntityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            BLL.Survey.AnswerGroups.Delete(EntityKey);
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

            var values = new Hashtable();
            GridEditableItem item = e.Item as GridEditableItem;

            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {

                GridEditFormItem editform = (GridEditFormItem)e.Item;
                RadComboBox ddAnswerType = (RadComboBox)editform.FindControl("ddAnswerType");
                RadTextBox Name = (RadTextBox)editform.FindControl("Name");
                ddAnswerType.DataSource = BLL.Survey.AnswerTypes.DataSource();
                ddAnswerType.DataTextField = "Name";
                ddAnswerType.DataValueField = "SurveyAnswerTypeID";
                ddAnswerType.DataBind();
                Name.Width = Unit.Pixel(500);
                ddAnswerType.Width = Unit.Pixel(500);

                if (!e.Item.OwnerTableView.IsItemInserted) //Skip on inserts
                {

                    EntityKey = (int)item.GetDataKeyValue("EntityKey");
                    values = BLL.Survey.AnswerGroups.GetAnswerGroup(EntityKey);

                    if (values["AnswerTypeID"] != null)
                    {
                        ddAnswerType.SelectedValue = values["AnswerTypeID"].ToString();
                    }

                }

            }

        }

    }
}