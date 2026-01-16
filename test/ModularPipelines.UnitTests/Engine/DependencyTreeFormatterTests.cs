using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Engine;

/// <summary>
/// Tests for DependencyTreeFormatter Spectre.Console tree generation.
/// </summary>
public class DependencyTreeFormatterTests
{
    #region Test Modules

    private class ModuleA : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(true);
    }

    private class ModuleB : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(true);
    }

    private class ModuleC : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(true);
    }

    private class ModuleD : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(true);
    }

    private class SharedModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(true);
    }

    #endregion

    #region Helper Methods

    private static string RenderToString(Tree tree)
    {
        using var writer = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer),
            Ansi = AnsiSupport.No, // Disable ANSI for easier text assertions
        });
        console.Write(tree);
        return writer.ToString();
    }

    private static ModuleDependencyModel CreateModel<T>() where T : IModule, new()
    {
        return new ModuleDependencyModel(new T());
    }

    #endregion

    [Test]
    public async Task FormatTree_SingleModule_NoDependencies_ReturnsTreeWithSingleNode()
    {
        // Arrange
        var formatter = new DependencyTreeFormatter();
        var moduleA = CreateModel<ModuleA>();
        var roots = new[] { moduleA };

        // Act
        var tree = formatter.FormatTree(roots);
        var output = RenderToString(tree);

        // Assert
        await Assert.That(output).Contains("Module Dependencies");
        await Assert.That(output).Contains("ModuleA");
    }

    [Test]
    public async Task FormatTree_LinearChain_ReturnsTreeWithCorrectHierarchy()
    {
        // Arrange: A -> B -> C (A is root, B depends on A, C depends on B)
        var formatter = new DependencyTreeFormatter();
        var moduleA = CreateModel<ModuleA>();
        var moduleB = CreateModel<ModuleB>();
        var moduleC = CreateModel<ModuleC>();

        // Set up dependency chain: A is dependency for B, B is dependency for C
        moduleA.IsDependencyFor.Add(moduleB);
        moduleB.IsDependencyFor.Add(moduleC);

        var roots = new[] { moduleA };

        // Act
        var tree = formatter.FormatTree(roots);
        var output = RenderToString(tree);

        // Assert
        await Assert.That(output).Contains("ModuleA");
        await Assert.That(output).Contains("ModuleB");
        await Assert.That(output).Contains("ModuleC");
    }

    [Test]
    public async Task FormatTree_MultipleRoots_ReturnsTreeWithAllRoots()
    {
        // Arrange: Two independent modules
        var formatter = new DependencyTreeFormatter();
        var moduleA = CreateModel<ModuleA>();
        var moduleB = CreateModel<ModuleB>();

        var roots = new[] { moduleA, moduleB };

        // Act
        var tree = formatter.FormatTree(roots);
        var output = RenderToString(tree);

        // Assert
        await Assert.That(output).Contains("ModuleA");
        await Assert.That(output).Contains("ModuleB");
    }

    [Test]
    public async Task FormatTree_SharedModule_MarkedAsReference_OnSecondOccurrence()
    {
        // Arrange: A -> Shared, B -> Shared (diamond pattern)
        var formatter = new DependencyTreeFormatter();
        var moduleA = CreateModel<ModuleA>();
        var moduleB = CreateModel<ModuleB>();
        var shared = CreateModel<SharedModule>();

        moduleA.IsDependencyFor.Add(shared);
        moduleB.IsDependencyFor.Add(shared);

        var roots = new[] { moduleA, moduleB };

        // Act
        var tree = formatter.FormatTree(roots);
        var output = RenderToString(tree);

        // Assert
        await Assert.That(output).Contains("SharedModule");
        // Should have reference marker on second occurrence
        await Assert.That(output).Contains("(↑)");
    }

    [Test]
    public async Task FormatTree_DiamondDependency_ShowsReferenceMarkerForSharedLeaf()
    {
        // Arrange: A -> B -> D, A -> C -> D (diamond)
        var formatter = new DependencyTreeFormatter();
        var moduleA = CreateModel<ModuleA>();
        var moduleB = CreateModel<ModuleB>();
        var moduleC = CreateModel<ModuleC>();
        var moduleD = CreateModel<ModuleD>();

        moduleA.IsDependencyFor.Add(moduleB);
        moduleA.IsDependencyFor.Add(moduleC);
        moduleB.IsDependencyFor.Add(moduleD);
        moduleC.IsDependencyFor.Add(moduleD);

        var roots = new[] { moduleA };

        // Act
        var tree = formatter.FormatTree(roots);
        var output = RenderToString(tree);

        // Assert
        await Assert.That(output).Contains("ModuleA");
        await Assert.That(output).Contains("ModuleB");
        await Assert.That(output).Contains("ModuleC");
        await Assert.That(output).Contains("ModuleD");
        // D appears twice, second should have reference marker
        await Assert.That(output).Contains("(↑)");
    }

    [Test]
    public async Task FormatTree_EmptyCollection_OnlyContainsHeader()
    {
        // Arrange
        var formatter = new DependencyTreeFormatter();
        var roots = Array.Empty<ModuleDependencyModel>();

        // Act
        var tree = formatter.FormatTree(roots);
        var output = RenderToString(tree);

        // Assert - should have header but no child nodes (no tree connectors)
        await Assert.That(output).Contains("Module Dependencies");
        await Assert.That(output).DoesNotContain("├");
        await Assert.That(output).DoesNotContain("└");
    }

    [Test]
    public async Task FormatTree_AlreadyPrintedRoot_SkipsIt()
    {
        // Arrange: Same module appearing as root twice shouldn't duplicate
        var formatter = new DependencyTreeFormatter();
        var moduleA = CreateModel<ModuleA>();

        // Add same root twice
        var roots = new[] { moduleA, moduleA };

        // Act
        var tree = formatter.FormatTree(roots);
        var output = RenderToString(tree);

        // Assert - should only appear once
        var count = output.Split("ModuleA").Length - 1;
        await Assert.That(count).IsEqualTo(1);
    }
}
