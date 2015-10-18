using System;
using System.Web.UI;
using System.Data;
using System.Linq;
using Telerik.Web.UI;
using System.Collections;

namespace Web.Admin
{
    public partial class AddEditUsers : System.Web.UI.Page
    {
        const string RadGridKey = "EntityKey";
        int EntityKey;

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = BLL.Users.DataSource();
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var item = e.Item as GridEditableItem;
            EntityKey = int.Parse(item.GetDataKeyValue(RadGridKey).ToString());
            var values = new Hashtable();
            item.ExtractValues(values);
            BLL.Users.Update(EntityKey, values);
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            var item = e.Item as GridEditableItem;
            var values = new Hashtable();
            item.ExtractValues(values);
            BLL.Users.Insert(values);
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            EntityKey = int.Parse((e.Item as GridDataItem).GetDataKeyValue(RadGridKey).ToString());
            BLL.Users.Delete(EntityKey);
        }

        protected void RadGrid1_ColumnCreating(object sender, GridColumnCreatingEventArgs e)
        {
            if ((e.ColumnType == typeof(BLL.CustomFilteringColumn).Name))
            {
                e.Column = new BLL.CustomFilteringColumn();
            }
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if ((e.CommandName == "Filter"))
            {
                foreach (GridColumn column in e.Item.OwnerTableView.Columns)
                {
                    column.CurrentFilterValue = string.Empty;
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                }
            }
        }

        protected void clrFilters_Click(object sender, EventArgs e)
        {
            foreach (GridColumn column in RadGrid1.MasterTableView.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            RadGrid1.MasterTableView.FilterExpression = string.Empty;
            RadGrid1.MasterTableView.Rebind();
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {

                GridEditCommandColumn GEC = new GridEditCommandColumn();
                this.RadGrid1.MasterTableView.Columns.Add(GEC);
                GEC.UniqueName = "EditCommandColumn";
                GEC.ButtonType = GridButtonColumnType.ImageButton;
                GEC.ItemStyle.Width = 20;
                GEC.OrderIndex = 0;

                GridButtonColumn GBC = new GridButtonColumn();
                this.RadGrid1.MasterTableView.Columns.Add(GBC);
                GBC.CommandName = "Delete";
                GBC.Text = "Delete";
                GBC.ItemStyle.Width = 20;
                GBC.ButtonType = GridButtonColumnType.ImageButton;
                
                BLL.CustomFilteringColumn gridColumn2 = new BLL.CustomFilteringColumn();
                this.RadGrid1.MasterTableView.Columns.Add(gridColumn2);
                gridColumn2.DataField = "FirstName";
                gridColumn2.HeaderText = "First Name";
                gridColumn2.ItemStyle.CssClass = "UseHand";

                BLL.CustomFilteringColumn gridColumn3 = new BLL.CustomFilteringColumn();
                this.RadGrid1.MasterTableView.Columns.Add(gridColumn3);
                gridColumn3.DataField = "LastName";
                gridColumn3.HeaderText = "Last Name";
                
                BLL.CustomFilteringColumn gridColumn4 = new BLL.CustomFilteringColumn();
                this.RadGrid1.MasterTableView.Columns.Add(gridColumn4);
                gridColumn4.DataField = "Email";
                gridColumn4.HeaderText = "Email";
                gridColumn4.ItemStyle.Width = 400;
                
                GridDateTimeColumn gridColumn5 = new GridDateTimeColumn();
                this.RadGrid1.MasterTableView.Columns.Add(gridColumn5);
                gridColumn5.DataField = "Birthday";
                gridColumn5.HeaderText = "Birthday";
                gridColumn5.ItemStyle.Width = 200;
                gridColumn5.AllowFiltering = false;
                
                GridCheckBoxColumn gridColumn6 = new GridCheckBoxColumn();
                this.RadGrid1.MasterTableView.Columns.Add(gridColumn6);
                gridColumn6.DataField = "IsLockedOut";
                gridColumn6.HeaderText = "Locked Out";
                gridColumn6.AllowFiltering = false;

                //BLL.CustomFilteringColumn gridColumn7 = new BLL.CustomFilteringColumn();
                //this.RadGrid1.MasterTableView.Columns.Add(gridColumn7);
                //gridColumn7.DataField = "Gender";
                //gridColumn7.HeaderText = "Gender";
                //gridColumn7.AllowFiltering = false;

                //RadComboBox gridColumn7 = new RadComboBox();
                //this.RadGrid1.MasterTableView.Columns.Add(gridColumn7);
                
                //gridColumn7.Items[0].Text = "Male";
                //gridColumn7.Items[0].Value = "Male";

                //gridColumn7.Items[1].Text = "Female";
                //gridColumn7.Items[1].Value = "Female";

            }

        }

    }

}