using BlazorGrpc.Service;

using Microsoft.AspNetCore.Components;

namespace BlazorGrpc.Components.Pages;

public partial class Home
{
    [Inject]
    public ServerProductService ServerProduct { get; set; }


    protected override async Task OnInitializedAsync()
    {
		try
		{
            await ServerProduct.CreateProduct(new gRPC.CreateProductRequest { Name = "Test", Price = 10 }, null);

            var prods = await ServerProduct.ListProducts(null , null);
        }
		catch (Exception ex)
		{
			throw;
		}

    }

}
