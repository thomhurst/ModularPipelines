using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class JsonSerializationTests : TestBase
{
    public class Module1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return new Dictionary<string, object>
            {
                ["Foo"] = "Bar",
                ["Hello"] = "world!"
            };
        }
    }

    public class Module2 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return new Dictionary<string, object>
            {
                ["Foo2"] = "Bar",
                ["Hello2"] = "world!"
            };
        }
    }

    public class Module3 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .ExecutePipelineAsync();

        var module = pipelineSummary.Modules.First();

        var moduleJson = JsonSerializer.Serialize(module);
        var deserializedModule = JsonSerializer.Deserialize<ModuleBase>(moduleJson);
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleJson).IsNotNull().And.IsNotEmpty();
            await Assert.That(deserializedModule).IsNotNull();
        }

        var pipelineJson = JsonSerializer.Serialize(pipelineSummary);
        var deserializedSummary = JsonSerializer.Deserialize<PipelineSummary>(pipelineJson);
        
        await using (Assert.Multiple())
        {
            await Assert.That(pipelineJson).IsNotNull().And.IsNotEmpty();
            await Assert.That(deserializedSummary).IsNotNull();
        }

        var module1Deserialized = deserializedSummary!.GetModule<Module1>();
        var module1DeserializedResult = await module1Deserialized;
        
        await using (Assert.Multiple())
        {
            await Assert.That(module1Deserialized).IsNotNull();
            await Assert.That(module1DeserializedResult).IsNotNull();

            await Assert.That(module1DeserializedResult.Value!["Foo"].ToString()).IsEqualTo("Bar");
            await Assert.That(module1DeserializedResult.Value!["Hello"].ToString()).IsEqualTo("world!");

            await Assert.That(deserializedSummary.Start).IsEqualTo(pipelineSummary.Start);
            await Assert.That(deserializedSummary.End).IsEqualTo(pipelineSummary.End);
            await Assert.That(deserializedSummary.TotalDuration).IsEqualTo(pipelineSummary.TotalDuration);
            await Assert.That(deserializedSummary.Modules).HasCount().EqualTo(pipelineSummary.Modules.Count);
            await Assert.That(deserializedSummary.Status).IsEqualTo(pipelineSummary.Status);

            await Assert.That(module1Deserialized.StartTime).IsEqualTo(module.StartTime);
            await Assert.That(module1Deserialized.EndTime).IsEqualTo(module.EndTime);
            await Assert.That(module1Deserialized.Duration).IsEqualTo(module.Duration);
            await Assert.That(module1Deserialized.SkipResult).IsEqualTo(module.SkipResult);
            await Assert.That(module1Deserialized.GetType().Name).IsEqualTo(module.GetType().Name);
            await Assert.That(module1Deserialized.TypeDiscriminator).IsEqualTo(module.GetType().AssemblyQualifiedName!);

            await Assert.That(module1DeserializedResult.ModuleStart).IsEqualTo(module.StartTime);
            await Assert.That(module1DeserializedResult.ModuleEnd).IsEqualTo(module.EndTime);
            await Assert.That(module1DeserializedResult.ModuleDuration).IsEqualTo(module.Duration);
            await Assert.That(module1DeserializedResult.SkipDecision).IsEqualTo(module.SkipResult);
            await Assert.That(module1DeserializedResult.ModuleName).IsEqualTo(module.GetType().Name);
        }
    }
}