using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Portfolio.Layout.Components;

public partial class ContactInfo : ComponentBase
{
    [Inject]
    public IJSRuntime? JS { get; set; }
    private readonly string[][] contacts = [
        ["mailto:alexandre.d.paquette@gmail.com", "envelope-fill", "Email"], 
        ["https://www.linkedin.com/in/apaquette0/", "linkedin", "LinkedIn"], 
        ["https://github.com/apaquette", "github", "GitHub"]
    ];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        
        if (firstRender && JS is not null)
        {
            await JS.InvokeVoidAsync("registerToolTips");
        }
    }
}