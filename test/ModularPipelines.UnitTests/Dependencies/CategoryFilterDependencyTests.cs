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
    [ModularPipelines.Attributes.DependsOn<CompileModule>]  // Optional by default
    private class TestModule : Module<string>
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
    [RequiresDependency<CompileModule>]
    private class TestModuleWithRequiredDep : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var result = await context.GetModule<CompileModule>();
            return $"test-with-{result.ValueOrDefault}";
        }
    }

    [Test]
    public async Task Optional_Dependency_Works_When_Filtered_By_Category()
    {
        // Issue #2164: Running only "test" category should not throw for optional deps
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<CompileModule>()
            .AddModule<TestModule>()
            .ConfigurePipelineOptions(opt => opt.RunOnlyCategories = ["test"])
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);

        var testModule = pipelineSummary.Modules.OfType<TestModule>().Single();
        var result = await testModule;
        // CompileModule is filtered out, so TestModule should handle gracefully
        await Assert.That(result.ValueOrDefault).IsEqualTo("test-without-compile")
            .Or.IsEqualTo("test-compile-skipped");
    }

    [Test]
    public async Task Required_Dependency_Fails_When_Filtered_By_Category()
    {
        // Required dependency in different category - should fail validation
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<CompileModule>()
                .AddModule<TestModuleWithRequiredDep>()
                .ConfigurePipelineOptions(opt => opt.RunOnlyCategories = ["test"])
                .ExecutePipelineAsync())
            .ThrowsException();
    }

    [Test]
    public async Task Both_Categories_Run_Successfully()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<CompileModule>()
            .AddModule<TestModule>()
            .ConfigurePipelineOptions(opt => opt.RunOnlyCategories = ["compile", "test"])
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);

        var testModule = pipelineSummary.Modules.OfType<TestModule>().Single();
        var result = await testModule;
        await Assert.That(result.ValueOrDefault).IsEqualTo("test-with-compiled");
    }
}
