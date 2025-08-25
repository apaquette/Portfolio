namespace Models;

public class Project
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> TechStack { get; set; }
    public string Link { get; set; }
    public string ImageSource { get; set; }
    public Project(string title, string desc, List<string> stack, string link, string imageSource)
    {
        Title = title;
        Description = desc;
        TechStack = stack;
        Link = link;
        ImageSource = imageSource;
    }
}