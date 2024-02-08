using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class SafeEstimatedTimeProviderTests
{
    [Test]
    public async Task When_EstimatedTimeProvider_Succeeds_Then_No_Error()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddModuleEstimatedTimeProvider<SuccessfulTimeProvider>()
            .ExecutePipelineAsync();

        var dummyModule = pipelineSummary.Modules.OfType<DummyModule>().First();
        await Assert.That(dummyModule.Status).Is.EqualTo(Status.Successful);
    }

    [Test]
    public async Task When_EstimatedTimeProvider_Fails_Receiving_Time_Then_Still_No_Error()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddModuleEstimatedTimeProvider<FailingTimeProvider>()
            .ExecutePipelineAsync();

        var dummyModule = pipelineSummary.Modules.OfType<DummyModule>().First();
        await Assert.That(dummyModule.Status).Is.EqualTo(Status.Successful);
    }

    [Test]
    public async Task When_EstimatedTimeProvider_Fails_Saving_Time_Then_Still_No_Error()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddModuleEstimatedTimeProvider<FailingTimeProvider2>()
            .ExecutePipelineAsync();

        var dummyModule = pipelineSummary.Modules.OfType<DummyModule>().First();
        await Assert.That(dummyModule.Status).Is.EqualTo(Status.Successful);
    }

    private class DummyModule : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await new Dictionary<string, object>().AsTask();
        }
    }

    private class SuccessfulTimeProvider : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
        {
            return Task.FromResult(TimeSpan.FromMinutes(1));
        }

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType)
        {
            return Task.FromResult<IEnumerable<SubModuleEstimation>>(new List<SubModuleEstimation>());
        }

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation)
        {
            return Task.CompletedTask;
        }
    }

    private class FailingTimeProvider : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
        {
            throw new Exception();
        }

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
        {
            throw new Exception();
        }

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType)
        {
            throw new Exception();
        }

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation)
        {
            throw new Exception();
        }
    }

    private class FailingTimeProvider2 : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
        {
            return Task.FromResult(TimeSpan.FromMinutes(2));
        }

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
        {
            throw new Exception();
        }

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType)
        {
            return Task.FromResult<IEnumerable<SubModuleEstimation>>(new List<SubModuleEstimation>());
        }

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation)
        {
            throw new Exception();
        }
    }
}