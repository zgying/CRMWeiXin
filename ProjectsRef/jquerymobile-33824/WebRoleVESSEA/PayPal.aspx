<%@ Page Language="C#" MasterPageFile="~/App_Master/Mobile.Master" AutoEventWireup="true" CodeBehind="PayPal.aspx.cs" Inherits="Web.PayPal" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        SiteLog(window.location.pathname, false);
    </script>
    <br />
    <a href="https://developer.paypal.com/">Login Into Sandbox</a> 
    <br />
    <p>For testing first you must login into the PayPal developer portal</p>
    <p>PayPal Developer Site</p>
    <p>User: dev@vessea.com</p>
    <p>Pass: Password2009</p>
    <p>PayPal Test Buyer Account</p>
    <p>User: buyer_1244149127_per@vessea.com</p>
    <p>Pass: Password2009</p>
    <br />
    <p>After you are logged into PayPal sandbox click the following link to make a test purchase</p>
    <div id="PayPalUrl" runat="server"></div>
    <br />
    <%--<div><a href="#" onclick="javascript:window.open('https://www.paypal.com/us/cgi-bin/webscr?cmd=xpt/Marketing/popup/OLCWhatIsPayPal-outside','olcwhatispaypal','toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, width=400, height=350');"><img  src="https://www.paypal.com/en_US/i/bnr/horizontal_solution_PPeCheck.gif" border="0" alt="Solution Graphics"></a></div>--%>
</asp:Content>
