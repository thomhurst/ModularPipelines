using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions;

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
            Assert.That(moduleJson).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(deserializedModule).Is.Not.Null();
        }

        var pipelineJson = JsonSerializer.Serialize(pipelineSummary);
        var deserializedSummary = JsonSerializer.Deserialize<PipelineSummary>(pipelineJson);
        
        await using (Assert.Multiple())
        {
            Assert.That(pipelineJson).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(deserializedSummary).Is.Not.Null();
        }

        var module1Deserialized = deserializedSummary!.GetModule<Module1>();
        var module1DeserializedResult = await module1Deserialized;
        
        await using (Assert.Multiple())
        {
            Assert.That(module1Deserialized).Is.Not.Null();
            Assert.That(module1DeserializedResult).Is.Not.Null();

            Assert.That(module1DeserializedResult.Value!["Foo"].ToString()).Is.EqualTo("Bar");
            Assert.That(module1DeserializedResult.Value!["Hello"].ToString()).Is.EqualTo("world!");

            Assert.That(deserializedSummary.Start).Is.EqualTo(pipelineSummary.Start);
            Assert.That(deserializedSummary.End).Is.EqualTo(pipelineSummary.End);
            Assert.That(deserializedSummary.TotalDuration).Is.EqualTo(pipelineSummary.TotalDuration);
            Assert.That(deserializedSummary.Modules).Has.Count().EqualTo(pipelineSummary.Modules.Count);
            Assert.That(deserializedSummary.Status).Is.EqualTo(pipelineSummary.Status);

            Assert.That(module1Deserialized.StartTime).Is.EqualTo(module.StartTime);
            Assert.That(module1Deserialized.EndTime).Is.EqualTo(module.EndTime);
            Assert.That(module1Deserialized.Duration).Is.EqualTo(module.Duration);
            Assert.That(module1Deserialized.SkipResult).Is.EqualTo(module.SkipResult);
            Assert.That(module1Deserialized.GetType().Name).Is.EqualTo(module.GetType().Name);
            Assert.That(module1Deserialized.TypeDiscriminator).Is.EqualTo(module.GetType().AssemblyQualifiedName!);

            Assert.That(module1DeserializedResult.ModuleStart).Is.EqualTo(module.StartTime);
            Assert.That(module1DeserializedResult.ModuleEnd).Is.EqualTo(module.EndTime);
            Assert.That(module1DeserializedResult.ModuleDuration).Is.EqualTo(module.Duration);
            Assert.That(module1DeserializedResult.SkipDecision).Is.EqualTo(module.SkipResult);
            Assert.That(module1DeserializedResult.ModuleName).Is.EqualTo(module.GetType().Name);
        }
    }
}