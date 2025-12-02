using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class JsonSerializationTests : TestBase
{
    public class Module1 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return new Dictionary<string, object>
            {
                ["Foo"] = "Bar",
                ["Hello"] = "world!"
            };
        }
    }

    public class Module2 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return new Dictionary<string, object>
            {
                ["Foo2"] = "Bar",
                ["Hello2"] = "world!"
            };
        }
    }

    public class Module3 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return new Dictionary<string, object>
            {
                ["Foo3"] = "Bar",
                ["Hello3"] = "world!"
            };
        }
    }

    [Test]
    public async Task Test1()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .BuildHostAsync();

        var pipelineSummary = await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var module1Result = resultRegistry.GetResult<IDictionary<string, object>?>(typeof(Module1))!;

        // Serialize and deserialize the pipeline summary
        var pipelineJson = JsonSerializer.Serialize(pipelineSummary);
        var deserializedSummary = JsonSerializer.Deserialize<PipelineSummary>(pipelineJson);

        using (Assert.Multiple())
        {
            await Assert.That(pipelineJson).IsNotNull().And.IsNotEmpty();
            await Assert.That(deserializedSummary).IsNotNull();
            await Assert.That(deserializedSummary!.Start).IsEqualTo(pipelineSummary.Start);
            await Assert.That(deserializedSummary.End).IsEqualTo(pipelineSummary.End);
            await Assert.That(deserializedSummary.TotalDuration).IsEqualTo(pipelineSummary.TotalDuration);
            // Modules are not serialized (interface types can't be deserialized)
            await Assert.That(deserializedSummary.Modules).HasCount().EqualTo(0);
        }

        // Verify the module result values
        using (Assert.Multiple())
        {
            await Assert.That(module1Result.Value!["Foo"].ToString()).IsEqualTo("Bar");
            await Assert.That(module1Result.Value!["Hello"].ToString()).IsEqualTo("world!");
            await Assert.That(module1Result.ModuleName).IsEqualTo(typeof(Module1).Name);
        }
    }
}
