using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Execution;

public class RunnableCategoryTests : TestBase
{
    [ModuleCategory("Run1")]
    private class RunnableModule1 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModuleCategory("Run2")]
    private class RunnableModule2 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModuleCategory("Run1")]
    private class RunnableModule3 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModuleCategory("NoRun1")]
    private class NonRunnableModule1 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModuleCategory("NoRun2")]
    private class NonRunnableModule2 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class OtherModule3 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class ConfiguredCategoryModule : SimpleTestModule<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithCategory("Run1");

        protected override bool Result => true;
    }

    [Test]
    public async Task When_RunOnlyCategories_Specified_Then_Expected_Modules_Run()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<RunnableModule1>()
            .AddModule<RunnableModule2>()
            .AddModule<NonRunnableModule1>()
            .AddModule<NonRunnableModule2>()
            .AddModule<RunnableModule3>()
            .AddModule<OtherModule3>()
            .ConfigureOptions(options => options with { RunOnlyCategories = ["Run1", "Run2"] })
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        using (Assert.Multiple())
        {
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule1))!.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule2))!.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule3))!.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(resultRegistry.GetResult(typeof(NonRunnableModule1))!.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(resultRegistry.GetResult(typeof(NonRunnableModule2))!.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(resultRegistry.GetResult(typeof(OtherModule3))!.Status).IsEqualTo(ModuleStatus.Skipped);
        }
    }

    [Test]
    public async Task RunOnlyCategories_Matches_Module_Category_Ignoring_Case()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<RunnableModule1>()
            .ConfigureOptions(options => options with { RunOnlyCategories = ["run1"] })
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        await Assert.That(resultRegistry.GetResult(typeof(RunnableModule1))!.Status)
            .IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task When_IgnoreCategories_Specified_Then_Expected_Modules_Run()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<RunnableModule1>()
            .AddModule<RunnableModule2>()
            .AddModule<NonRunnableModule1>()
            .AddModule<NonRunnableModule2>()
            .AddModule<RunnableModule3>()
            .AddModule<OtherModule3>()
            .ConfigureOptions(options => options with { IgnoreCategories = ["NoRun1", "NoRun2"] })
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        using (Assert.Multiple())
        {
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule1))!.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule2))!.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule3))!.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(resultRegistry.GetResult(typeof(NonRunnableModule1))!.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(resultRegistry.GetResult(typeof(NonRunnableModule2))!.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(resultRegistry.GetResult(typeof(OtherModule3))!.Status).IsEqualTo(ModuleStatus.Succeeded);
        }
    }

    [Test]
    public async Task IgnoreCategories_Matches_Module_Category_Ignoring_Case()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<NonRunnableModule1>()
            .ConfigureOptions(options => options with { IgnoreCategories = ["norun1"] })
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        await Assert.That(resultRegistry.GetResult(typeof(NonRunnableModule1))!.Status)
            .IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Configured_Category_Is_Used_For_Run_Filtering()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<ConfiguredCategoryModule>()
            .ConfigureOptions(options => options with { RunOnlyCategories = ["Run1"] })
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        await Assert.That(resultRegistry.GetResult(typeof(ConfiguredCategoryModule))!.Status)
            .IsEqualTo(ModuleStatus.Succeeded);
    }
}
