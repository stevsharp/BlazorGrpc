using BlazorGrpc.Model;
using BlazorGrpc.Service;
using BlazorGrpc.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

using MudBlazor;


namespace BlazorGrpc.Components.Pages;

public partial class Home
{
    [Inject]
    protected ServerProductService? ServerProduct { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    protected List<ProductDto> productListResponse { get; set; } = [];
    private IQueryable<ProductDto>? ProductIQueryable { get; set; }

    protected PaginationState pagination = new() { ItemsPerPage = 5 };

    protected bool loading = false;

    private QuickGrid<ProductDto> grid;

    private bool ShowMessageBox { get; set; } = false;

    private ProductDto CurrentProduct { get; set; } = default!;
    protected override async Task OnInitializedAsync()
    {
		try
		{
            await FillData();
        }
		catch (Exception ex)
		{
            Snackbar.Add(ex.Message, Severity.Error);
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
                productListResponse.Add(new ProductDto
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


    private  Task EditRow(ProductDto product )
    {
        NavigationManager.NavigateTo($"producteedit/{product.Id}");

        return Task.CompletedTask;
    }

    private Task Delete(ProductDto product)
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

    private async Task ShowConfirmDialog(Product product)
    {
        ShowMessageBox = true;
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
