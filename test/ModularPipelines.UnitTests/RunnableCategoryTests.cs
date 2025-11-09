using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class RunnableCategoryTests : TestBase
{
    [ModuleCategory("Run1")]
    private class RunnableModule1 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    [ModuleCategory("Run2")]
    private class RunnableModule2 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    [ModuleCategory("Run1")]
    private class RunnableModule3 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    [ModuleCategory("NoRun1")]
    private class NonRunnableModule1 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    [ModuleCategory("NoRun2")]
    private class NonRunnableModule2 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    private class OtherModule3 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    [Test]
    public async Task When_RunCategories_Specified_Then_Expected_Modules_Run()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<RunnableModule1>()
            .AddModule<RunnableModule2>()
            .AddModule<NonRunnableModule1>()
            .AddModule<NonRunnableModule2>()
            .AddModule<RunnableModule3>()
            .AddModule<OtherModule3>()
            .RunCategories("Run1", "Run2")
            .ExecutePipelineAsync();

        using (Assert.Multiple())
        {
            await Assert.That(pipelineSummary.GetModule<RunnableModule1>().Status).IsEqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<RunnableModule2>().Status).IsEqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<RunnableModule3>().Status).IsEqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<NonRunnableModule1>().Status).IsEqualTo(Status.Skipped);
            await Assert.That(pipelineSummary.GetModule<NonRunnableModule2>().Status).IsEqualTo(Status.Skipped);
            await Assert.That(pipelineSummary.GetModule<OtherModule3>().Status).IsEqualTo(Status.Skipped);
        }
    }

    [Test]
    public async Task When_IgnoreCategories_Specified_Then_Expected_Modules_Run()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<RunnableModule1>()
            .AddModule<RunnableModule2>()
            .AddModule<NonRunnableModule1>()
            .AddModule<NonRunnableModule2>()
            .AddModule<RunnableModule3>()
            .AddModule<OtherModule3>()
            .IgnoreCategories("NoRun1", "NoRun2")
            .ExecutePipelineAsync();

        using (Assert.Multiple())
        {
            await Assert.That(pipelineSummary.GetModule<RunnableModule1>().Status).IsEqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<RunnableModule2>().Status).IsEqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<RunnableModule3>().Status).IsEqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<NonRunnableModule1>().Status).IsEqualTo(Status.Skipped);
            await Assert.That(pipelineSummary.GetModule<NonRunnableModule2>().Status).IsEqualTo(Status.Skipped);
            await Assert.That(pipelineSummary.GetModule<OtherModule3>().Status).IsEqualTo(Status.Successful);
        }
    }
}