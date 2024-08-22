using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CoreWCF.IdentityModel.Policy;
using CoreWCF.Security;
using ModernizationDemo.Wcf.Contracts;
using ModernizationDemo.WcfCore;
using ModernizationDemo.WcfCore.Security;

var builder = WebApplication.CreateBuilder(args);

// configure HTTP and TCP endpoints
builder.WebHost
    .UseKestrel(options =>
    {
        options.ListenAnyIP(5217);
        options.ListenAnyIP(7226, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    })
    .UseNetTcp(9001);

// Add WSDL support
builder.Services
    .AddServiceModelServices()
    .AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

// register services
app.UseServiceModel(options =>
{
    var httpBinding = new BasicHttpBinding();
    var httpsBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
    var tcpBinding = new NetTcpBinding(SecurityMode.None);
    var wsBinding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential)
    {
        Security = new WSHTTPSecurity()
        {
            Mode = SecurityMode.TransportWithMessageCredential,
            Transport = new HttpTransportSecurity()
            {
                ClientCredentialType = HttpClientCredentialType.None
            },
            Message = new NonDualMessageSecurityOverHttp()
            {
                ClientCredentialType = MessageCredentialType.UserName
            }
        }
    };

    options
        .AddService<OrderService>(serviceOptions =>
        {
            serviceOptions.BaseAddresses.Add(new Uri("http://localhost:5217/"));
            serviceOptions.BaseAddresses.Add(new Uri("https://localhost:7226/"));
            serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
        })
        .AddServiceEndpoint<OrderService, IOrderService>(httpBinding, "OrderService.svc")
        .AddServiceEndpoint<OrderService, IOrderService>(httpsBinding, "OrderService.svc")
        .AddServiceEndpoint<OrderService, IOrderService>(tcpBinding, "OrderService.svc");

    options
        .AddService<ProductService>(serviceOptions =>
        {
            serviceOptions.BaseAddresses.Add(new Uri("https://localhost:7226/"));
            serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
        })
        .AddServiceEndpoint<ProductService, IProductService>(wsBinding, "ProductService.svc");

    options.ConfigureServiceHostBase<ProductService>(hostOptions =>
    {
        hostOptions.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
        hostOptions.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUserNamePasswordValidator();

        hostOptions.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
        hostOptions.Authorization.ExternalAuthorizationPolicies = (new IAuthorizationPolicy[] { new CustomRoleAuthorizationPolicy() }).AsReadOnly();
    });
});

// enable metadata endpoint
var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
serviceMetadataBehavior.HttpGetEnabled = true;
serviceMetadataBehavior.HttpsGetEnabled = true;

app.Run();
