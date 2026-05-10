using Models;

namespace Portfolio.Test.Mocks;

public static class ProjectBuilder
{
    public static Project ValidProject()
    {
        return new Project(new DateOnly(2022, 6, 1))
        {
            Title = "Test Project",
            Description = "A test project",
            Link = "https://example.com",
            ImageSource = "test.png",
            TechStack = new() { "C#", "Blazor", "Azure" }
        };
    }

    public static Project MinimalProject()
    {
        return new Project(new DateOnly(2022, 6, 1));
    }

    public static Project ProjectWithoutTitle()
    {
        return new Project(new DateOnly(2022, 6, 1))
        {
            Title = null,
            Description = "No title project"
        };
    }

    public static Project ProjectWithoutDescription()
    {
        return new Project(new DateOnly(2022, 6, 1))
        {
            Title = "No description project",
            Description = null
        };
    }

    public static Project OldProject()
    {
        return new Project(new DateOnly(2015, 1, 1))
        {
            Title = "Old Project",
            Description = "Completed long ago"
        };
    }

    public static Project RecentProject()
    {
        return new Project(new DateOnly(2025, 12, 31))
        {
            Title = "Recent Project",
            Description = "Just completed"
        };
    }
}

public static class ExperienceBuilder
{
    public static Experience ValidExperience()
    {
        return new Experience(new DateTime(2020, 1, 1))
        {
            Title = "Senior Developer",
            Company = "Tech Corp",
            Description = "Built amazing things",
            Location = "Remote",
            EmployerSite = "techcorp.com",
            Skills = new() { "C#", "Azure", "Leadership" }
        };
    }

    public static Experience MinimalExperience()
    {
        return new Experience(new DateTime(2020, 1, 1));
    }

    public static Experience OngoingExperience()
    {
        return new Experience(new DateTime(2023, 1, 1))
        {
            Title = "Current Role",
            Company = "Active Company",
            JobEnd = null
        };
    }

    public static Experience ShortExperience()
    {
        return new Experience(new DateTime(2024, 1, 1))
        {
            Title = "Brief Role",
            Company = "Short Lived",
            JobEnd = new DateTime(2024, 1, 15)
        };
    }

    public static Experience LongExperience()
    {
        return new Experience(new DateTime(2015, 1, 1))
        {
            Title = "Long Term Role",
            Company = "Long Company",
            JobEnd = new DateTime(2023, 12, 31)
        };
    }
}

public static class DegreeBuilder
{
    public static Degree ValidDegree()
    {
        return new Degree(new DateOnly(2020, 6, 1))
        {
            Diploma = "BSc (Honours)",
            Institution = "University of Testing",
            Logo = "logo.png",
            Website = "university.com"
        };
    }

    public static Degree MinimalDegree()
    {
        return new Degree(new DateOnly(2020, 6, 1));
    }

    public static Degree RecentDegree()
    {
        return new Degree(new DateOnly(2024, 6, 1))
        {
            Diploma = "Masters",
            Institution = "Advanced University"
        };
    }

    public static Degree OldDegree()
    {
        return new Degree(new DateOnly(2010, 6, 1))
        {
            Diploma = "BSc",
            Institution = "Early University"
        };
    }
}

public static class CertificationBuilder
{
    public static Certification ValidCertification()
    {
        return new Certification
        {
            Name = "AWS Certified Solutions Architect",
            EarnedOn = new DateOnly(2023, 3, 15),
            IssuedBy = "Amazon Web Services",
            Icon = "aws.png",
            Link = "https://aws.amazon.com/certification"
        };
    }

    public static Certification MinimalCertification()
    {
        return new Certification
        {
            EarnedOn = new DateOnly(2023, 3, 15)
        };
    }

    public static Certification RecentCertification()
    {
        return new Certification
        {
            Name = "Latest Cert",
            EarnedOn = new DateOnly(2025, 12, 1),
            IssuedBy = "Recent Issuer"
        };
    }

    public static Certification OldCertification()
    {
        return new Certification
        {
            Name = "Old Cert",
            EarnedOn = new DateOnly(2018, 1, 1),
            IssuedBy = "Old Issuer"
        };
    }
}
