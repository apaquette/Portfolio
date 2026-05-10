using Bunit;
using Microsoft.AspNetCore.Components;

namespace Portfolio.Test;

/// <summary>
/// Base class for bUnit tests with NUnit
/// Provides TestContext lifecycle for Blazor component testing
/// </summary>
public abstract class BunitTestBase
{
    protected BunitContext? Context { get; private set; } = null!;

    [SetUp]
    public void Setup()
    {
        Context = new BunitContext();
    }

    [TearDown]
    public void TearDown()
    {
        Context?.Dispose();
    }

    protected IRenderedComponent<TComponent> RenderComponent<TComponent>(
        Action<ComponentParameterCollectionBuilder<TComponent>>? parameters = null)
        where TComponent : ComponentBase
    {
        return parameters is null ?
         Context!.Render<TComponent>()
         : Context!.Render<TComponent>(parameters);
    }
}
