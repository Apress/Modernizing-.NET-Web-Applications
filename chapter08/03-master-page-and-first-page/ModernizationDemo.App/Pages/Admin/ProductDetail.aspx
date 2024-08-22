<%@ Page Title="Contoso Shop Admin" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetail.aspx.cs" Inherits="ModernizationDemo.App.Pages.Admin.ProductDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:FormView ID="ProductForm" runat="server" 
        SelectMethod="GetData"
        InsertMethod="InsertData"
        UpdateMethod="UpdateData"
        RenderOuterTable="false">
        <InsertItemTemplate>
                
            <h2>Create product</h2>

            <div class="mb-3">
                <asp:Label runat="server" AssociatedControlID="NameTextBox" 
                           Text="Product name" CssClass="form-label" />
                <asp:TextBox runat="server" ID="NameTextBox" 
                             CssClass="form-control" 
                             Text='<%# Bind("Name") %>' />
                <asp:RequiredFieldValidator runat="server" 
                                            ControlToValidate="NameTextBox" 
                                            CssClass="text-danger" 
                                            ErrorMessage="Product name is required" />
            </div>
            <div class="mb-3">
                <asp:Label runat="server" AssociatedControlID="DescriptionTextBox" 
                           Text="Product name" CssClass="form-label" />
                <asp:TextBox runat="server" ID="DescriptionTextBox" 
                             CssClass="form-control" TextMode="MultiLine" Rows="5" 
                             Text='<%# Bind("Description") %>' />
                <asp:RequiredFieldValidator runat="server" 
                                            ControlToValidate="DescriptionTextBox" 
                                            CssClass="text-danger" 
                                            ErrorMessage="Product description is required" />
            </div>
            <div class="mb-3">
                <asp:Label runat="server" AssociatedControlID="ImageUrlTextBox" 
                           Text="Image URL" CssClass="form-label" />
                <asp:TextBox runat="server" ID="ImageUrlTextBox" 
                             CssClass="form-control" 
                             Text='<%# Bind("ImageUrl") %>' />
                <asp:RequiredFieldValidator runat="server" 
                                            ControlToValidate="ImageUrlTextBox" 
                                            CssClass="text-danger" 
                                            ErrorMessage="Image is required" />
            </div>
            
            <div class="text-center">
                <asp:Button runat="server" Text="Add product" CommandName="Insert" CssClass="btn btn-primary"/>
                <asp:HyperLink runat="server" Text="Cancel" NavigateUrl="<%$ RouteUrl: RouteName=AdminProducts %>" CssClass="btn btn-secondary" />
            </div>
        </InsertItemTemplate>
        
        <EditItemTemplate>
        
            <h2>Edit product</h2>

            <div class="mb-3">
                <asp:Label runat="server" AssociatedControlID="NameTextBox" 
                           Text="Product name" CssClass="form-label" />
                <asp:TextBox runat="server" ID="NameTextBox" 
                             CssClass="form-control" 
                             Text='<%# Bind("Name") %>' />
                <asp:RequiredFieldValidator runat="server" 
                                            ControlToValidate="NameTextBox" 
                                            CssClass="text-danger" 
                                            ErrorMessage="Product name is required" />
            </div>
            <div class="mb-3">
                <asp:Label runat="server" AssociatedControlID="DescriptionTextBox" 
                           Text="Product name" CssClass="form-label" />
                <asp:TextBox runat="server" ID="DescriptionTextBox" 
                             CssClass="form-control" TextMode="MultiLine" Rows="5" 
                             Text='<%# Bind("Description") %>' />
                <asp:RequiredFieldValidator runat="server" 
                                            ControlToValidate="DescriptionTextBox" 
                                            CssClass="text-danger" 
                                            ErrorMessage="Product description is required" />
            </div>
            <div class="mb-3">
                <asp:Label runat="server" AssociatedControlID="ImageUrlTextBox" 
                           Text="Image URL" CssClass="form-label" />
                <asp:TextBox runat="server" ID="ImageUrlTextBox" 
                             CssClass="form-control" 
                             Text='<%# Bind("ImageUrl") %>' />
                <asp:RequiredFieldValidator runat="server" 
                                            ControlToValidate="ImageUrlTextBox" 
                                            CssClass="text-danger" 
                                            ErrorMessage="Image is required" />
            </div>
            
            <h3>Prices</h3>
            <asp:GridView runat="server" ID="PricesGrid" 
                          AutoGenerateColumns="false"
                          SelectMethod="GetPrices"
                          DataKeyNames="CurrencyCode"
                          ShowHeaderWhenEmpty="true"
                          UpdateMethod="UpdatePrice"
                          DeleteMethod="DeletePrice"
                          CssClass="table table-bordered">
                <Columns>
                    <asp:TemplateField HeaderText="Currency">
                        <ItemTemplate>
                            <%# Eval("CurrencyCode") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%# Eval("CurrencyCode") %>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Price">
                        <ItemTemplate>
                            <%# Eval("Price", "{0:n2}") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" Id="PriceTextBox" 
                                         Text='<%# Bind("Price", "{0:n2}") %>' 
                                         CssClass="form-control"/>
                            <asp:RequiredFieldValidator runat="server" 
                                                        ControlToValidate="PriceTextBox" 
                                                        CssClass="text-danger"
                                                        ErrorMessage="Price is required!"
                                                        ValidationGroup="UpdatePrice"/>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="EditButton" CommandName="Edit" CausesValidation="false">
                                <i class="bi bi-pencil"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" ID="SaveButton" CommandName="Update" ValidationGroup="UpdatePrice" CssClass="btn btn-primary">
                                <i class="bi bi-check"></i>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="CancelButton" CommandName="Cancel" CausesValidation="false" CssClass="btn btn-secondary">
                                <i class="bi bi-x"></i>
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="DeleteButton" CommandName="Delete" CausesValidation="false">
                                <i class="bi bi-trash"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            
            <asp:FormView runat="server" DefaultMode="Insert"
                          RenderOuterTable="false"
                          InsertMethod="InsertPrice"
                          SelectMethod="GetPrice">
                <InsertItemTemplate>
                    <div class="card mb-4">
                        <div class="card-body">
                            <div class="d-flex gap-4 align-items-center">
                                <div class="flex-grow-1">
                                    <asp:Label runat="server" AssociatedControlID="InsertCurrencyDropDown" Text="Currency" />
                                    <asp:DropDownList runat="server" 
                                                      ID="InsertCurrencyDropDown" 
                                                      CssClass="form-select"
                                                      SelectedValue='<%# Bind("CurrencyCode") %>'>
                                        <Items>
                                            <asp:ListItem Text="USD" Value="USD" />
                                            <asp:ListItem Text="EUR" Value="EUR" />
                                            <asp:ListItem Text="JPY" Value="JPY" />
                                            <asp:ListItem Text="GBP" Value="GBP" />
                                        </Items>
                                    </asp:DropDownList>
                                    <asp:CustomValidator runat="server"
                                                         ControlToValidate="InsertCurrencyDropDown"
                                                         CssClass="text-danger"
                                                         ErrorMessage="Price for this currency is already set!"
                                                         ValidationGroup="InsertPrice"
                                                         OnServerValidate="InsertCurrencyDropDown_ServerValidate"/>
                                </div>
                                <div class="flex-grow-1">
                                    <asp:Label runat="server" AssociatedControlID="InsertPriceTextBox" Text="Price" />
                                    <asp:TextBox runat="server" Id="InsertPriceTextBox" 
                                                 ValidationGroup="InsertPrice"
                                                 Text='<%# Bind("Price") %>'
                                                 CssClass="form-control"/>
                                    <asp:RequiredFieldValidator runat="server" 
                                                                ControlToValidate="InsertPriceTextBox" 
                                                                CssClass="text-danger"
                                                                ErrorMessage="Price is required!"
                                                                ValidationGroup="InsertPrice" />
                                </div>
                                <div>
                                    <asp:LinkButton runat="server" ID="AddPriceButton" ValidationGroup="InsertPrice" CssClass="btn btn-secondary" CommandName="Insert">
                                        <i class="bi bi-plus"></i> Add price
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>
                    </div>
                </InsertItemTemplate>
            </asp:FormView>

            <div class="mt-4 text-center">
                <asp:Button runat="server" Text="Save changes" CommandName="Update" CssClass="btn btn-primary"/>
                <asp:HyperLink runat="server" Text="Cancel" NavigateUrl="<%$ RouteUrl: RouteName=AdminProducts %>" CssClass="btn btn-secondary" />
            </div>
        </EditItemTemplate>
    </asp:FormView>

</asp:Content>
