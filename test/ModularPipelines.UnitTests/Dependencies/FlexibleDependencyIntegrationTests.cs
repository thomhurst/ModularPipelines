using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<DatabaseModuleA>()
            .AddModule<DatabaseModuleB>()
            .AddModule<NonDatabaseModule>()
            .AddModule<AfterDatabaseModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<NonDatabaseModule>()
            .AddModule<ModuleDependingOnNonExistentTag>()
            .ExecutePipelineAsync();

        // Assert - should succeed as no modules have the tag
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task DependsOnModulesWithTag_MultipleTagsOnModule_MatchesCorrectly()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithMultipleTags>()
            .AddModule<AfterSlowModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<InfrastructureModuleA>()
            .AddModule<InfrastructureModuleB>()
            .AddModule<BuildModule>()
            .AddModule<AfterInfrastructureModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<BuildModule>()
            .AddModule<ModuleDependingOnNonExistentCategory>()
            .ExecutePipelineAsync();

        // Assert - should succeed as no modules have the category
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    #endregion

    #region Attribute-Based Dependency Tests

    [Test]
    public async Task DependsOnModulesWithAttribute_WaitsForAttributedModules()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<CriticalModuleA>()
            .AddModule<CriticalModuleB>()
            .AddModule<NonCriticalModule>()
            .AddModule<AfterCriticalModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<DerivedCriticalModule>()
            .AddModule<AfterCriticalModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

        var order = GetExecutionOrder();
        var derivedIndex = order.IndexOf(nameof(DerivedCriticalModule));
        var afterCriticalIndex = order.IndexOf(nameof(AfterCriticalModule));

        await Assert.That(afterCriticalIndex).IsGreaterThan(derivedIndex);
    }

    [Test]
    public async Task DependsOnModulesWithAttribute_NoMatchingModules_StillSucceeds()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<NonCriticalModule>()
            .AddModule<AfterCriticalModule>()
            .ExecutePipelineAsync();

        // Assert - should succeed as no modules have the attribute
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    #endregion

    #region Override-Based Tags Tests

    [Test]
    public async Task ModuleWithOverrideTags_IsRecognizedByTagDependency()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithOverrideTags>()
            .AddModule<AfterDatabaseModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

        var order = GetExecutionOrder();
        var overrideIndex = order.IndexOf(nameof(ModuleWithOverrideTags));
        var afterDbIndex = order.IndexOf(nameof(AfterDatabaseModule));

        // AfterDatabaseModule depends on "database" tag, which is set via override
        await Assert.That(afterDbIndex).IsGreaterThan(overrideIndex);
    }

    [Test]
    public async Task ModuleWithOverrideCategory_IsRecognizedByCategoryDependency()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithOverrideCategory>()
            .AddModule<AfterInfrastructureModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

        var order = GetExecutionOrder();
        var overrideIndex = order.IndexOf(nameof(ModuleWithOverrideCategory));
        var afterInfraIndex = order.IndexOf(nameof(AfterInfrastructureModule));

        // AfterInfrastructureModule depends on "infrastructure" category, which is set via override
        await Assert.That(afterInfraIndex).IsGreaterThan(overrideIndex);
    }

    #endregion

    #region Registration-Time Tags Tests

    [Test]
    public async Task ModuleWithRegistrationTags_IsRecognizedByTagDependency()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services.AddModule<PlainModule>().WithTags("database");
            })
            .AddModule<AfterDatabaseModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

        var order = GetExecutionOrder();
        var plainIndex = order.IndexOf(nameof(PlainModule));
        var afterDbIndex = order.IndexOf(nameof(AfterDatabaseModule));

        // AfterDatabaseModule depends on "database" tag, which was set at registration time
        await Assert.That(afterDbIndex).IsGreaterThan(plainIndex);
    }

    [Test]
    public async Task ModuleWithRegistrationCategory_IsRecognizedByCategoryDependency()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services.AddModule<PlainModule>().WithCategory("infrastructure");
            })
            .AddModule<AfterInfrastructureModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

        var order = GetExecutionOrder();
        var plainIndex = order.IndexOf(nameof(PlainModule));
        var afterInfraIndex = order.IndexOf(nameof(AfterInfrastructureModule));

        // AfterInfrastructureModule depends on "infrastructure" category, which was set at registration time
        await Assert.That(afterInfraIndex).IsGreaterThan(plainIndex);
    }

    [Test]
    public async Task ModuleWithBothAttributeAndRegistrationTags_MergesTags()
    {
        // Arrange & Act - DatabaseModuleA has "database" tag via attribute, add "slow" via registration
        var result = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services.AddModule<DatabaseModuleA>().WithTags("slow");
            })
            .AddModule<AfterSlowModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

        var order = GetExecutionOrder();
        var dbAIndex = order.IndexOf(nameof(DatabaseModuleA));
        var afterSlowIndex = order.IndexOf(nameof(AfterSlowModule));

        // AfterSlowModule depends on "slow" tag, which was added at registration time
        await Assert.That(afterSlowIndex).IsGreaterThan(dbAIndex);
    }

    #endregion

    #region Combined Dependency Tests

    [Test]
    public async Task CombinedDependencies_ModuleWithMultipleFlexibleDependencies()
    {
        // Arrange & Act
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<DatabaseModuleA>()
            .AddModule<InfrastructureModuleA>()
            .AddModule<CriticalModuleA>()
            .AddModule<ModuleWithMultipleFlexibleDependencies>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<DatabaseModuleA>()
            .AddModule<AfterDatabaseModuleWithPhase1Tag>()
            .AddModule<AfterPhase1Module>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

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
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(DatabaseModuleA));
            return "DatabaseA";
        }
    }

    [ModuleTag("database")]
    private class DatabaseModuleB : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(DatabaseModuleB));
            return "DatabaseB";
        }
    }

    [ModuleTag("other")]
    private class NonDatabaseModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(NonDatabaseModule));
            return "NonDatabase";
        }
    }

    [DependsOnModulesWithTag("database")]
    private class AfterDatabaseModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterDatabaseModule));
            return "AfterDatabase";
        }
    }

    [DependsOnModulesWithTag("nonexistent")]
    private class ModuleDependingOnNonExistentTag : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
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
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(ModuleWithMultipleTags));
            return "MultipleTags";
        }
    }

    [DependsOnModulesWithTag("slow")]
    private class AfterSlowModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
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
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(InfrastructureModuleA));
            return "InfrastructureA";
        }
    }

    [ModuleCategory("infrastructure")]
    private class InfrastructureModuleB : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(InfrastructureModuleB));
            return "InfrastructureB";
        }
    }

    [ModuleCategory("build")]
    private class BuildModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(BuildModule));
            return "Build";
        }
    }

    [DependsOnModulesInCategory("infrastructure")]
    private class AfterInfrastructureModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterInfrastructureModule));
            return "AfterInfrastructure";
        }
    }

    [DependsOnModulesInCategory("nonexistent")]
    private class ModuleDependingOnNonExistentCategory : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
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
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(CriticalModuleA));
            return "CriticalA";
        }
    }

    [Critical]
    private class CriticalModuleB : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(CriticalModuleB));
            return "CriticalB";
        }
    }

    private class NonCriticalModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(NonCriticalModule));
            return "NonCritical";
        }
    }

    [Critical]
    private class BaseCriticalModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(BaseCriticalModule));
            return "BaseCritical";
        }
    }

    // Inherits Critical attribute from base class
    private class DerivedCriticalModule : BaseCriticalModule
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(DerivedCriticalModule));
            return "DerivedCritical";
        }
    }

    [DependsOnModulesWithAttribute<CriticalAttribute>]
    private class AfterCriticalModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterCriticalModule));
            return "AfterCritical";
        }
    }

    #endregion

    #region Test Modules - Override-Based Tags/Category

    private class ModuleWithOverrideTags : Module<string>
    {
        public override IReadOnlySet<string> Tags => new HashSet<string> { "database", "override-tag" };

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(ModuleWithOverrideTags));
            return "OverrideTags";
        }
    }

    private class ModuleWithOverrideCategory : Module<string>
    {
        public override string? Category => "infrastructure";

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(ModuleWithOverrideCategory));
            return "OverrideCategory";
        }
    }

    #endregion

    #region Test Modules - Registration-Time Tags

    private class PlainModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(PlainModule));
            return "Plain";
        }
    }

    #endregion

    #region Test Modules - Combined Dependencies

    [DependsOnModulesWithTag("database")]
    [DependsOnModulesInCategory("infrastructure")]
    [DependsOnModulesWithAttribute<CriticalAttribute>]
    private class ModuleWithMultipleFlexibleDependencies : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
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
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterDatabaseModuleWithPhase1Tag));
            return "AfterDbWithPhase1";
        }
    }

    [DependsOnModulesWithTag("phase1")]
    private class AfterPhase1Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken token)
        {
            await Task.Yield();
            RecordExecution(nameof(AfterPhase1Module));
            return "AfterPhase1";
        }
    }

    #endregion
}
