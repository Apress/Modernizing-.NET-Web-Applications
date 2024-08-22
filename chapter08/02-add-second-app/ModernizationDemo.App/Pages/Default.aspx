<%@ Page Title="Contoso Shop Products" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ModernizationDemo.App.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ListView ID="ProductsList" runat="server" 
                  SelectMethod="GetData"
                  OnItemDataBound="ProductsList_OnItemDataBound"
                  AllowPaging="true"
                  ItemPlaceholderID="ItemPlaceHolder">
        <LayoutTemplate>
            <div class="row row-cols-1 row-cols-md-3 g-4">
                <asp:PlaceHolder runat="server" ID="ItemPlaceHolder"></asp:PlaceHolder>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div class="col">
                <div class="card h-100">
                    
                    <img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("Name") %>"/>

                    <div class="card-body">
                        <h5 class="card-title"><%#: Eval("Name") %></h5>
                        <p class="card-text"><%#: Eval("Description") %></p>
                        <a class="btn btn-primary" 
                           href="<%# GetRouteUrl("ProductDetail", new { Id = Eval("Id") }) %>">
                            <asp:Literal runat="server" ID="PriceLiteral" />
                        </a>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:ListView>
    
    <div class="mt-4 text-center">
        <asp:DataPager ID="ProductsPager" runat="server" PagedControlID="ProductsList" PageSize="12">
            <Fields>
                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" 
                                            ButtonCssClass="btn btn-secondary" />
            </Fields>
        </asp:DataPager>
    </div>

</asp:Content>
