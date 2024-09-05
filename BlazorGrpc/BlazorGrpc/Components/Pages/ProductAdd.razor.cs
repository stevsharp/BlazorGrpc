
using BlazorGrpc.Model;
using BlazorGrpc.Service;
using BlazorGrpc.Shared;

using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace BlazorGrpc.Components.Pages;

public partial class ProductAdd
{
    [Inject]
    protected ServerProductService? ServerProduct { get; set; }

    [SupplyParameterFromForm]
    public ProductDto product { get; set; }
    protected override Task OnInitializedAsync()
    {
        product = new ProductDto
        {
            Id = 0,
            Name = string.Empty,
            Price = 0m
        };

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

            product = new ProductDto
            {
                Id = 0,
                Name = string.Empty,
                Price = 0m
            };

        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }

    }
}
