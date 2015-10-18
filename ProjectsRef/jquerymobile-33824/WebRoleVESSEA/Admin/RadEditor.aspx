<%@ Page Language="C#" MasterPageFile="~/App_Master/Empty.Master" AutoEventWireup="true"
    CodeBehind="RadEditor.aspx.cs" Inherits="Web.Admin.RadEditor" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent" data-role="none">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadEditor ID="RadEditor1" runat="server">
    </telerik:RadEditor>
    </form>
</asp:Content>
