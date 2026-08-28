using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Dependencies;

/// <summary>
/// Tests for issue #2164: DependsOn and ModuleCategory interaction
/// </summary>
public class CategoryFilterDependencyTests : TestBase
{
    [ModuleCategory("compile")]
    private class CompileModule : SimpleTestModule<string>
    {
        protected override string Result => "compiled";
    }

    [ModuleCategory("compile")]
    [ModularPipelines.Attributes.DependsOn<CompileModule>]
    private class CompileResultConsumerModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var result = await context.GetModule<CompileModule>();
            return result.Value;
        }
    }

    [ModuleCategory("test")]
    [ModularPipelines.Attributes.DependsOn<CompileModule>(Optional = true)]  // Optional - gracefully handle if dependency is filtered
    private class TestModuleWithOptionalDep : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var compile = context.GetModuleIfRegistered<CompileModule>();
            if (compile == null)
            {
                return "test-without-compile";
            }

            var result = await compile;
            return result.SkipDecisionOrDefault is not null
                ? "test-compile-skipped"
                : $"test-with-{result.ValueOrDefault}";
        }
    }

    [ModuleCategory("test")]
    [ModularPipelines.Attributes.DependsOn<CompileModule>(Optional = true)]  // Must be optional when dependency might be filtered by category
    private class TestModuleWithOptionalDepForCategoryFilter : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var compile = context.GetModuleIfRegistered<CompileModule>();
            if (compile == null)
            {
                return "test-without-compile";
            }

            var result = await compile;
            return result.SkipDecisionOrDefault is not null
                ? "test-compile-skipped"
                : $"test-with-{result.ValueOrDefault}";
        }
    }

    [ModuleCategory("test")]
    [ModularPipelines.Attributes.DependsOn<CompileModule>]
    private class TestModuleWithRequiredDep : SimpleTestModule<string>
    {
        protected override string Result => throw new InvalidOperationException("A cascade-skipped module must not execute");
    }

    [ModuleCategory("test")]
    [ModularPipelines.Attributes.DependsOn<TestModuleWithRequiredDep>]
    private class TransitiveRequiredDepModule : SimpleTestModule<string>
    {
        protected override string Result => throw new InvalidOperationException("A transitively cascade-skipped module must not execute");
    }

    private class FluentlySkippedModule : SimpleTestModule<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("Fluent skip"))
            .Build();

        protected override string Result => throw new InvalidOperationException("A fluently skipped module must not execute");
    }

    [ModularPipelines.Attributes.DependsOn<FluentlySkippedModule>]
    private class FluentSkipDependentModule : SimpleTestModule<string>
    {
        protected override string Result => throw new InvalidOperationException("A dependent of a skipped module must not execute");
    }

    private abstract class ValueEqualModule : SimpleTestModule<string>
    {
        public override bool Equals(object? obj) => obj is ValueEqualModule;

        public override int GetHashCode() => 1;
    }

    private class FirstValueEqualModule : ValueEqualModule
    {
        protected override string Result => "first";
    }

    private class SecondValueEqualModule : ValueEqualModule
    {
        protected override string Result => "second";
    }

    [Test]
    public async Task Optional_Dependency_Works_When_Filtered_By_Category()
    {
        // Issue #2164: Running only "test" category with optional deps should work
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<CompileModule>()
            .AddModule<TestModuleWithOptionalDep>()
            .ConfigurePipelineOptions(options => options with { RunOnlyCategories = ["test"] })
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);

        var testModule = pipelineSummary.Modules.OfType<TestModuleWithOptionalDep>().Single();
        var result = await testModule;
        // CompileModule is filtered out (skipped), TestModule handles gracefully
        await Assert.That(result.ValueOrDefault).IsEqualTo("test-compile-skipped");
    }

    [Test]
    public async Task Optional_Dependency_Is_Skipped_When_Filtered_By_Category()
    {
        // When using category filters, dependencies in other categories should be marked optional
        // This test verifies that optional deps work correctly with category filtering
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<CompileModule>()
            .AddModule<TestModuleWithOptionalDepForCategoryFilter>()
            .ConfigurePipelineOptions(options => options with { RunOnlyCategories = ["test"] })
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);

        var testModule = pipelineSummary.Modules.OfType<TestModuleWithOptionalDepForCategoryFilter>().Single();
        var result = await testModule;
        // CompileModule was skipped due to category filter
        await Assert.That(result.ValueOrDefault).IsEqualTo("test-compile-skipped");
    }

    [Test]
    public async Task Required_Dependency_Filtered_By_Category_Cascade_Skips_Dependents()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<CompileModule>()
            .AddModule<TestModuleWithRequiredDep>()
            .AddModule<TransitiveRequiredDepModule>()
            .ConfigurePipelineOptions(options => options with { RunOnlyCategories = ["test"] })
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);

        var requiredResult = await pipelineSummary.Modules
            .OfType<TestModuleWithRequiredDep>()
            .Single();
        var transitiveResult = await pipelineSummary.Modules
            .OfType<TransitiveRequiredDepModule>()
            .Single();

        await Assert.That(requiredResult.SkipDecisionOrDefault).IsNotNull();
        await Assert.That(requiredResult.SkipDecisionOrDefault!.Reason)
            .Contains(nameof(CompileModule));
        await Assert.That(transitiveResult.SkipDecisionOrDefault).IsNotNull();
        await Assert.That(transitiveResult.SkipDecisionOrDefault!.Reason)
            .Contains(nameof(TestModuleWithRequiredDep));
    }

    [Test]
    public async Task Duplicate_Filtered_Module_Types_Are_Rejected_By_Validation()
    {
        var firstCompileModule = new CompileModule();
        var secondCompileModule = new CompileModule();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => TestPipelineBuilder.Create()
            .AddModule(firstCompileModule)
            .AddModule(secondCompileModule)
            .AddModule<TestModuleWithRequiredDep>()
            .ConfigurePipelineOptions(options => options with { RunOnlyCategories = ["test"] })
            .RunAsync());

        await Assert.That(exception!.ValidationResult.Errors.Single().Message)
            .IsEqualTo("Module 'CompileModule' is registered multiple times. Each module type should only be registered once.");
    }

    [Test]
    public async Task Repeated_Runnable_Module_Instance_Is_Registered_Once_For_Dependents()
    {
        var compileModule = new CompileModule();

        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule(compileModule)
            .AddModule(compileModule)
            .AddModule<CompileResultConsumerModule>()
            .ConfigurePipelineOptions(options => options with { RunOnlyCategories = ["compile"] })
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(pipelineSummary.Modules.Count(module => ReferenceEquals(module, compileModule)))
            .IsEqualTo(1);
        await Assert.That((await compileModule).ValueOrDefault).IsEqualTo("compiled");

        var consumerResult = await pipelineSummary.Modules
            .OfType<CompileResultConsumerModule>()
            .Single();
        await Assert.That(consumerResult.ValueOrDefault).IsEqualTo("compiled");
    }

    [Test]
    public async Task Repeated_Factory_Module_Type_Is_Rejected_By_Validation()
    {
        var compileModule = new CompileModule();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => TestPipelineBuilder.Create()
            .AddModule<CompileModule>(_ => compileModule)
            .AddModule<CompileModule>(_ => compileModule)
            .AddModule<CompileResultConsumerModule>()
            .ConfigurePipelineOptions(options => options with { RunOnlyCategories = ["compile"] })
            .RunAsync());

        await Assert.That(exception!.ValidationResult.Errors.Single().Message)
            .IsEqualTo("Module 'CompileModule' is registered multiple times. Each module type should only be registered once.");
    }

    [Test]
    public async Task Fluent_Skip_Cascade_Skips_Required_Dependent()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<FluentlySkippedModule>()
            .AddModule<FluentSkipDependentModule>()
            .RunAsync();

        var dependentResult = await pipelineSummary.Modules
            .OfType<FluentSkipDependentModule>()
            .Single();

        await Assert.That(dependentResult.SkipDecisionOrDefault).IsNotNull();
        await Assert.That(dependentResult.SkipDecisionOrDefault!.Reason)
            .Contains(nameof(FluentlySkippedModule));
    }

    [Test]
    public async Task Value_Equal_Module_Instances_Are_Discovered_Independently()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<FirstValueEqualModule>()
            .AddModule<SecondValueEqualModule>()
            .RunAsync();

        await Assert.That((await pipelineSummary.Modules.OfType<FirstValueEqualModule>().Single()).ValueOrDefault)
            .IsEqualTo("first");
        await Assert.That((await pipelineSummary.Modules.OfType<SecondValueEqualModule>().Single()).ValueOrDefault)
            .IsEqualTo("second");
    }

    [Test]
    public async Task Both_Categories_Run_Successfully()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<CompileModule>()
            .AddModule<TestModuleWithOptionalDep>()
            .ConfigurePipelineOptions(options => options with { RunOnlyCategories = ["compile", "test"] })
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);

        var testModule = pipelineSummary.Modules.OfType<TestModuleWithOptionalDep>().Single();
        var result = await testModule;
        await Assert.That(result.ValueOrDefault).IsEqualTo("test-with-compiled");
    }
}
