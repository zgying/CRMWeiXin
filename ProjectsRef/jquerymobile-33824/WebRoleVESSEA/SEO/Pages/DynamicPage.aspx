<%@ Page Language="C#" MasterPageFile="~/App_Master/Mobile.Master" AutoEventWireup="true"
    CodeBehind="DynamicPage.aspx.cs" Inherits="Web.SEO.Pages.DynamicPage" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="SecoundContent">
    <div runat="server" id="ContentText"></div>
    <script type="text/javascript">
        SiteLog(window.location.pathname, false);
    </script>
</asp:Content>
