<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddEditProduct.aspx.cs" Inherits="WebRoleVESSEA.Admin.Store.AddEditProduct" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource"
            OnDeleteCommand="RadGrid1_DeleteCommand" OnInsertCommand="RadGrid1_InsertCommand"
            OnUpdateCommand="RadGrid1_UpdateCommand" OnItemDataBound="RadGrid1_ItemDataBound"
            AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" ShowStatusBar="true"
            PagerStyle-Position="TopAndBottom">
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" DataKeyNames="EntityKey"
                InsertItemPageIndexAction="ShowItemOnCurrentPage">
                <Columns>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" ItemStyle-Width="40px" />
                    <telerik:GridTemplateColumn DataField="EntityKey" HeaderText="EntityKey" UniqueName="EntityKey"
                        Visible="false">
                        <InsertItemTemplate>
                            <telerik:RadTextBox ID="RadTextBox1" runat="server" Text='<%# Bind("EntityKey") %>' />
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="RadTextBox1" runat="server" Text='<%# Eval("EntityKey") %>'
                                ReadOnly="true" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="Name" HeaderText="Name" UniqueName="Name" />
                    <telerik:GridBoundColumn DataField="Price" HeaderText="Price" UniqueName="Price" />
                    <telerik:GridBoundColumn DataField="CategoryName" HeaderText="Category" UniqueName="CategoryName" />
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <table>
                            <tr>
                                <td>
                                    <label>
                                        Name:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="Name" runat="server" Text='<%# Bind("Name") %>' Width="800px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Price:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="Price" Type="Currency" runat="server" Text='<%# Bind("Price") %>'
                                        Width="800px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Category:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadComboBox runat="server" ID="ddCategory" Width="800px">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Deleted:
                                    </label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDeleted" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Description:
                                </td>
                                <td>
                                    <telerik:RadEditor ID="RadEditor1" runat="server" Width="800px">
                                    </telerik:RadEditor>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <telerik:RadButton ID="RadButtonUpdate" runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'
                                        Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'>
                                    </telerik:RadButton>
                                    <telerik:RadButton ID="RadButtonCancel" runat="server" CommandName="Cancel" Text="Cancel">
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </FormTemplate>
                    <FormTableButtonRowStyle HorizontalAlign="Left" CssClass="EditFormButtonRow"></FormTableButtonRowStyle>
                </EditFormSettings>
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="RowDblClick" />
            </ClientSettings>
        </telerik:RadGrid>
    </div>
</asp:Content>
