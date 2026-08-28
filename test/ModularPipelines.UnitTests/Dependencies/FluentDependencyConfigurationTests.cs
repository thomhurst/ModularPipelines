using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Dependencies;

/// <summary>
/// Tests for dependencies declared through module configuration.
/// </summary>
public class FluentDependencyConfigurationTests : TestBase
{
    #region Helper Modules

    /// <summary>
    /// A basic module with no dependencies.
    /// </summary>
    private class BaseModule : SimpleTestModule<string>
    {
        protected override string Result => "base";
    }

    /// <summary>
    /// A module that others can optionally depend on.
    /// </summary>
    private class OptionalDependencyModule : SimpleTestModule<string>
    {
        protected override string Result => "optional";
    }

    /// <summary>
    /// A module for testing conditional dependencies.
    /// </summary>
    private class ConditionalModule : SimpleTestModule<string>
    {
        protected override string Result => "conditional";
    }

    /// <summary>
    /// A module that declares a required dependency programmatically.
    /// </summary>
    private class ModuleWithProgrammaticDependency : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<BaseModule>()
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "programmatic";
        }
    }

    /// <summary>
    /// A module that declares an optional dependency programmatically.
    /// </summary>
    private class ModuleWithOptionalDependency : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOnOptional<OptionalDependencyModule>()
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "optional-dep";
        }
    }

    /// <summary>
    /// A module that declares a conditional dependency that is active.
    /// </summary>
    private class ModuleWithActiveConditionalDependency : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOnIf<ConditionalModule>(true)
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "conditional-active";
        }
    }

    /// <summary>
    /// A module that declares a conditional dependency that is inactive.
    /// </summary>
    private class ModuleWithInactiveConditionalDependency : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOnIf<ConditionalModule>(false)
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "conditional-inactive";
        }
    }

    /// <summary>
    /// A module that combines both attribute and programmatic dependencies.
    /// </summary>
    [ModularPipelines.Attributes.DependsOn<BaseModule>]
    private class ModuleWithBothAttributeAndProgrammaticDependencies : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOnOptional<OptionalDependencyModule>()
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "combined";
        }
    }

    /// <summary>
    /// A module that chains multiple dependency declarations.
    /// </summary>
    private class ModuleWithChainedDependencies : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<BaseModule>()
            .DependsOnOptional<OptionalDependencyModule>()
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "chained";
        }
    }

    /// <summary>
    /// A module that depends on an unregistered required dependency (should fail).
    /// </summary>
    private class ModuleWithMissingRequiredDependency : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<BaseModule>() // BaseModule not registered
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "missing-dep";
        }
    }

    /// <summary>
    /// A module that uses DependsOn with Type parameter.
    /// </summary>
    private class ModuleWithTypeDependency : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn(typeof(BaseModule))
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "type-dep";
        }
    }

    #endregion

    #region Required Dependency Tests

    [Test]
    public async Task Programmatic_Required_Dependency_Works_When_Registered()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<ModuleWithProgrammaticDependency>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Programmatic_Required_Dependency_Throws_When_Not_Registered()
    {
        await Assert.That(async () => await TestPipelineBuilder.Create()
                .AddModule<ModuleWithMissingRequiredDependency>()
                .RunAsync())
            .ThrowsException();
    }

    [Test]
    public async Task Programmatic_Type_Dependency_Works()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<ModuleWithTypeDependency>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    #endregion

    #region Optional Dependency Tests

    [Test]
    public async Task Optional_Dependency_Works_When_Registered()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<OptionalDependencyModule>()
            .AddModule<ModuleWithOptionalDependency>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Optional_Dependency_Does_Not_Fail_When_Not_Registered()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<ModuleWithOptionalDependency>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    #endregion

    #region Conditional Dependency Tests

    [Test]
    public async Task Conditional_Dependency_Works_When_Condition_True_And_Registered()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<ConditionalModule>()
            .AddModule<ModuleWithActiveConditionalDependency>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Conditional_Dependency_Throws_When_Condition_True_And_Not_Registered()
    {
        await Assert.That(async () => await TestPipelineBuilder.Create()
                .AddModule<ModuleWithActiveConditionalDependency>()
                .RunAsync())
            .ThrowsException();
    }

    [Test]
    public async Task Conditional_Dependency_Not_Added_When_Condition_False()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<ModuleWithInactiveConditionalDependency>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    #endregion

    #region Combined Dependency Tests

    [Test]
    public async Task Combined_Attribute_And_Programmatic_Dependencies_Work()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<OptionalDependencyModule>()
            .AddModule<ModuleWithBothAttributeAndProgrammaticDependencies>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Combined_Dependencies_Work_With_Only_Attribute_Dependency_Registered()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<ModuleWithBothAttributeAndProgrammaticDependencies>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Chained_Dependency_Declarations_Work()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<OptionalDependencyModule>()
            .AddModule<ModuleWithChainedDependencies>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Chained_Dependency_Declarations_Work_With_Only_Required_Registered()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<ModuleWithChainedDependencies>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    #endregion
}
