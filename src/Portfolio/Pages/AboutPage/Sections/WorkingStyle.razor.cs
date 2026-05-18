using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Portfolio.Pages.AboutPage.Sections;

[ExcludeFromCodeCoverage]
public partial class WorkingStyle : ComponentBase
{
    protected readonly WorkingStyleData[] WorkingStyles = [
        new WorkingStyleData
        {
            Title = "Ownership",
            Subtitle = "I see problems through to resolution.",
            Highlights =
            [
                "Root-cause analysis",
                "End-to-end accountability",
                "Initiative beyond assigned work"
            ]
        },

        new WorkingStyleData
        {
            Title = "Engineering Quality",
            Subtitle = "I build software with long-term maintainability in mind.",
            Highlights =
            [
                "Test Driven Development",
                "Automated testing",
                "Thoughtful architecture"
            ]
        },

        new WorkingStyleData
        {
            Title = "Pragmatic Problem Solving",
            Subtitle = "I solve the right problems without overengineering.",
            Highlights =
            [
                "Risk reduction",
                "Secure development",
                "Practical trade-offs"
            ]
        },

        new WorkingStyleData
        {
            Title = "Collaboration & Leadership",
            Subtitle = "I help teams improve through action and initiative.",
            Highlights =
            [
                "Process improvement",
                "Mentoring",
                "Engineering advocacy"
            ]
        },

        new WorkingStyleData
        {
            Title = "Continuous Learning",
            Subtitle = "I learn by building, experimenting, and exploring.",
            Highlights =
            [
                "Linux & NixOS",
                "Self-hosted tooling",
                "Workflow experimentation"
            ]
        },

        new WorkingStyleData
        {
            Title = "Calm Under Pressure",
            Subtitle = "I stay methodical when reliability matters most.",
            Highlights =
            [
                "Production troubleshooting",
                "Deployment support",
                "Incident investigation"
            ]
        },
    ];
}

public sealed record WorkingStyleData
{
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public string[] Highlights { get; set; } = [];
}

