<%@ Page Language="C#" MasterPageFile="~/App_Master/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="Web.Account.Login" %>

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
    <form id="frmLogin" action="" runat="server">
    <h2>Login</h2>
    <fieldset>
        <legend>Not registered with us? <a data-ajax="false" href="/Account/Register.aspx">Register</a></legend>
        <div data-role="fieldcontain" id="DivErrorMsg" title="">
            <label for="lblErrorMessage" id="lbl_ErrorMessage">
            </label>
            <asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
        <div data-role="fieldcontain" id="DivEmail" title="Please provide a email address">
            <label data-mini="true" for="email" id="lbl_email">
                Email:</label>
            <input data-mini="true" type="text" id="email" name="email" title="Please provide a email address" />
        </div>
        <div data-role="fieldcontain" id="DivPassword" title="Please provide a password">
            <label data-mini="true" for="password" id="lbl_password">
                Password:</label>
            <input data-mini="true" type="password" id="password" name="password" />
            <p>
                <a href="/Account/Forgot.aspx">Forgot your password?</a></p>
        </div>
        <div data-role="fieldcontain" id="DivBtn1" title="">
            <asp:Button data-mini="true" ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" UseSubmitBehavior="False" />
        </div>
        <div data-role="fieldcontain" id="DivBtn2" title="" runat="server">
            <label data-mini="true" for="img_fbLogin" id="Label1">
                Or Log in with your Facebook account</label>
            <br />
            <br />
            <asp:Button data-mini="true" ID="btnFBLogin" runat="server" Text="Sign in with Facebook" UseSubmitBehavior="False"
                data-icon="facebook" data-theme="c" OnClick="btnFBLogin_Click" />
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
                    password: {
                        required: true
                    }
                },
                messages: {
                    password: {
                        required: "* Please provide a password"
                    },
                    email: {
                        required: "* Please provide a email address",
                        email: "* Please provide a valid email address"
                    }
                }
            });
        });
    </script>
</asp:Content>
