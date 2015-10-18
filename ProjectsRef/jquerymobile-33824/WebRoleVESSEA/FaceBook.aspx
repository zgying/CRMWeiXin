<%@ Page Language="C#" MasterPageFile="~/App_Master/Mobile.Master" AutoEventWireup="true" CodeBehind="FaceBook.aspx.cs" Inherits="Web.FaceBook" %>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div id="fb-root"></div>
    
    <script type="text/javascript" src="/Scripts/facebook.js"></script>

    <fb:login-button autologoutlink="true" perms="email,user_birthday,status_update,publish_stream,user_about_me"></fb:login-button>
    
    <%--<p><fb:login-button autologoutlink="true" show-faces="true" scope="email"></fb:login-button></p>--%>
    
    <%--<p><a href="#" onclick="fqlQuery(); return false;">FQL Query</a></p>--%>
    
    <p><a href="#" data-ajax="false" data-role="button" data-inline="true" onclick="fqlQuery();">FQL Query</a></p>
    
    <div id="login" style="display: none"></div>
    
    <div id="name" name="name"></div>

</asp:Content>
