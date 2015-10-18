<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddEditPortals.aspx.cs" Inherits="Web.Admin.AddEditPortals" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="DivSuperUser" runat="server" visible="false">
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
                    <telerik:GridTemplateColumn HeaderText="PortalID" UniqueName="PortalID">
                        <ItemTemplate>
                            <asp:Label ID="lblPortalID" runat="server" Text='<%# Eval("PortalID") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" UniqueName="Name">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' Width="500px"></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <table>
                            <tr>
                                <td>
                                    <label>
                                        Portal Name:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="Name" runat="server" Text='<%# Bind("Name") %>' Width="500px">
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                        ControlToValidate="Name" CssClass="TxtValidate"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Master Page:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="MasterPage" runat="server" Text='<%# Bind("MasterPage") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                        ControlToValidate="MasterPage" CssClass="TxtValidate"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Theme:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadComboBox runat="server" ID="ddTheme" Width="500px">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadAsyncUpload runat="server" ID="AsyncUpload1" OnClientFileUploaded="uploadFile"
                                        OnFileUploaded="RadAsyncUpload_FileUploaded" MaxFileInputsCount="1" Width="500px">
                                        <Localization Select="Upload" />
                                    </telerik:RadAsyncUpload>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Portal Start Method:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RadTextBox1" runat="server" Text='<%# Bind("StartMethod") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Url Redirect Page:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="UrlRedirect" runat="server" Text='<%# Bind("UrlRedirect") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Facebook AppID:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="FacebookAppID" runat="server" Text='<%# Bind("FacebookAppID") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Faceboo Secret:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="FacebookSecret" runat="server" Text='<%# Bind("FacebookSecret") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Production PayPal:
                                    </label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkPayPalEnvironment" runat="server"></asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        PayPal Business:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="PayPalBusiness" runat="server" Text='<%# Bind("PayPalBusiness") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Email Host:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="EmailHost" runat="server" Text='<%# Bind("EmailHost") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Email Port:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="EmailPort" runat="server" Text='<%# Bind("EmailPort") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Email User:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="EmailUser" runat="server" Text='<%# Bind("EmailUser") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Email Pass:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="EmailPass" runat="server" Text='<%# Bind("EmailPass") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Use SSL:
                                    </label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkEmailSSL" runat="server"></asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Tax Rate:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RadTextBox2" runat="server" Text='<%# Bind("TaxRate") %>'
                                        Width="500px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Google Analytics:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="GoogleAnalytics" runat="server" Text='<%# Bind("GoogleAnalytics") %>'
                                        Width="500px" Height="250px" TextMode="MultiLine">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Robots Text:
                                    </label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RobotsText" runat="server" Text='<%# Bind("RobotsText") %>'
                                        Width="500px" Height="250px" TextMode="MultiLine">
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                        ControlToValidate="Name" CssClass="TxtValidate"></asp:RequiredFieldValidator>
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
        <script type="text/javascript">
         //<![CDATA[
            function uploadFile() {
                __doPostBack();
            }
        //]]>
        </script>
    </div>
</asp:Content>
