namespace ModularPipelines.UnitTests.FSharp.Models

open System.Collections.Generic
open System.Text.Json
open Microsoft.Extensions.DependencyInjection
open ModularPipelines.Engine
open ModularPipelines.Extensions
open ModularPipelines.Models
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private JsonModule1() =
    inherit Module<IDictionary<string, obj>>()
    override _.ExecuteAsync(_, _) =
        task {
            do! System.Threading.Tasks.Task.Yield()
            let d = Dictionary<string, obj>()
            d["Foo"] <- "Bar"
            d["Hello"] <- "world!"
            return d :> IDictionary<string, obj>
        }

type JsonSerializationTests() =
    inherit TestBase()

    [<Test>]
    member _.Test1() = async {
        let! host =
            TestPipelineHostBuilder.Create()
                .AddModule<JsonModule1>()
                .BuildHostAsync()
            |> Async.AwaitTask

        let! pipelineSummary = host.ExecutePipelineAsync() |> Async.AwaitTask

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let module1Result = resultRegistry.GetResult<IDictionary<string, obj>>(typeof<JsonModule1>)

        let pipelineJson = JsonSerializer.Serialize(pipelineSummary)
        let deserializedSummary = JsonSerializer.Deserialize<PipelineSummary>(pipelineJson)

        do! check(Assert.That(pipelineJson).IsNotNull())
        do! check(Assert.That(pipelineJson).IsNotEmpty())
        do! check(Assert.That(deserializedSummary).IsNotNull())
        do! check(Assert.That(deserializedSummary.Start = pipelineSummary.Start).IsTrue())
        do! check(Assert.That(deserializedSummary.End = pipelineSummary.End).IsTrue())
        do! check(Assert.That(deserializedSummary.TotalDuration = pipelineSummary.TotalDuration).IsTrue())
        do! check(Assert.That(deserializedSummary.Modules |> Seq.isEmpty).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(module1Result.ValueOrDefault["Foo"].ToString()), "Bar"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(module1Result.ValueOrDefault["Hello"].ToString()), "world!"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(module1Result.ModuleName), typeof<JsonModule1>.Name))
    }
