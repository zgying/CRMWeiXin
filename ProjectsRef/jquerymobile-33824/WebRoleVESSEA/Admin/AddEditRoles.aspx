<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddEditRoles.aspx.cs" Inherits="Web.Admin.AddEditRoles" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource"
            OnDeleteCommand="RadGrid1_DeleteCommand" OnInsertCommand="RadGrid1_InsertCommand"
            OnUpdateCommand="RadGrid1_UpdateCommand">
            <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" DataKeyNames="RoleID"
                InsertItemPageIndexAction="ShowItemOnCurrentPage">
                <Columns>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                        ItemStyle-Width="20px">
                        <ItemStyle CssClass="MyImageButton" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ConfirmText="Delete record?" ConfirmDialogType="RadWindow"
                        ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete"
                        UniqueName="DeleteColumn" ItemStyle-Width="20px">
                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                    </telerik:GridButtonColumn>
                    <telerik:GridBoundColumn DataField="RoleName" HeaderText="Role Name" SortExpression="RoleName"
                        UniqueName="RoleName" ColumnEditorID="RoleName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Description" HeaderText="Description" SortExpression="Description"
                        UniqueName="Description" ColumnEditorID="Description">
                    </telerik:GridBoundColumn>
                </Columns>
                <EditFormSettings ColumnNumber="2" CaptionDataField="RoleName" CaptionFormatString="Edit Role {0}"
                    InsertCaption="Create Role">
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
</asp:Content>
