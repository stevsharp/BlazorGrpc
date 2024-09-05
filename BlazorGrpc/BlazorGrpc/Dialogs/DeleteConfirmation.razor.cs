using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorGrpc.Dialogs;

public partial class DeleteConfirmation
{
    [CascadingParameter] 
    private MudDialogInstance MudDialog { get; set; } = default!;

    [EditorRequired]
    [Parameter] 
    public string? ContentText { get; set; }

    [EditorRequired]
    [Parameter] 
    public object Command { get; set; } = default!;
    private Task Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));

        return Task.CompletedTask;
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
