namespace Portfolio.Navigation;

public class NavigationService : INavigationService
{
    public IEnumerable<NavItem> GetNavItems() => new[]
    {
        new NavItem { Label = "Home", Route = "/" },
        new NavItem { Label = "Experience", Route = "/experience" }
    };
}
