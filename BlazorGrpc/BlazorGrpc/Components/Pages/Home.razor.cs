using BlazorGrpc.Model;
using BlazorGrpc.Service;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BlazorGrpc.Components.Pages;

public partial class Home
{
    [Inject]
    public ServerProductService ServerProduct { get; set; }

    private List<Product> productListResponse { get; set; } = [];

    private IQueryable<Product> productIQueryable { get; set; }

    PaginationState pagination = new PaginationState { ItemsPerPage = 10 };

    protected override async Task OnInitializedAsync()
    {
		try
		{
          
            var response =  await ServerProduct.ListProducts(null , null);

            if (response != null) {

                foreach (var item in response.Products)
                {
                    productListResponse.Add(new Product
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price  = (decimal)item.Price
                    });
                }

                productIQueryable = productListResponse.AsQueryable();

                pagination.TotalItemCountChanged += (sender, eventArgs) => StateHasChanged();
            }
        }
		catch (Exception ex)
		{
			throw;
		}

    }

    private async Task CreateNewRecord()
    {
        Random random = new();

        int randomInt = random.Next();

        await ServerProduct.CreateProduct(new gRPC.CreateProductRequest { Name = "Test " + randomInt.ToString(), Price = randomInt }, null);

        this.StateHasChanged();
    }

    private async Task GoToPageAsync(int pageIndex)
    {
        await pagination.SetCurrentPageIndexAsync(pageIndex);
    }

    private string? PageButtonClass(int pageIndex)
        => pagination.CurrentPageIndex == pageIndex ? "current" : null;

    private string? AriaCurrentValue(int pageIndex)
        => pagination.CurrentPageIndex == pageIndex ? "page" : null;
}
