using BlazorGrpc.Model;
using BlazorGrpc.Service;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;

namespace BlazorGrpc.Components.Pages;

public partial class Home
{
    [Inject]
    protected ServerProductService? ServerProduct { get; set; }
    protected List<Product> productListResponse { get; set; } = [];
    private IQueryable<Product>? ProductIQueryable { get; set; }

    protected PaginationState pagination = new() { ItemsPerPage = 5 };

    protected bool loading = false;

    private QuickGrid<Product> grid;

    private bool ShowMessageBox { get; set; } = false;

    private Product CurrentProduct { get; set; } = default!;
    protected override async Task OnInitializedAsync()
    {
		try
		{
            await FillData();
        }
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}

    }

    private async ValueTask FillData()
    {
        loading = true;

        if (productListResponse.Any())
            productListResponse.Clear();

        var response = await ServerProduct.ListProducts(null, null);

        if (response != null)
        {

            foreach (var item in response.Products)
            {
                productListResponse.Add(new Product
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = (decimal)item.Price
                });
            }

            ProductIQueryable = productListResponse.AsQueryable();

            pagination.TotalItemCountChanged += (sender, eventArgs) => StateHasChanged();
        }

        
        loading = false;
    }


    private async Task EditRow(Product product )
    {

        if (product is null)
            return;

        loading = true;

        var response = await ServerProduct.UpdateProduct(new gRPC.UpdateProductRequest
            {
                Id = product.Id,
                Name = product.Name,
            }, null);


        await FillData();
    }

    private Task Delete(Product product)
    {
        ShowMessageBox = true;

        CurrentProduct = product;

        return Task.CompletedTask;
    }


    private void OnRowEdit(ChangeEventArgs e)
    {
        // Handle input change events
        var input = e.Value.ToString();
        // Update the model based on the input
    }

    private async Task CreateNewRecord()
    {
        Random random = new();

        int randomInt = random.Next();

        await ServerProduct.CreateProduct(new gRPC.CreateProductRequest { Name = "Test " + randomInt.ToString(), Price = randomInt }, null);

        await FillData();

        this.StateHasChanged();
    }


    private async Task ShowConfirmDialog(Product product)
    {
        try
        {
            ShowMessageBox = true;

            
        }
        catch (Exception ex)
        {
            var m = ex.Message;
        }


    }

    private async Task OnConfirmed(bool isConfirmed)
    {
        if (isConfirmed)
        {
            if (CurrentProduct is null)
                return;

            loading = true;

            await ServerProduct.DeleteProduct(new gRPC.DeleteProductRequest
            {
                Id = CurrentProduct.Id
            }, null);


            await FillData();

        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }

        this.ShowMessageBox = false;
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
