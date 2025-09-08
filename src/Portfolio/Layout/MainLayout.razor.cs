using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Portfolio.Layout;

public partial class MainLayout
{
    [Inject]
    public IJSRuntime? JS { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && JS is not null)
        {
            await JS.InvokeVoidAsync("getHeaderIds");
        }
    }
}