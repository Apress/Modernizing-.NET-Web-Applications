﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModernizationDemo.WcfTests.Orders {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Orders.IOrderService")]
    public interface IOrderService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/GetOrders", ReplyAction="http://tempuri.org/IOrderService/GetOrdersResponse")]
        ModernizationDemo.BusinessLogic.Models.OrderModel[] GetOrders();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/GetOrders", ReplyAction="http://tempuri.org/IOrderService/GetOrdersResponse")]
        System.Threading.Tasks.Task<ModernizationDemo.BusinessLogic.Models.OrderModel[]> GetOrdersAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/GetOrder", ReplyAction="http://tempuri.org/IOrderService/GetOrderResponse")]
        ModernizationDemo.BusinessLogic.Models.OrderModel GetOrder(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/GetOrder", ReplyAction="http://tempuri.org/IOrderService/GetOrderResponse")]
        System.Threading.Tasks.Task<ModernizationDemo.BusinessLogic.Models.OrderModel> GetOrderAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/AddOrder", ReplyAction="http://tempuri.org/IOrderService/AddOrderResponse")]
        int AddOrder(ModernizationDemo.BusinessLogic.Models.OrderCreateModel order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/AddOrder", ReplyAction="http://tempuri.org/IOrderService/AddOrderResponse")]
        System.Threading.Tasks.Task<int> AddOrderAsync(ModernizationDemo.BusinessLogic.Models.OrderCreateModel order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/CalculateTotalPrice", ReplyAction="http://tempuri.org/IOrderService/CalculateTotalPriceResponse")]
        decimal CalculateTotalPrice(ModernizationDemo.BusinessLogic.Models.OrderCreateModel order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/CalculateTotalPrice", ReplyAction="http://tempuri.org/IOrderService/CalculateTotalPriceResponse")]
        System.Threading.Tasks.Task<decimal> CalculateTotalPriceAsync(ModernizationDemo.BusinessLogic.Models.OrderCreateModel order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/CompleteOrder", ReplyAction="http://tempuri.org/IOrderService/CompleteOrderResponse")]
        void CompleteOrder(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/CompleteOrder", ReplyAction="http://tempuri.org/IOrderService/CompleteOrderResponse")]
        System.Threading.Tasks.Task CompleteOrderAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/CancelOrder", ReplyAction="http://tempuri.org/IOrderService/CancelOrderResponse")]
        void CancelOrder(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/CancelOrder", ReplyAction="http://tempuri.org/IOrderService/CancelOrderResponse")]
        System.Threading.Tasks.Task CancelOrderAsync(int id);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IOrderServiceChannel : ModernizationDemo.WcfTests.Orders.IOrderService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OrderServiceClient : System.ServiceModel.ClientBase<ModernizationDemo.WcfTests.Orders.IOrderService>, ModernizationDemo.WcfTests.Orders.IOrderService {
        
        public OrderServiceClient() {
        }
        
        public OrderServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OrderServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OrderServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OrderServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public ModernizationDemo.BusinessLogic.Models.OrderModel[] GetOrders() {
            return base.Channel.GetOrders();
        }
        
        public System.Threading.Tasks.Task<ModernizationDemo.BusinessLogic.Models.OrderModel[]> GetOrdersAsync() {
            return base.Channel.GetOrdersAsync();
        }
        
        public ModernizationDemo.BusinessLogic.Models.OrderModel GetOrder(int id) {
            return base.Channel.GetOrder(id);
        }
        
        public System.Threading.Tasks.Task<ModernizationDemo.BusinessLogic.Models.OrderModel> GetOrderAsync(int id) {
            return base.Channel.GetOrderAsync(id);
        }
        
        public int AddOrder(ModernizationDemo.BusinessLogic.Models.OrderCreateModel order) {
            return base.Channel.AddOrder(order);
        }
        
        public System.Threading.Tasks.Task<int> AddOrderAsync(ModernizationDemo.BusinessLogic.Models.OrderCreateModel order) {
            return base.Channel.AddOrderAsync(order);
        }
        
        public decimal CalculateTotalPrice(ModernizationDemo.BusinessLogic.Models.OrderCreateModel order) {
            return base.Channel.CalculateTotalPrice(order);
        }
        
        public System.Threading.Tasks.Task<decimal> CalculateTotalPriceAsync(ModernizationDemo.BusinessLogic.Models.OrderCreateModel order) {
            return base.Channel.CalculateTotalPriceAsync(order);
        }
        
        public void CompleteOrder(int id) {
            base.Channel.CompleteOrder(id);
        }
        
        public System.Threading.Tasks.Task CompleteOrderAsync(int id) {
            return base.Channel.CompleteOrderAsync(id);
        }
        
        public void CancelOrder(int id) {
            base.Channel.CancelOrder(id);
        }
        
        public System.Threading.Tasks.Task CancelOrderAsync(int id) {
            return base.Channel.CancelOrderAsync(id);
        }
    }
}
