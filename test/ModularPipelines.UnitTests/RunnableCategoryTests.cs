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
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
        }
    }

    [ModuleCategory("Run2")]
    private class RunnableModule2 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
        }
    }

    [ModuleCategory("Run1")]
    private class RunnableModule3 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
        }
    }

    [ModuleCategory("NoRun1")]
    private class NonRunnableModule1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
        }
    }

    [ModuleCategory("NoRun2")]
    private class NonRunnableModule2 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
        }
    }

    private class OtherModule3 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
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
        
        await using (Assert.Multiple())
        {
            await Assert.That(pipelineSummary.GetModule<RunnableModule1>().Status).Is.EqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<RunnableModule2>().Status).Is.EqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<RunnableModule3>().Status).Is.EqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<NonRunnableModule1>().Status).Is.EqualTo(Status.Skipped);
            await Assert.That(pipelineSummary.GetModule<NonRunnableModule2>().Status).Is.EqualTo(Status.Skipped);
            await Assert.That(pipelineSummary.GetModule<OtherModule3>().Status).Is.EqualTo(Status.Skipped);
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
        
        await using (Assert.Multiple())
        {
            await Assert.That(pipelineSummary.GetModule<RunnableModule1>().Status).Is.EqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<RunnableModule2>().Status).Is.EqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<RunnableModule3>().Status).Is.EqualTo(Status.Successful);
            await Assert.That(pipelineSummary.GetModule<NonRunnableModule1>().Status).Is.EqualTo(Status.Skipped);
            await Assert.That(pipelineSummary.GetModule<NonRunnableModule2>().Status).Is.EqualTo(Status.Skipped);
            await Assert.That(pipelineSummary.GetModule<OtherModule3>().Status).Is.EqualTo(Status.Successful);
        }
    }
}