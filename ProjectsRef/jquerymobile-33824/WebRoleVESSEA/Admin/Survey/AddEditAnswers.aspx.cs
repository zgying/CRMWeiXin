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
    public partial class AddEditAnswers : System.Web.UI.Page
    {

        string RadGridKey = "EntityKey";
        int EntityKey;

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = BLL.Survey.Answers.DataSource();
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var items = e.Item as GridEditableItem;
            EntityKey = int.Parse(items.GetDataKeyValue(RadGridKey).ToString());
            var values = new Hashtable();
            items.ExtractValues(values);
            GridEditableItem item = e.Item as GridEditableItem;
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            RadComboBox ddAnswerGroupID = (RadComboBox)editform.FindControl("ddAnswerGroupID");
            values["AnswerGroupID"] = ddAnswerGroupID.SelectedValue;
            BLL.Survey.Answers.Update(EntityKey, values);
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            var items = e.Item as GridEditableItem;
            var values = new Hashtable();
            items.ExtractValues(values);
            GridEditableItem item = e.Item as GridEditableItem;
            GridEditFormItem editform = (GridEditFormItem)e.Item;
            RadComboBox ddAnswerGroupID = (RadComboBox)editform.FindControl("ddAnswerGroupID");
            values["AnswerGroupID"] = ddAnswerGroupID.SelectedValue;
            BLL.Survey.Answers.Insert(values);
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            EntityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            BLL.Survey.Answers.Delete(EntityKey);
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {

            var values = new Hashtable();
            GridEditableItem item = e.Item as GridEditableItem;

            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {

                GridEditFormItem editform = (GridEditFormItem)e.Item;
                RadComboBox ddAnswerGroupID = (RadComboBox)editform.FindControl("ddAnswerGroupID");
                RadTextBox Answer = (RadTextBox)editform.FindControl("Answer");

                ddAnswerGroupID.DataSource = BLL.Survey.AnswerGroups.DataSource();
                ddAnswerGroupID.DataTextField = "Name";
                ddAnswerGroupID.DataValueField = "AnswerGroupID";
                ddAnswerGroupID.DataBind();

                //ddAnswerGroupID.Items.Insert(0, new RadComboBoxItem("--Select---", string.Empty));
                //ddAnswerGroupID.SelectedIndex = 0;

                Answer.Width = Unit.Pixel(500);
                ddAnswerGroupID.Width = Unit.Pixel(500);

                if (!e.Item.OwnerTableView.IsItemInserted) //Skip on inserts
                {

                    EntityKey = (int)item.GetDataKeyValue("EntityKey");
                    values = BLL.Survey.AnswerGroups.GetAnswerGroup(EntityKey);

                    if (values["AnswerGroupID"] != null)
                    {
                        ddAnswerGroupID.SelectedValue = values["AnswerGroupID"].ToString();
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
                BLL.Survey.Answers.MoveRecord(int.Parse(CommandArgument.ToString()), true);
            }

            if (cmd == "Down")
            {
                BLL.Survey.Answers.MoveRecord(int.Parse(CommandArgument.ToString()), false);
            }

            RadGrid1.DataSource = BLL.Survey.Answers.DataSource();
            RadGrid1.Rebind();
        }
    }
}