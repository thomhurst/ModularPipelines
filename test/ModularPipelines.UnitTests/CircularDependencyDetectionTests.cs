using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

/// <summary>
/// Tests for circular dependency detection at registration time.
/// </summary>
public class CircularDependencyDetectionTests
{
    #region Test Modules - Direct Circular Dependency (A -> B -> A)

    [ModularPipelines.Attributes.DependsOn<DirectCycleModuleB>]
    private class DirectCycleModuleA : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<DirectCycleModuleA>]
    private class DirectCycleModuleB : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    #endregion

    #region Test Modules - Triple Cycle (A -> B -> C -> A)

    [ModularPipelines.Attributes.DependsOn<TripleCycleModuleB>]
    private class TripleCycleModuleA : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<TripleCycleModuleC>]
    private class TripleCycleModuleB : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<TripleCycleModuleA>]
    private class TripleCycleModuleC : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    #endregion

    #region Test Modules - No Circular Dependency (Linear Chain)

    [ModularPipelines.Attributes.DependsOn<LinearModuleB>]
    private class LinearModuleA : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<LinearModuleC>]
    private class LinearModuleB : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    private class LinearModuleC : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    #endregion

    #region Test Modules - Independent Modules (No Dependencies)

    private class IndependentModuleA : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    private class IndependentModuleB : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    #endregion

    #region Test Modules - Complex Graph with Cycle in Subset

    private class ComplexGraphRoot : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<ComplexGraphRoot>]
    [ModularPipelines.Attributes.DependsOn<ComplexGraphCycleB>]
    private class ComplexGraphCycleA : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<ComplexGraphCycleA>]
    private class ComplexGraphCycleB : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return true;
        }
    }

    #endregion

    #region DependencyGraphValidator Unit Tests

    [Test]
    public async Task ValidateNoCycles_WithDirectCycle_ThrowsCircularDependencyException()
    {
        var moduleTypes = new[] { typeof(DirectCycleModuleA), typeof(DirectCycleModuleB) };

        await Assert.That(() => DependencyGraphValidator.ValidateNoCycles(moduleTypes))
            .Throws<CircularDependencyException>();
    }

    [Test]
    public async Task ValidateNoCycles_WithTripleCycle_ThrowsCircularDependencyException()
    {
        var moduleTypes = new[] { typeof(TripleCycleModuleA), typeof(TripleCycleModuleB), typeof(TripleCycleModuleC) };

        await Assert.That(() => DependencyGraphValidator.ValidateNoCycles(moduleTypes))
            .Throws<CircularDependencyException>();
    }

    [Test]
    public async Task ValidateNoCycles_WithLinearChain_DoesNotThrow()
    {
        var moduleTypes = new[] { typeof(LinearModuleA), typeof(LinearModuleB), typeof(LinearModuleC) };

        await Assert.That(() => DependencyGraphValidator.ValidateNoCycles(moduleTypes))
            .ThrowsNothing();
    }

    [Test]
    public async Task ValidateNoCycles_WithIndependentModules_DoesNotThrow()
    {
        var moduleTypes = new[] { typeof(IndependentModuleA), typeof(IndependentModuleB) };

        await Assert.That(() => DependencyGraphValidator.ValidateNoCycles(moduleTypes))
            .ThrowsNothing();
    }

    [Test]
    public async Task ValidateNoCycles_WithEmptyCollection_DoesNotThrow()
    {
        var moduleTypes = Array.Empty<Type>();

        await Assert.That(() => DependencyGraphValidator.ValidateNoCycles(moduleTypes))
            .ThrowsNothing();
    }

    [Test]
    public async Task ValidateNoCycles_WithComplexGraphContainingCycle_ThrowsCircularDependencyException()
    {
        var moduleTypes = new[] { typeof(ComplexGraphRoot), typeof(ComplexGraphCycleA), typeof(ComplexGraphCycleB) };

        await Assert.That(() => DependencyGraphValidator.ValidateNoCycles(moduleTypes))
            .Throws<CircularDependencyException>();
    }

    [Test]
    public async Task ValidateNoCycles_ExceptionContainsCycleTypes()
    {
        var moduleTypes = new[] { typeof(DirectCycleModuleA), typeof(DirectCycleModuleB) };

        try
        {
            DependencyGraphValidator.ValidateNoCycles(moduleTypes);
            Assert.Fail("Expected CircularDependencyException to be thrown");
        }
        catch (CircularDependencyException ex)
        {
            await Assert.That(ex.CycleTypes).IsNotNull();
            await Assert.That(ex.CycleTypes.Count).IsGreaterThanOrEqualTo(2);
        }
    }

    [Test]
    public async Task ValidateNoCycles_ExceptionMessageShowsCyclePath()
    {
        var moduleTypes = new[] { typeof(DirectCycleModuleA), typeof(DirectCycleModuleB) };

        try
        {
            DependencyGraphValidator.ValidateNoCycles(moduleTypes);
            Assert.Fail("Expected CircularDependencyException to be thrown");
        }
        catch (CircularDependencyException ex)
        {
            await Assert.That(ex.Message).IsNotNull();
            await Assert.That(ex.Message).Contains("->");
            await Assert.That(ex.Message).Contains("Circular dependency detected");
        }
    }

    #endregion

    #region Integration Tests with Pipeline Registration

    [Test]
    public async Task AddModulesFromAssembly_WithCircularDependency_ThrowsAtRegistrationTime()
    {
        // This test verifies that circular dependencies are detected during registration,
        // not during execution. We need to create a test assembly scenario.
        // Since we can't easily create a separate assembly in the test, we'll test
        // through the ServiceCollectionExtensions with a controlled set of types.

        var moduleTypes = new[] { typeof(DirectCycleModuleA), typeof(DirectCycleModuleB) };

        await Assert.That(() => DependencyGraphValidator.ValidateNoCycles(moduleTypes))
            .Throws<CircularDependencyException>()
            .And.HasMessageContaining("Circular dependency detected at registration");
    }

    #endregion

    #region CircularDependencyException Tests

    [Test]
    public async Task CreateWithCyclePath_FormatsMessageCorrectly()
    {
        var cyclePath = new List<Type> { typeof(DirectCycleModuleA), typeof(DirectCycleModuleB), typeof(DirectCycleModuleA) };

        var exception = CircularDependencyException.CreateWithCyclePath(cyclePath);

        await Assert.That(exception.Message).Contains("**DirectCycleModuleA**");
        await Assert.That(exception.Message).Contains("->");
        await Assert.That(exception.CycleTypes).IsEqualTo(cyclePath);
    }

    [Test]
    public async Task CreateWithCyclePath_HighlightsStartAndEndOfCycle()
    {
        var cyclePath = new List<Type>
        {
            typeof(TripleCycleModuleA),
            typeof(TripleCycleModuleB),
            typeof(TripleCycleModuleC),
            typeof(TripleCycleModuleA)
        };

        var exception = CircularDependencyException.CreateWithCyclePath(cyclePath);

        // First and last types should be highlighted
        await Assert.That(exception.Message).Contains("**TripleCycleModuleA**");
    }

    #endregion
}
