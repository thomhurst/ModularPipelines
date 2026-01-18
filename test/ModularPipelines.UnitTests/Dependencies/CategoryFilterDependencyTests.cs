using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

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
            return result.IsSkipped ? "test-compile-skipped" : $"test-with-{result.ValueOrDefault}";
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
            return result.IsSkipped ? "test-compile-skipped" : $"test-with-{result.ValueOrDefault}";
        }
    }

    [Test]
    public async Task Optional_Dependency_Works_When_Filtered_By_Category()
    {
        // Issue #2164: Running only "test" category with optional deps should work
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<CompileModule>()
            .AddModule<TestModuleWithOptionalDep>()
            .ConfigurePipelineOptions(opt => opt.RunOnlyCategories = ["test"])
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);

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
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<CompileModule>()
            .AddModule<TestModuleWithOptionalDepForCategoryFilter>()
            .ConfigurePipelineOptions(opt => opt.RunOnlyCategories = ["test"])
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);

        var testModule = pipelineSummary.Modules.OfType<TestModuleWithOptionalDepForCategoryFilter>().Single();
        var result = await testModule;
        // CompileModule was skipped due to category filter
        await Assert.That(result.ValueOrDefault).IsEqualTo("test-compile-skipped");
    }

    [Test]
    public async Task Both_Categories_Run_Successfully()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<CompileModule>()
            .AddModule<TestModuleWithOptionalDep>()
            .ConfigurePipelineOptions(opt => opt.RunOnlyCategories = ["compile", "test"])
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);

        var testModule = pipelineSummary.Modules.OfType<TestModuleWithOptionalDep>().Single();
        var result = await testModule;
        await Assert.That(result.ValueOrDefault).IsEqualTo("test-with-compiled");
    }
}
