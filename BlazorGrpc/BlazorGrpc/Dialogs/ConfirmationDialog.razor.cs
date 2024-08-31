using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorGrpc.Dialogs;

public partial class ConfirmationDialog
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public string? ContentText { get; set; }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
