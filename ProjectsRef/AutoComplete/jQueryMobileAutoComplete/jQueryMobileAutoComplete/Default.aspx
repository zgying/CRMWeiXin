<%@ Page Title="" Language="C#" MasterPageFile="~/Design.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="jQueryMobileAutoComplete.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<meta name="viewport" content="width=device-width, initial-scale=1" />
<link rel="stylesheet" href="css/themes/default/jquery.mobile-1.3.1.min.css" />
<link rel="stylesheet" href="_assets/css/jqm-demos.css" />
<link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Open+Sans:300,400,700" />
<script src="js/jquery.js"></script>
<script src="_assets/js/index.js"></script>
<script src="js/jquery.mobile-1.3.1.min.js"></script>
<link href="jQueryMobileAutoComplete/styles.css" rel="stylesheet" />
<script src="jQueryMobileAutoComplete/jqm.autoComplete-1.5.2-min.js"></script>
<script src="jQueryMobileAutoComplete/code.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div data-role="page" id="mainPage">
    
<script type="text/javascript">
    $("#mainPage").bind("pageshow", function (e)
    {
        var data = 'Search.ashx';
        $("#<%=txtSearch.ClientID%>").autocomplete({
            target: $('#MytxtSearch'),
            source: data,
            callback: function (e)
            {
                var $a = $(e.currentTarget);
                $("#<%=txtSearch.ClientID%>").val($a.text());
                $("#<%=txtSearch.ClientID%>").autocomplete('clear');
            },
            minLength: 2,
            transition: "fade"
        });
    });
</script>

    <div>
        <asp:Label ID="lblCountry" runat="server" Text="Select Your Country : "></asp:Label>
        <asp:TextBox ID="txtSearch" placeholder="Search Country..." data-clear-btn="true" type="search" runat="server"></asp:TextBox>
        <ul id="MytxtSearch" data-role="listview" data-inset="true"></ul>
    </div>
</div>

</asp:Content>
