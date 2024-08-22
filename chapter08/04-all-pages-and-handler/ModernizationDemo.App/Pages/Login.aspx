<%@ Page Title="Constoso Shop Sign In" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ModernizationDemo.App.Login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row">
        <div class="col col-md-4 mx-auto">
            
            <h2>Sign in</h2>
            <p>Use your Contoso Shop account to sign in.</p>
            <asp:Login ID="Login1" runat="server" 
                       RenderOuterTable="false" 
                       FailureText="Invalid username or password."
                       OnAuthenticate="Login1_OnAuthenticate"
                       DestinationPageUrl="~/">
                <LayoutTemplate>
                    
                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="UserName" Text="User name" CssClass="form-label" />
                        <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="Password" Text="Password" CssClass="form-label" />
                        <asp:TextBox runat="server" ID="Password" CssClass="form-control" TextMode="Password" />
                    </div>
                    <div class="mb-4">
                        <asp:Button runat="server" Text="Sign In" CssClass="btn btn-primary" CommandName="Login" />
                    </div>
                    <p class="text-danger">
                        <asp:Literal id="FailureText" runat="server"></asp:Literal>
                    </p>
                </LayoutTemplate>
            </asp:Login>

        </div>
    </div>

</asp:Content>
