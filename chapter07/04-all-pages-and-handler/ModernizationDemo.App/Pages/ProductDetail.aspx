<%@ Page Title="Contoso Shop Product" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetail.aspx.cs" Inherits="ModernizationDemo.App.ProductDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:FormView ID="ProductForm" runat="server" 
                  SelectMethod="GetData"
                  OnDataBound="ProductForm_OnDataBound">
        <ItemTemplate>
            <div class="row">
                <div class="col col-md-4">
                    <img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("Name") %>" class="img-fluid"/>
                </div>

                <div class="col col-md-8">
                    <h2><%#: Eval("Name") %></h2>
                    <p class="mb-4">
                        <i class="bi bi-star-fill"></i>
                        <i class="bi bi-star-fill"></i>
                        <i class="bi bi-star-fill"></i>
                        <i class="bi bi-star-fill"></i>
                        <i class="bi bi-star"></i>
                    </p>
                    <p class="lead"><%#: Eval("Description") %></p>
                    <h4 class="mb-4">
                        <asp:Literal runat="server" ID="PriceLiteral" />
                    </h4>
                    
                    <hr />
                    
                    <div class="row mt-4">
                        <div class="col">
                            <div class="mb-3">
                                <asp:Label runat="server" AssociatedControlID="StockLiteral" Text="Availability" CssClass="form-label"/>
                                <asp:TextBox runat="server" ID="StockLiteral" Text="In stock" ReadOnly="true" CssClass="form-control-plaintext fw-bold text-success" />
                            </div>
                        </div>
                        <div class="col">
                            <div class="mb-3">
                                <asp:Label runat="server" 
                                           Text="Quantity" 
                                           AssociatedControlID="QuantityTextBox"
                                           CssClass="form-label"/>
                                <asp:TextBox runat="server" 
                                             Text="1" 
                                             ID="QuantityTextBox" 
                                             TextMode="Number" 
                                             CssClass="form-control"
                                             style="width: 5em"/>
                            </div>
                        </div>
                        <div class="col text-end">
                            <asp:Button runat="server" 
                                        Text="Add to cart" 
                                        CssClass="btn btn-primary btn-lg" />
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:FormView>

</asp:Content>
