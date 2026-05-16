using Microsoft.AspNetCore.Components;
using Models;

namespace Portfolio.Layout.Components;

public partial class Navbar : ComponentBase
{
    private static readonly IEnumerable<NavItem> NavItems = [
        new NavItem { Label = "Home", Route = "/" },
        new NavItem { Label = "Experience", Route = "/experience" },
        new NavItem { Label = "Projects", Route = "/projects" },
        new NavItem { Label = "About", Route = "/about" },
    ];
}