<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="FileManager.aspx.cs" Inherits="Web.Admin.FileManager" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent" data-role="none">
    <div>
        <telerik:RadFileExplorer ID="RadFileExplorer1" runat="server" Width="100%">
        </telerik:RadFileExplorer>
    </div>
</asp:Content>
