//using zudioclone.Core.Services;

//WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//builder.CreateUmbracoBuilder()
//    .AddBackOffice()
//    .AddWebsite()
//    .AddDeliveryApi()
//    .AddComposers()
//    .Build();

//WebApplication app = builder.Build();

//builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<IAccountService, AccountService>();


//await app.BootUmbracoAsync();


//app.UseUmbraco()
//    .WithMiddleware(u =>
//    {
//        u.UseBackOffice();
//        u.UseWebsite();
//    })
//    .WithEndpoints(u =>
//    {
//        u.UseInstallerEndpoints();
//        u.UseBackOfficeEndpoints();
//        u.UseWebsiteEndpoints();
//    });

//await app.RunAsync();



using zudioclone.Core.Services.Interface;
using zudioclone.Core.Services;
using Umbraco.Cms.Core.Web;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);






// Create the Umbraco builder and build it
builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

// Register services
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAccountService, AccountService>(); // Register the IAccountService with its implementation


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000") // React app origin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // If using authentication
    });
});





WebApplication app = builder.Build();

// Apply CORS middleware
app.UseCors("AllowReactApp");

await app.BootUmbracoAsync();

// Configure middleware
app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
