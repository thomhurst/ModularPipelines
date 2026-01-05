using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

/// <summary>
/// Tests for the dynamic dependency declaration feature introduced in Issue #1870.
/// </summary>
public class DynamicDependencyDeclarationTests : TestBase
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
    /// A module for testing lazy dependencies.
    /// </summary>
    private class LazyModule : SimpleTestModule<string>
    {
        protected override string Result => "lazy";
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
        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOn<BaseModule>();
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOnOptional<OptionalDependencyModule>();
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOnIf<ConditionalModule>(true);
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOnIf<ConditionalModule>(false);
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "conditional-inactive";
        }
    }

    /// <summary>
    /// A module that declares a conditional dependency using a predicate.
    /// </summary>
    private class ModuleWithPredicateConditionalDependency : Module<string>
    {
        public static bool ShouldDependOnConditional { get; set; } = true;

        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOnIf<ConditionalModule>(() => ShouldDependOnConditional);
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "predicate-conditional";
        }
    }

    /// <summary>
    /// A module that declares a lazy dependency.
    /// </summary>
    private class ModuleWithLazyDependency : Module<string>
    {
        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOnLazy<LazyModule>();
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "lazy-dep";
        }
    }

    /// <summary>
    /// A module that combines both attribute and programmatic dependencies.
    /// </summary>
    [ModularPipelines.Attributes.DependsOn<BaseModule>]
    private class ModuleWithBothAttributeAndProgrammaticDependencies : Module<string>
    {
        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOnOptional<OptionalDependencyModule>();
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOn<BaseModule>()
                .DependsOnOptional<OptionalDependencyModule>()
                .DependsOnLazy<LazyModule>();
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOn<BaseModule>(); // BaseModule not registered
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOn(typeof(BaseModule));
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<ModuleWithProgrammaticDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Programmatic_Required_Dependency_Throws_When_Not_Registered()
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<ModuleWithMissingRequiredDependency>()
                .ExecutePipelineAsync())
            .ThrowsException();
    }

    [Test]
    public async Task Programmatic_Type_Dependency_Works()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<ModuleWithTypeDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    #endregion

    #region Optional Dependency Tests

    [Test]
    public async Task Optional_Dependency_Works_When_Registered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<OptionalDependencyModule>()
            .AddModule<ModuleWithOptionalDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Optional_Dependency_Does_Not_Fail_When_Not_Registered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithOptionalDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    #endregion

    #region Conditional Dependency Tests

    [Test]
    public async Task Conditional_Dependency_Works_When_Condition_True_And_Registered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ConditionalModule>()
            .AddModule<ModuleWithActiveConditionalDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Conditional_Dependency_Throws_When_Condition_True_And_Not_Registered()
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<ModuleWithActiveConditionalDependency>()
                .ExecutePipelineAsync())
            .ThrowsException();
    }

    [Test]
    public async Task Conditional_Dependency_Not_Added_When_Condition_False()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithInactiveConditionalDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    [TUnit.Core.NotInParallel(Order = 1)]
    public async Task Conditional_Predicate_Dependency_Works_When_Predicate_Returns_True()
    {
        ModuleWithPredicateConditionalDependency.ShouldDependOnConditional = true;

        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ConditionalModule>()
            .AddModule<ModuleWithPredicateConditionalDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    [TUnit.Core.NotInParallel(Order = 2)]
    public async Task Conditional_Predicate_Dependency_Not_Added_When_Predicate_Returns_False()
    {
        ModuleWithPredicateConditionalDependency.ShouldDependOnConditional = false;

        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithPredicateConditionalDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    #endregion

    #region Lazy Dependency Tests

    [Test]
    public async Task Lazy_Dependency_Does_Not_Fail_When_Not_Registered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithLazyDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Lazy_Dependency_Works_When_Registered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<LazyModule>()
            .AddModule<ModuleWithLazyDependency>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    #endregion

    #region Combined Dependency Tests

    [Test]
    public async Task Combined_Attribute_And_Programmatic_Dependencies_Work()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<OptionalDependencyModule>()
            .AddModule<ModuleWithBothAttributeAndProgrammaticDependencies>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Combined_Dependencies_Work_With_Only_Attribute_Dependency_Registered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<ModuleWithBothAttributeAndProgrammaticDependencies>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Chained_Dependency_Declarations_Work()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<OptionalDependencyModule>()
            .AddModule<LazyModule>()
            .AddModule<ModuleWithChainedDependencies>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Chained_Dependency_Declarations_Work_With_Only_Required_Registered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<BaseModule>()
            .AddModule<ModuleWithChainedDependencies>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    #endregion

    #region DependencyDeclaration Unit Tests

    [Test]
    public async Task DependencyDeclaration_DependsOn_Returns_Same_Instance_For_Chaining()
    {
        var declaration = new DependencyDeclaration();

        var result = declaration.DependsOn<BaseModule>();

        await Assert.That(result).IsSameReferenceAs(declaration);
    }

    [Test]
    public async Task DependencyDeclaration_DependsOnOptional_Returns_Same_Instance_For_Chaining()
    {
        var declaration = new DependencyDeclaration();

        var result = declaration.DependsOnOptional<BaseModule>();

        await Assert.That(result).IsSameReferenceAs(declaration);
    }

    [Test]
    public async Task DependencyDeclaration_DependsOnIf_Returns_Same_Instance_For_Chaining()
    {
        var declaration = new DependencyDeclaration();

        var result = declaration.DependsOnIf<BaseModule>(true);

        await Assert.That(result).IsSameReferenceAs(declaration);
    }

    [Test]
    public async Task DependencyDeclaration_DependsOnLazy_Returns_Same_Instance_For_Chaining()
    {
        var declaration = new DependencyDeclaration();

        var result = declaration.DependsOnLazy<BaseModule>();

        await Assert.That(result).IsSameReferenceAs(declaration);
    }

    [Test]
    public async Task DependencyDeclaration_Throws_For_Non_Module_Type()
    {
        var declaration = new DependencyDeclaration();

        await Assert.That(() => declaration.DependsOn(typeof(string)))
            .ThrowsException()
            .And.HasMessageContaining("is not a Module");
    }

    [Test]
    public async Task DependencyDeclaration_Required_Has_Correct_DependencyType()
    {
        var declaration = new DependencyDeclaration();
        declaration.DependsOn<BaseModule>();

        var deps = declaration.Dependencies;

        await Assert.That(deps).HasCount().EqualTo(1);
        await Assert.That(deps[0].Kind).IsEqualTo(DependencyType.Required);
        await Assert.That(deps[0].IgnoreIfNotRegistered).IsEqualTo(false);
    }

    [Test]
    public async Task DependencyDeclaration_Optional_Has_Correct_DependencyType()
    {
        var declaration = new DependencyDeclaration();
        declaration.DependsOnOptional<BaseModule>();

        var deps = declaration.Dependencies;

        await Assert.That(deps).HasCount().EqualTo(1);
        await Assert.That(deps[0].Kind).IsEqualTo(DependencyType.Optional);
        await Assert.That(deps[0].IgnoreIfNotRegistered).IsEqualTo(true);
    }

    [Test]
    public async Task DependencyDeclaration_Lazy_Has_Correct_DependencyType()
    {
        var declaration = new DependencyDeclaration();
        declaration.DependsOnLazy<BaseModule>();

        var deps = declaration.Dependencies;

        await Assert.That(deps).HasCount().EqualTo(1);
        await Assert.That(deps[0].Kind).IsEqualTo(DependencyType.Lazy);
        await Assert.That(deps[0].IgnoreIfNotRegistered).IsEqualTo(true);
    }

    [Test]
    public async Task DependencyDeclaration_Conditional_Has_Correct_DependencyType()
    {
        var declaration = new DependencyDeclaration();
        declaration.DependsOnIf<BaseModule>(true);

        var deps = declaration.Dependencies;

        await Assert.That(deps).HasCount().EqualTo(1);
        await Assert.That(deps[0].Kind).IsEqualTo(DependencyType.Conditional);
        await Assert.That(deps[0].IgnoreIfNotRegistered).IsEqualTo(false);
    }

    [Test]
    public async Task DependencyDeclaration_Conditional_False_Does_Not_Add_Dependency()
    {
        var declaration = new DependencyDeclaration();
        declaration.DependsOnIf<BaseModule>(false);

        var deps = declaration.Dependencies;

        await Assert.That(deps).HasCount().EqualTo(0);
    }

    #endregion
}
