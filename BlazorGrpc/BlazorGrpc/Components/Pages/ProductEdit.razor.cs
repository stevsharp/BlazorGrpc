
using BlazorGrpc.Model;
using BlazorGrpc.Service;

using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace BlazorGrpc.Components.Pages;

public partial class ProductEdit
{
    [Inject]
    protected ServerProductService? ServerProduct { get; set; }

    [Parameter]
    public int ProductId { get; set; }

    [SupplyParameterFromForm]
    public Product product { get; set; } = new();

    protected string StatusClass = string.Empty;
    protected override async Task OnInitializedAsync()
    {
        var response = await ServerProduct.GetProduct(new gRPC.GetProductRequest {  Id = ProductId }, null);

        if (response is not null )
        {
            product.Price = (decimal)response.Price;
            product.Id = response.Id;
            product.Name = response.Name;
        }

    }

    private async Task OnSubmit()
    {

        try
        {
            var response = await ServerProduct.UpdateProduct(new gRPC.UpdateProductRequest
            {
                Name = product.Name,
                Price = (float)product.Price,
                Id = product.Id
            }, null);


            Snackbar.Add("Product Updated Successfully", Severity.Info);

        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }

    }

}
