using Microsoft.AspNetCore.Components;
using Portfolio.Navigation;

namespace Portfolio.Layout.Components;

public partial class Navbar : ComponentBase
{
    [Inject]
    private INavigationService NavigationService { get; set; } = default!;

    private IEnumerable<NavItem> _navItems = [];

    protected override void OnInitialized()
    {
        try
        {
            _navItems = NavigationService.GetNavItems();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading navigation items: {ex}");
            _navItems = [];
        }
    }

    private IEnumerable<NavItem> NavItems => _navItems;
}