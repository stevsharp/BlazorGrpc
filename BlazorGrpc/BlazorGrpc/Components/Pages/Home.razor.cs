using BlazorGrpc.Model;
using BlazorGrpc.Service;
using BlazorGrpc.Shared;
using BlazorGrpc.UIServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using MudBlazor;
namespace BlazorGrpc.Components.Pages;

public partial class Home
{
    [Inject]
    protected ServerProductService? ServerProduct { get; set; }

    [Inject]
    protected DialogUIService? Dialog { get; set; }

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

        (productListResponse ??= new List<ProductDto>()).Clear();

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

        StateHasChanged();

        loading = false;
    }


    private Task EditRow(ProductDto product)
    {
        NavigationManager.NavigateTo($"producteedit/{product.Id}");

        return Task.CompletedTask;
    }

    private async Task Delete(ProductDto product)
    {
        var contentText = $"Delete Confirmation {product.Name}";

        await Dialog.ShowDeleteConfirmationDialog(new gRPC.DeleteProductRequest
            {
                Id = product.Id
            }, "Delete ConfirmationTitle ", contentText,
            async () =>
            {
                await FillData();
            }
        );
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

    private async Task GoToPageAsync(int pageIndex)
    {
        await pagination.SetCurrentPageIndexAsync(pageIndex);
    }

    private string? PageButtonClass(int pageIndex)
        => pagination.CurrentPageIndex == pageIndex ? "current" : null;

    private string? AriaCurrentValue(int pageIndex)
        => pagination.CurrentPageIndex == pageIndex ? "page" : null;
}
