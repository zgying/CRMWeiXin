<%@ Page Language="C#" MasterPageFile="~/App_Master/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Forgot.aspx.cs" Inherits="Web.Account.Forgot" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="SecoundContent">
    <script type="text/javascript">
        SiteLog(window.location.pathname, false);
    </script>
    <style type="text/css">
        label.error
        {
            color: red;
            line-height: 1.4;
            margin-top: 0.5em;
            width: 100%;
            float: none;
        }
        @media screen and (orientation: portrait)
        {
            label.error
            {
                margin-left: 0;
                display: block;
            }
        }
        @media screen and (orientation: landscape)
        {
            label.error
            {
                display: inline-block;
                margin-left: 22%;
            }
        }
        #DivHuman
        {
            visibility: hidden;
            display: none;
        }
    </style>
    <form id="frmLogin" action="" runat="server">
    <h2>
        Forgot Your Password?</h2>
    <hr />
    <p>
        Enter your email address below, and we'll email you your password.</p>
    <hr />
    <fieldset>
        <legend></legend>
        <div data-role="fieldcontain" id="DivErrorMsg">
            <label for="lblErrorMessage" id="lbl_ErrorMessage">
            </label>
            <asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
        <div data-role="fieldcontain" title="Please provide a email address" id="DivEmail">
            <label for="email" id="lbl_email">
                Email Address:</label>
            <input type="text" id="email" name="email" title="Please provide a email address" />
        </div>
        <div data-role="fieldcontain" title="Please provide an answer" id="DivHuman">
            <label for="human" id="lbl_human" class="txtClass">
                Are you human? please add 1 and 4 and type the answer here:</label>
            <input type="text" id="human" class="txtClass" name="human" value="" />
        </div>
        <div data-role="fieldcontain" id="DivBtn1">
            <asp:Button ID="btnForgot" runat="server" Text="Submit" OnClick="btnForgot_Click" />
            <br />
            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
        </div>
    </fieldset>
    <br />
    </form>
    <script type="text/javascript">

        //Execute scripts when the DOM is ready. this is a good habit
        $().ready(function () {
            $("#frmLogin").validate({
                rules: {
                    email: {
                        required: true,
                        email: true
                    },
                    human: {
                        required: true
                    }
                },
                messages: {
                    email: {
                        required: "* Please provide a email address",
                        email: "* Please provide a valid email address"
                    }
                }
            });
        });
    </script>
</asp:Content>
