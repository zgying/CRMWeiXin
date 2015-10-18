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
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace BLL
{
    public class CustomFilteringColumn : GridBoundColumn
    {

        //RadGrid will call this method when it initializes the controls inside the filtering item cells
        protected override void SetupFilterControls(TableCell cell)
        {
            base.SetupFilterControls(cell);
            cell.Controls.RemoveAt(0);
            RadComboBox combo = new RadComboBox();
            combo.ID = ("RadComboBox1" + this.UniqueName);
            combo.ShowToggleImage = false;
            combo.EnableLoadOnDemand = true;
            combo.AutoPostBack = true;
            combo.MarkFirstMatch = true;
            combo.Height = Unit.Pixel(100);
            combo.ItemsRequested += this.list_ItemsRequested;
            combo.SelectedIndexChanged += this.list_SelectedIndexChanged;
            cell.Controls.AddAt(0, combo);
            cell.Controls.RemoveAt(1);
        }

        //RadGrid will cal this method when the value should be set to the filtering input control(s)
        protected override void SetCurrentFilterValueToControl(TableCell cell)
        {
            base.SetCurrentFilterValueToControl(cell);
            RadComboBox combo = (RadComboBox)cell.Controls[0];
            if ((this.CurrentFilterValue != string.Empty))
            {
                combo.Text = this.CurrentFilterValue;
            }
        }

        //RadGrid will cal this method when the filtering value should be extracted from the filtering input control(s)
        protected override string GetCurrentFilterValueFromControl(TableCell cell)
        {
            RadComboBox combo = (RadComboBox)cell.Controls[0];
            return combo.Text;
        }

        private void list_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            ((RadComboBox)o).DataTextField = this.DataField;
            ((RadComboBox)o).DataValueField = this.DataField;
            ((RadComboBox)o).DataSource = BLL.Users.DataSource();
            ((RadComboBox)o).DataBind();
        }

        private void list_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            GridFilteringItem filterItem = (GridFilteringItem)((RadComboBox)o).NamingContainer;
            if ((this.UniqueName == "Index"))
            {
                //this is filtering for integer column type 
                filterItem.FireCommandEvent("Filter", new Pair("EqualTo", this.UniqueName));
            }
            //filtering for string column type
            filterItem.FireCommandEvent("Filter", new Pair("Contains", this.UniqueName));
        }

    }

}
