using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using NUnit.Framework.Constraints;

namespace ModularPipelines.UnitTests;

public class PipelineRequirementTests
{
    [Test]
    public async Task When_Requirement_Succeeds_Then_No_Error()
    {
        var modules = await PipelineHostBuilder.Create()
            .ConfigureServices((context, collection) => collection.Configure<PipelineOptions>(opt => opt.ShowProgressInConsole = false))
            .ConfigureServices((context, collection) =>
            {
                collection.AddModule<DummyModule>()
                    .AddRequirement<SuccessfulRequirement>();
            })
            .ExecutePipelineAsync();

        var dummyModule = modules.OfType<DummyModule>().First();
        
        Assert.That(dummyModule.Status, Is.EqualTo(Status.Successful));
    }
    
    [Test]
    public void When_Requirement_Fails_Then_Error()
    {
        var executePipelineDelegate = () => PipelineHostBuilder.Create()
            .ConfigureServices((context, collection) =>
            {
                collection.AddModule<DummyModule>()
                    .AddRequirement<FailingRequirement>();
            })
            .ExecutePipelineAsync();
        
        Assert.That(executePipelineDelegate, Throws.Exception.TypeOf<FailedRequirementsException>()
            .With.Message.EqualTo("Requirements failed: FailingRequirement"));
    }

    private class DummyModule : Module
    {
        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return ModuleResult.Empty<IDictionary<string, object>>();
        }
    }
    
    private class SuccessfulRequirement : IPipelineRequirement
    {
        public async Task<bool> MustAsync(IModuleContext context)
        {
            await Task.Yield();
            return true;
        }
    }
    
    private class FailingRequirement : IPipelineRequirement
    {
        public async Task<bool> MustAsync(IModuleContext context)
        {
            await Task.Yield();
            return false;
        }
    }
}
