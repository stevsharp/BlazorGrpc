using Microsoft.AspNetCore.Components;

namespace BlazorGrpc.Components
{
    public partial class ConfirmDialog
    {
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public EventCallback<bool> OnConfirmed { get; set; }

        [Parameter]
        public EventCallback<bool> OnCancel { get; set; }


        [Parameter]
        public bool Show { get; set; }

        private async Task Confirm(bool isConfirmed)
        {
            await OnConfirmed.InvokeAsync(isConfirmed);
        }

        private async Task Cancel(bool isConfirmed)
        {
            await OnCancel.InvokeAsync(isConfirmed);
        }
    }
}
