<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddEditCategories.aspx.cs" Inherits="WebRoleVESSEA.Admin.Store.AddEditCategories" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
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
                    <telerik:GridTemplateColumn ItemStyle-Width="15px">
                        <ItemTemplate>
                            <telerik:RadButton ID="RadButtonUP" runat="server" Width="15px" Height="15px" Text="MoveUp"
                                Visible='<%# Eval("MaxDisplayOrder").ToString()!="1" %>' Enabled="true" OnClick="MoveItem"
                                CommandName="Up" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EntityKey")%>'>
                                <Image ImageUrl="~/Images/ArrowUp2.gif" />
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn ItemStyle-Width="15px">
                        <ItemTemplate>
                            <telerik:RadButton ID="RadButtonDown" runat="server" Width="15px" Height="15px" Text="MoveDown"
                                Visible='<%# Eval("MaxDisplayOrder").ToString()!="1" %>' Enabled="true" OnClick="MoveItem"
                                CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EntityKey")%>'>
                                <Image ImageUrl="~/Images/ArrowDown2.gif" />
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="Name" HeaderText="Name" UniqueName="Name" />
                    <telerik:GridBoundColumn DataField="ParentName" HeaderText="Parent Name" UniqueName="ParentName" />
                    <telerik:GridCheckBoxColumn DataField="Deleted" HeaderText="Deleted" UniqueName="Deleted" />
                    <telerik:GridCheckBoxColumn DataField="IsMenu" HeaderText="IsMenu" UniqueName="IsMenu" />
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
                                    <telerik:RadTextBox ID="txtTitle" runat="server" Text='<%# Bind("Name") %>' Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Parent Category:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadComboBox runat="server" ID="ddParentCategory" Width="500px">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Menu:
                                    </label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsMenu" runat="server"></asp:CheckBox>
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
