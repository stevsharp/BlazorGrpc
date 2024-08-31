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

    private Task Submit()
    {

        return Task.CompletedTask;
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
