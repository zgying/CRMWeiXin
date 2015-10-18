<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddEditUsers.aspx.cs" Inherits="Web.Admin.AddEditUsers" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="clrFilters">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                        <telerik:AjaxUpdatedControl ControlID="clrFilters" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource"
            OnDeleteCommand="RadGrid1_DeleteCommand" OnInsertCommand="RadGrid1_InsertCommand"
            OnUpdateCommand="RadGrid1_UpdateCommand" OnColumnCreating="RadGrid1_ColumnCreating"
            OnItemCommand="RadGrid1_ItemCommand" AllowPaging="True" AllowFilteringByColumn="True"
            AllowSorting="True" CellSpacing="0" GridLines="None" ShowStatusBar="true" PagerStyle-Position="TopAndBottom">
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" DataKeyNames="EntityKey"
                InsertItemPageIndexAction="ShowItemOnCurrentPage">
                <EditFormSettings ColumnNumber="2" InsertCaption="Create Record">
                    <FormTableItemStyle Wrap="False"></FormTableItemStyle>
                    <FormCaptionStyle CssClass="EditFormHeader"></FormCaptionStyle>
                    <FormMainTableStyle GridLines="None" CellSpacing="0" CellPadding="3" BackColor="White"
                        Width="100%" />
                    <FormTableStyle CellSpacing="0" CellPadding="2" Height="5px" BackColor="White" />
                    <FormTableAlternatingItemStyle Wrap="False"></FormTableAlternatingItemStyle>
                    <EditColumn ButtonType="PushButton" InsertText="Insert" UpdateText="Update" UniqueName="EditCommandColumn1"
                        CancelText="Cancel">
                    </EditColumn>
                    <FormTableButtonRowStyle HorizontalAlign="Left" CssClass="EditFormButtonRow"></FormTableButtonRowStyle>
                </EditFormSettings>
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="RowDblClick" />
            </ClientSettings>
        </telerik:RadGrid>
    </div>
    <div>
        <asp:Button ID="clrFilters" runat="server" Text="Clear filters" CssClass="button"
            OnClick="clrFilters_Click"></asp:Button>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="True" PersistenceKey="QsfSkin"
            PersistenceMode="Session">
        </telerik:RadSkinManager>
    </div>
</asp:Content>