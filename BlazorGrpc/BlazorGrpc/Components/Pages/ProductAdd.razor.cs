
using BlazorGrpc.Model;
using BlazorGrpc.Service;
using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace BlazorGrpc.Components.Pages;

public partial class ProductAdd
{
    [Inject]
    protected ServerProductService? ServerProduct { get; set; }

    [SupplyParameterFromForm]
    public Product product { get; set; } = new Product();
    protected override Task OnInitializedAsync()
    {
        product ??= new();

        return base.OnInitializedAsync();
    }

    private async Task OnSubmit()
    {

        try
        {
            var response = await ServerProduct.CreateProduct(new gRPC.CreateProductRequest
            {
                Name = product.Name,
                Price = (float)product.Price
            }, null);


            Snackbar.Add("Product Added Successfully", Severity.Info);

        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }

    }
}
