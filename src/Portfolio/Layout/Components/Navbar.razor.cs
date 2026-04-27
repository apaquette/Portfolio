using Microsoft.AspNetCore.Components;
using Portfolio.Navigation;

namespace Portfolio.Layout.Components;

public partial class Navbar : ComponentBase
{
    [Inject]
    private INavigationService NavigationService { get; set; } = default!;

    private IEnumerable<NavItem> NavItems => NavigationService.GetNavItems();
}