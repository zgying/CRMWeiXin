<%@ Page Language="C#" MasterPageFile="~/App_Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="ChangeSkin.aspx.cs" Inherits="Web.Admin.ChangeSkin" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="True" PersistenceKey="QsfSkin"
            PersistenceMode="Session">
        </telerik:RadSkinManager>
    </div>
</asp:Content>
