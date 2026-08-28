using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Dependencies;

/// <summary>
/// Integration tests for the flexible dependency system.
/// These tests verify that modules with tag/category/attribute dependencies
/// actually execute in the correct order during pipeline execution.
/// </summary>
[TUnit.Core.NotInParallel(nameof(FlexibleDependencyIntegrationTests))]
public class FlexibleDependencyIntegrationTests : TestBase
{
    // Use a thread-safe queue to preserve insertion order
    private static readonly ConcurrentQueue<string> ExecutionOrderQueue = new();

    [Before(Test)]
    public void Setup()
    {
        ExecutionOrderQueue.Clear();
    }

    /// <summary>
    /// Gets the execution order as an indexed list.
    /// </summary>
    private static List<string> GetExecutionOrder() => ExecutionOrderQueue.ToList();

    /// <summary>
    /// Records module execution in thread-safe manner.
    /// </summary>
    private static void RecordExecution(string moduleName) => ExecutionOrderQueue.Enqueue(moduleName);

    #region Tag-Based Dependency Tests

    [Test]
    public async Task DependsOnModulesWithTag_WaitsForTaggedModules()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<DatabaseModuleA>()
            .AddModule<DatabaseModuleB>()
            .AddModule<NonDatabaseModule>()
            .AddModule<AfterDatabaseModule>()
            .RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);

        var order = GetExecutionOrder();
        var afterDbIndex = order.IndexOf(nameof(AfterDatabaseModule));
        var dbAIndex = order.IndexOf(nameof(DatabaseModuleA));
        var dbBIndex = order.IndexOf(nameof(DatabaseModuleB));

        // AfterDatabaseModule should execute after both database modules
        await Assert.That(afterDbIndex).IsGreaterThan(dbAIndex);
        await Assert.That(afterDbIndex).IsGreaterThan(dbBIndex);
    }

    [Test]
    public async Task DependsOnModulesWithTag_NoMatchingModules_StillSucceeds()
    {
        // Arrange & Act - AfterDatabaseModule depends on "database" tag but no modules have it
        var result = await TestPipelineBuilder.Create()
            .AddModule<NonDatabaseModule>()
            .AddModule<ModuleDependingOnNonExistentTag>()
            .RunAsync();

        // Assert - should succeed as no modules have the tag
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task DependsOnModulesWithTag_MultipleTagsOnModule_MatchesCorrectly()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<ModuleWithMultipleTags>()
            .AddModule<AfterSlowModule>()
            .RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);

        var order = GetExecutionOrder();
        var multiTagIndex = order.IndexOf(nameof(ModuleWithMultipleTags));
        var afterSlowIndex = order.IndexOf(nameof(AfterSlowModule));

        await Assert.That(afterSlowIndex).IsGreaterThan(multiTagIndex);
    }

    #endregion

    #region Category-Based Dependency Tests

    [Test]
    public async Task DependsOnModulesInCategory_WaitsForCategorizedModules()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<InfrastructureModuleA>()
            .AddModule<InfrastructureModuleB>()
            .AddModule<BuildModule>()
            .AddModule<AfterInfrastructureModule>()
            .RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);

        var order = GetExecutionOrder();
        var afterInfraIndex = order.IndexOf(nameof(AfterInfrastructureModule));
        var infraAIndex = order.IndexOf(nameof(InfrastructureModuleA));
        var infraBIndex = order.IndexOf(nameof(InfrastructureModuleB));

        // AfterInfrastructureModule should execute after both infrastructure modules
        await Assert.That(afterInfraIndex).IsGreaterThan(infraAIndex);
        await Assert.That(afterInfraIndex).IsGreaterThan(infraBIndex);
    }

    [Test]
    public async Task DependsOnModulesInCategory_NoMatchingModules_StillSucceeds()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<BuildModule>()
            .AddModule<ModuleDependingOnNonExistentCategory>()
            .RunAsync();

        // Assert - should succeed as no modules have the category
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    #endregion

    #region Attribute-Based Dependency Tests

    [Test]
    public async Task DependsOnModulesWithAttribute_WaitsForAttributedModules()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<CriticalModuleA>()
            .AddModule<CriticalModuleB>()
            .AddModule<NonCriticalModule>()
            .AddModule<AfterCriticalModule>()
            .RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);

        var order = GetExecutionOrder();
        var afterCriticalIndex = order.IndexOf(nameof(AfterCriticalModule));
        var criticalAIndex = order.IndexOf(nameof(CriticalModuleA));
        var criticalBIndex = order.IndexOf(nameof(CriticalModuleB));

        // AfterCriticalModule should execute after both critical modules
        await Assert.That(afterCriticalIndex).IsGreaterThan(criticalAIndex);
        await Assert.That(afterCriticalIndex).IsGreaterThan(criticalBIndex);
    }

    [Test]
    public async Task DependsOnModulesWithAttribute_InheritedAttribute_IsRecognized()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<DerivedCriticalModule>()
            .AddModule<AfterCriticalModule>()
            .RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);

        var order = GetExecutionOrder();
        var derivedIndex = order.IndexOf(nameof(DerivedCriticalModule));
        var afterCriticalIndex = order.IndexOf(nameof(AfterCriticalModule));

        await Assert.That(afterCriticalIndex).IsGreaterThan(derivedIndex);
    }

    [Test]
    public async Task DependsOnModulesWithAttribute_NoMatchingModules_StillSucceeds()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<NonCriticalModule>()
            .AddModule<AfterCriticalModule>()
            .RunAsync();

        // Assert - should succeed as no modules have the attribute
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    #endregion

    #region Override-Based Tags Tests

    [Test]
    public async Task ModuleWithConfiguredTags_IsRecognizedByTagDependency()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<ModuleWithConfiguredTags>()
            .AddModule<AfterDatabaseModule>()
            .RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);

        var order = GetExecutionOrder();
        var configuredIndex = order.IndexOf(nameof(ModuleWithConfiguredTags));
        var afterDbIndex = order.IndexOf(nameof(AfterDatabaseModule));

        await Assert.That(afterDbIndex).IsGreaterThan(configuredIndex);
    }

    [Test]
    public async Task ModuleWithConfiguredCategory_IsRecognizedByCategoryDependency()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<ModuleWithConfiguredCategory>()
            .AddModule<AfterInfrastructureModule>()
            .RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);

        var order = GetExecutionOrder();
        var configuredIndex = order.IndexOf(nameof(ModuleWithConfiguredCategory));
        var afterInfraIndex = order.IndexOf(nameof(AfterInfrastructureModule));

        await Assert.That(afterInfraIndex).IsGreaterThan(configuredIndex);
    }

    #endregion

    #region Combined Dependency Tests

    [Test]
    public async Task CombinedDependencies_ModuleWithMultipleFlexibleDependencies()
    {
        // Arrange & Act
        var result = await TestPipelineBuilder.Create()
            .AddModule<DatabaseModuleA>()
            .AddModule<InfrastructureModuleA>()
            .AddModule<CriticalModuleA>()
            .AddModule<ModuleWithMultipleFlexibleDependencies>()
            .RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);

        var order = GetExecutionOrder();
        var combinedIndex = order.IndexOf(nameof(ModuleWithMultipleFlexibleDependencies));
        var dbAIndex = order.IndexOf(nameof(DatabaseModuleA));
        var infraAIndex = order.IndexOf(nameof(InfrastructureModuleA));
        var criticalAIndex = order.IndexOf(nameof(CriticalModuleA));

        // ModuleWithMultipleFlexibleDependencies should execute after all its dependencies
        await Assert.That(combinedIndex).IsGreaterThan(dbAIndex);
        await Assert.That(combinedIndex).IsGreaterThan(infraAIndex);
        await Assert.That(combinedIndex).IsGreaterThan(criticalAIndex);
    }

    [Test]
    public async Task ChainedFlexibleDependencies_ExecuteInCorrectOrder()
    {
        // Arrange & Act
        // Chain: DatabaseModuleA (tag: database) -> AfterDatabaseModule (depends on database tag, tag: phase1)
        //        -> AfterPhase1Module (depends on phase1 tag)
        var result = await TestPipelineBuilder.Create()
            .AddModule<DatabaseModuleA>()
            .AddModule<AfterDatabaseModuleWithPhase1Tag>()
            .AddModule<AfterPhase1Module>()
            .RunAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);

        var order = GetExecutionOrder();
        var dbAIndex = order.IndexOf(nameof(DatabaseModuleA));
        var afterDbIndex = order.IndexOf(nameof(AfterDatabaseModuleWithPhase1Tag));
        var afterPhase1Index = order.IndexOf(nameof(AfterPhase1Module));

        await Assert.That(afterDbIndex).IsGreaterThan(dbAIndex);
        await Assert.That(afterPhase1Index).IsGreaterThan(afterDbIndex);
    }

    #endregion

    #region Test Attributes

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    private sealed class CriticalAttribute : Attribute { }

    #endregion

    #region Test Modules - Tag-Based

    [ModuleTag("database")]
    private class DatabaseModuleA : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(DatabaseModuleA));
            return "DatabaseA";
        }
    }

    [ModuleTag("database")]
    private class DatabaseModuleB : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(DatabaseModuleB));
            return "DatabaseB";
        }
    }

    [ModuleTag("other")]
    private class NonDatabaseModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(NonDatabaseModule));
            return "NonDatabase";
        }
    }

    [DependsOnModulesWithTag("database")]
    private class AfterDatabaseModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterDatabaseModule));
            return "AfterDatabase";
        }
    }

    [DependsOnModulesWithTag("nonexistent")]
    private class ModuleDependingOnNonExistentTag : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(ModuleDependingOnNonExistentTag));
            return "DependsOnNonExistent";
        }
    }

    [ModuleTag("database")]
    [ModuleTag("slow")]
    [ModuleTag("critical")]
    private class ModuleWithMultipleTags : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(ModuleWithMultipleTags));
            return "MultipleTags";
        }
    }

    [DependsOnModulesWithTag("slow")]
    private class AfterSlowModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterSlowModule));
            return "AfterSlow";
        }
    }

    #endregion

    #region Test Modules - Category-Based

    [ModuleCategory("infrastructure")]
    private class InfrastructureModuleA : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(InfrastructureModuleA));
            return "InfrastructureA";
        }
    }

    [ModuleCategory("infrastructure")]
    private class InfrastructureModuleB : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(InfrastructureModuleB));
            return "InfrastructureB";
        }
    }

    [ModuleCategory("build")]
    private class BuildModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(BuildModule));
            return "Build";
        }
    }

    [DependsOnModulesInCategory("infrastructure")]
    private class AfterInfrastructureModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterInfrastructureModule));
            return "AfterInfrastructure";
        }
    }

    [DependsOnModulesInCategory("nonexistent")]
    private class ModuleDependingOnNonExistentCategory : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(ModuleDependingOnNonExistentCategory));
            return "DependsOnNonExistentCategory";
        }
    }

    #endregion

    #region Test Modules - Attribute-Based

    [Critical]
    private class CriticalModuleA : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(CriticalModuleA));
            return "CriticalA";
        }
    }

    [Critical]
    private class CriticalModuleB : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(CriticalModuleB));
            return "CriticalB";
        }
    }

    private class NonCriticalModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(NonCriticalModule));
            return "NonCritical";
        }
    }

    [Critical]
    private class BaseCriticalModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(BaseCriticalModule));
            return "BaseCritical";
        }
    }

    // Inherits Critical attribute from base class
    private class DerivedCriticalModule : BaseCriticalModule
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(DerivedCriticalModule));
            return "DerivedCritical";
        }
    }

    [DependsOnModulesWithAttribute<CriticalAttribute>]
    private class AfterCriticalModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterCriticalModule));
            return "AfterCritical";
        }
    }

    #endregion

    #region Test Modules - Configured Tags/Category

    private class ModuleWithConfiguredTags : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTags("database", "configured-tag")
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(ModuleWithConfiguredTags));
            return "ConfiguredTags";
        }
    }

    private class ModuleWithConfiguredCategory : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithCategory("infrastructure")
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(ModuleWithConfiguredCategory));
            return "ConfiguredCategory";
        }
    }

    #endregion

    #region Test Modules - Combined Dependencies

    [DependsOnModulesWithTag("database")]
    [DependsOnModulesInCategory("infrastructure")]
    [DependsOnModulesWithAttribute<CriticalAttribute>]
    private class ModuleWithMultipleFlexibleDependencies : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(ModuleWithMultipleFlexibleDependencies));
            return "MultipleFlexibleDeps";
        }
    }

    [ModuleTag("database")]
    [DependsOnModulesWithTag("database")]
    [ModuleTag("phase1")]
    private class AfterDatabaseModuleWithPhase1Tag : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterDatabaseModuleWithPhase1Tag));
            return "AfterDbWithPhase1";
        }
    }

    [DependsOnModulesWithTag("phase1")]
    private class AfterPhase1Module : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterPhase1Module));
            return "AfterPhase1";
        }
    }

    #endregion
}
