
using BlazorGrpc.Model;
using BlazorGrpc.Service;
using Microsoft.AspNetCore.Components;

namespace BlazorGrpc.Components.Pages;

public partial class ProductAdd
{
    [Inject]
    protected ServerProductService? ServerProduct { get; set; }

    [SupplyParameterFromForm]
    public Product product { get; set; }

    protected string Message = string.Empty;

    protected bool IsSaved = false;
    protected override Task OnInitializedAsync()
    {
        product ??= new();

        return base.OnInitializedAsync();
    }

    private async Task OnSubmit()
    {
        var response = await ServerProduct.CreateProduct(new gRPC.CreateProductRequest
        {
            Name = product.Name,
            Price = (float)product.Price
        }, null);

        IsSaved = true;

        Message = "Product Added Successfully";
    }
}
