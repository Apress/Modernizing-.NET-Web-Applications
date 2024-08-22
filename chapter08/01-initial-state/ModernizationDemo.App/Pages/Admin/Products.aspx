<%@ Page Title="Contoso Shop Admin" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="ModernizationDemo.App.Admin.Products" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <h2>Products</h2>
    
    <div class="my-4">
        <asp:HyperLink runat="server" NavigateUrl='<%$ RouteUrl: RouteName=AdminProductCreate %>'
                       CssClass="btn btn-primary">
            <i class="bi bi-plus"></i> Add product
        </asp:HyperLink>
    </div>

    <asp:GridView ID="ProductsGrid" runat="server"
                  SelectMethod="GetData"
                  DeleteMethod="DeleteProduct"
                  OnRowDataBound="ProductsGrid_OnRowDataBound"
                  DataKeyNames="Id"
                  AutoGenerateColumns="False" 
                  BorderStyle="NotSet"
                  CssClass="table table-bordered"
                  AllowPaging="true"
                  PageSize="10"
                  PagerStyle-CssClass="grid-pager-row">
        <Columns>
            <asp:TemplateField HeaderText="Image">
                <ItemTemplate>
                    <img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("Name") %>" class="img-fluid" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:TemplateField HeaderText="Price">
                <ItemTemplate>
                    <asp:Literal ID="PriceLiteral" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# GetRouteUrl("AdminProductDetail", new { Id = Eval("Id") }) %>'>
                        <i class="bi bi-pencil"></i>
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandName="Delete" CausesValidation="false">
                        <i class="bi bi-trash"></i>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
</asp:Content>
