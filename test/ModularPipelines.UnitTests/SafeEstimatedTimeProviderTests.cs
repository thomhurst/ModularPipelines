using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Host;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests;

public class SafeEstimatedTimeProviderTests
{
    [Test]
    public async Task When_EstimatedTimeProvider_Succeeds_Then_No_Error()
    {
        var modules = await PipelineHostBuilder.Create()
            .ConfigureServices((context, collection) => collection.Configure<PipelineOptions>(opt => opt.ShowProgressInConsole = false))
            .AddModule<DummyModule>()
            .RegisterEstimatedTimeProvider<SuccessfulTimeProvider>()
            .ExecutePipelineAsync();

        var dummyModule = modules.OfType<DummyModule>().First();
        
        Assert.That(dummyModule.Status, Is.EqualTo(Status.Successful));
    }
    
    [Test]
    public async Task When_EstimatedTimeProvider_Fails_Receiving_Time_Then_Still_No_Error()
    {
        var modules = await PipelineHostBuilder.Create()
            .ConfigureServices((context, collection) => collection.Configure<PipelineOptions>(opt => opt.ShowProgressInConsole = false))
            .AddModule<DummyModule>()
            .RegisterEstimatedTimeProvider<FailingTimeProvider>()
            .ExecutePipelineAsync();

        var dummyModule = modules.OfType<DummyModule>().First();
        
        Assert.That(dummyModule.Status, Is.EqualTo(Status.Successful));
    }
    
    [Test]
    public async Task When_EstimatedTimeProvider_Fails_Saving_Time_Then_Still_No_Error()
    {
        var modules = await PipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .RegisterEstimatedTimeProvider<FailingTimeProvider2>()
            .ExecutePipelineAsync();

        var dummyModule = modules.OfType<DummyModule>().First();
        
        Assert.That(dummyModule.Status, Is.EqualTo(Status.Successful));
    }

    private class DummyModule : Module
    {
        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return ModuleResult.Empty<IDictionary<string, object>>();
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

        public Task<TimeSpan> GetSubModuleEstimatedTimeAsync(Type moduleType, string subModuleName)
        {
            return Task.FromResult(TimeSpan.FromMinutes(1));
        }

        public Task SaveSubModuleTimeAsync(Type moduleType, string subModuleName, TimeSpan duration)
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

        public Task<TimeSpan> GetSubModuleEstimatedTimeAsync(Type moduleType, string subModuleName)
        {
            throw new Exception();
        }

        public Task SaveSubModuleTimeAsync(Type moduleType, string subModuleName, TimeSpan duration)
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

        public Task<TimeSpan> GetSubModuleEstimatedTimeAsync(Type moduleType, string subModuleName)
        {
            throw new Exception();
        }

        public Task SaveSubModuleTimeAsync(Type moduleType, string subModuleName, TimeSpan duration)
        {
            throw new Exception();
        }
    }
}
