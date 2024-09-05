using BlazorGrpc.Dialogs;
using BlazorGrpc.gRPC;
using BlazorGrpc.Service;
using MudBlazor;

namespace BlazorGrpc.UIServices
{
    public class DialogUIService
    {
        private readonly IDialogService _dialogService;
        private readonly ISnackbar _snackbar;
        protected readonly ServerProductService _serverProduct;
        public DialogUIService(IDialogService dialogService, ISnackbar snackbar, ServerProductService ServerProduct)
        {
            _dialogService = dialogService;
            _snackbar = snackbar;
            _serverProduct = ServerProduct;
        }

        public async Task ShowDeleteConfirmationDialog(DeleteProductRequest dto,
        string title, string contentText,
        Func<Task> onConfirm, Func<Task>? onCancel = null)
        {
            var parameters = new DialogParameters
            {
                { nameof(DeleteConfirmation.ContentText), contentText },
                { nameof(DeleteConfirmation.Command), dto }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = _dialogService.Show<DeleteConfirmation>(title, parameters, options);

            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await _serverProduct.DeleteProduct(dto,null);

                _snackbar.Add($"{"DeleteSuccess"}", Severity.Info);

                await onConfirm();
            }
            else if (onCancel != null)
            {
                await onCancel();
            }
        }
    }
}
