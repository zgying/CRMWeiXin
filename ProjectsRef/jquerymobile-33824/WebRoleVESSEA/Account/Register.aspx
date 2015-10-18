<%@ Page Language="C#" MasterPageFile="~/App_Master/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="Web.Account.Register" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="SecoundContent">
    <script type="text/javascript">
        SiteLog(window.location.pathname, false);
    </script>
    <style type="text/css">
        .ui-icon-facebook
        {
            background-image: url("/Images/facebook_18.png");
        }
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
    </style>
    <form id="frmRegister" action="" runat="server">
    <h2>Register</h2>
    <fieldset>
        <legend></legend>
        <div data-role="fieldcontain" id="DivErrorMsg" title="">
            <label for="lblErrorMessage" id="lbl_ErrorMessage">
            </label>
            <asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
        <div data-role="fieldcontain" id="DivEmail" title="Please provide a email address">
            <label for="email" id="lbl_email">
                Email:</label>
            <input type="text" id="email" name="email" value="" title="Please provide a email address" />
        </div>
        <div data-role="fieldcontain" id="DivPassword" title="Please provide a password">
            <label for="password" id="lbl_password">
                Password:</label>
            <input id="password" name="password" type="password" />
        </div>
        <div data-role="fieldcontain" id="ConfirmPassword" title="Please confirm password">
            <label for="confirm_password" id="lbl_confirm_password">
                Confirm:</label>
            <input id="confirm_password" name="confirm_password" type="password" />
        </div>
        <div data-role="fieldcontain" id="DivBtn1" title="">
            <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
        </div>
        <div data-role="fieldcontain" id="DivBtn2" title="">
            <label for="btnFBLogin" id="lblFB">
                Or Register with your Facebook account</label>
            <asp:Button ID="btnFBLogin" runat="server" Text="Sign in with Facebook" UseSubmitBehavior="False"
                data-icon="facebook" data-theme="c" OnClick="btnFBLogin_Click" />
        </div>
    </fieldset>
    </form>
    <script type="text/javascript">
        $().ready(function () {
            // validate signup form on keyup and submit
            $("#frmRegister").validate({
                rules: {
                    email: {
                        required: true,
                        email: true
                    },
                    password: {
                        required: true
                    },
                    confirm_password: {
                        required: true,
                        equalTo: "#password"
                    }
                },
                messages: {
                    email: {
                        required: "* Please provide a email address",
                        email: "* Please provide a valid email address"
                    },
                    password: {
                        required: "* Please provide a password"
                    },
                    confirm_password: {
                        required: "* Please provide a password",
                        equalTo: "* Please enter the same password as above"
                    }
                }
            });
        });
    </script>
</asp:Content>
