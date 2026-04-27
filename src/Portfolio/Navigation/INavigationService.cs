namespace Portfolio.Navigation;

public interface INavigationService
{
    IEnumerable<NavItem> GetNavItems();
}
