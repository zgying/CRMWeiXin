﻿<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddEditAnswers.aspx.cs" Inherits="Web.Admin.Survey.AddEditAnswers" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource"
            OnDeleteCommand="RadGrid1_DeleteCommand" OnInsertCommand="RadGrid1_InsertCommand"
            OnUpdateCommand="RadGrid1_UpdateCommand" OnItemDataBound="RadGrid1_ItemDataBound">
            <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" DataKeyNames="EntityKey"
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
                    <telerik:GridBoundColumn DataField="Answer" HeaderText="Answer" UniqueName="Answer" />
                    <telerik:GridBoundColumn DataField="AnswerGroup" HeaderText="Answer Group" UniqueName="AnswerGroup" />
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <table>
                            <tr>
                                <td>
                                    <label>
                                        Answer:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="Answer" runat="server" Text='<%# Bind("Answer") %>'>
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                        ControlToValidate="Answer" CssClass="TxtValidate"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Answer Group:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadComboBox runat="server" ID="ddAnswerGroupID">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <telerik:RadButton ID="RadButtonUpdate" runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'
                                        Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'>
                                    </telerik:RadButton>
                                    <telerik:RadButton ID="RadButtonCancel" runat="server" CommandName="Cancel" Text="Cancel"
                                        CausesValidation="false">
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
