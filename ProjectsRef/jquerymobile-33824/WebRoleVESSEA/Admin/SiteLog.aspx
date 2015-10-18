<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="SiteLog.aspx.cs" Inherits="Web.Admin.SiteLog" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource"
            AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None">
            <ClientSettings EnableRowHoverStyle="true">
            </ClientSettings>
        </telerik:RadGrid>
    </div>
    <div>
        <asp:Button ID="btnExport" Text="Export to CSV" runat="server" OnClick="btnExport_Click">
        </asp:Button>
    </div>
</asp:Content>
