
using BlazorAppData.Interrface;
using BlazorAppData.Repository;
using BlazorAppData.UnitOfWork;

using Blazored.LocalStorage;

using BlazorGrpc.Components;
using BlazorGrpc.Handler;
using BlazorGrpc.Model;
using BlazorGrpc.Notification;
using BlazorGrpc.Service;
using BlazorGrpc.UIServices;

using BlazorGrpcSimpleMediater;
using Microsoft.EntityFrameworkCore;

using MudBlazor;
using MudBlazor.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddGrpc(x=>x.EnableDetailedErrors = true);
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlite("Data Source=products.db"));

builder.Services.AddScoped<DialogUIService>();
builder.Services.AddScoped<ServerProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Services.AddLazyCache();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddScoped<IMediator, Mediator>();

builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));


builder.Services.AddScoped<IHandler<CreateProductCommand, CreateProductResponse>, CreateProductHandler>();
builder.Services.AddScoped<IHandler<GetProductByIdCommand, GetByIdProductResponse>, GetByIdProductHandler>();
builder.Services.AddScoped<IHandler<DeleteProductByIdCommand, bool>, DeleteByIdProductHandler>();
builder.Services.AddScoped<IHandler<UpdateProductCommand, UpdateProductResponse>, UpdateProductHandler>();
builder.Services.AddScoped<IHandler<GetAllProductCommand, List<GetAllProductResponse>>, GetAllProductHandler>();
builder.Services.AddScoped<INotificationHandler<ProductCreatedNotification>, ProducCreatedNotificationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorGrpc.Client._Imports).Assembly);

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGrpcService<ServerProductService>()
            .EnableGrpcWeb();

app.Run();
