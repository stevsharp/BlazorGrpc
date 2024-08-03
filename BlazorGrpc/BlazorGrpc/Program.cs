
using BlazorAppData.Interrface;
using BlazorAppData.Repository;

using BlazorGrpc.Components;
using BlazorGrpc.Model;
using BlazorGrpc.Service;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddGrpc(x=>x.EnableDetailedErrors = true);
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlite("Data Source=products.db"));
builder.Services.AddScoped<ServerProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();



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
