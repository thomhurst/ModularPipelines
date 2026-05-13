namespace ModularPipelines.UnitTests.FSharp.Results

open System.Text.Json
open Microsoft.Extensions.DependencyInjection
open ModularPipelines.Context
open ModularPipelines.Engine
open ModularPipelines.Extensions
open ModularPipelines.FileSystem
open ModularPipelines.Models
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open ModularPipelines.Enums

[<AutoOpen>]
module private ResultsRepositoryTestsHelpers =
    let sharedFolder = Folder.CreateTemporaryFolder()

type private JsonResultRepository() =
    interface IModuleResultRepository with
        member _.IsEnabled = true

        member _.SaveResultAsync(m, moduleResult, _) =
            task {
                let file = sharedFolder.CreateFile(m.GetType().FullName)
                use! fileStream = System.Threading.Tasks.Task.FromResult(file.GetStream())
                do! JsonSerializer.SerializeAsync(fileStream, moduleResult)
            }

        member _.GetResultAsync(m, _) =
            task {
                let file = sharedFolder.GetFile(m.GetType().FullName)
                use! fileStream = System.Threading.Tasks.Task.FromResult(file.GetStream())
                return! JsonSerializer.DeserializeAsync<ModuleResult<'T>>(fileStream)
            }

type private RepoModule1() =
    inherit SimpleTestModule<bool>()
    override _.Result = true

[<ModularPipelines.Attributes.DependsOn(typeof<RepoModule1>)>]
type private RepoModule2() =
    inherit SimpleTestModule<bool>()
    override _.Result = true

[<TUnit.Core.NotInParallel(nameof ResultsRepositoryTests, Order = 1)>]
type ResultsRepositoryTests() =
    inherit TestBase()

    [<Test>]
    [<TUnit.Core.NotInParallel(nameof ResultsRepositoryTests, Order = 1)>]
    member _.RunOne() = async {
        let! host =
            TestPipelineHostBuilder
                .Create()
                .AddResultsRepository<JsonResultRepository>()
                .AddModule<RepoModule1>()
                .AddModule<RepoModule2>()
                .BuildHostAsync()
            |> Async.AwaitTask

        do! (host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore)

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let module1Result = resultRegistry.GetResult(typeof<RepoModule1>)
        let module2Result = resultRegistry.GetResult(typeof<RepoModule2>)

        do! check(Assert.That(module1Result.ModuleStatus).IsEqualTo(Status.Successful))
        do! check(Assert.That(module2Result.ModuleStatus).IsEqualTo(Status.Successful))
    }

    [<Test>]
    [<TUnit.Core.NotInParallel(nameof ResultsRepositoryTests, Order = 2)>]
    member _.RunTwoFromHistory() = async {
        let! seedHost =
            TestPipelineHostBuilder
                .Create()
                .AddResultsRepository<JsonResultRepository>()
                .AddModule<RepoModule1>()
                .AddModule<RepoModule2>()
                .BuildHostAsync()
            |> Async.AwaitTask

        do! (seedHost.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore)

        let! host =
            TestPipelineHostBuilder
                .Create()
                .AddResultsRepository<JsonResultRepository>()
                .AddModule<RepoModule1>()
                .AddModule<RepoModule2>()
                .RunCategories("Other")
                .BuildHostAsync()
            |> Async.AwaitTask

        do! (host.ExecutePipelineAsync() |> Async.AwaitTask |> Async.Ignore)

        let resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>()
        let module1Result = resultRegistry.GetResult(typeof<RepoModule1>)
        let module2Result = resultRegistry.GetResult(typeof<RepoModule2>)

        do! check(Assert.That(module1Result.ModuleStatus).IsEqualTo(Status.UsedHistory))
        do! check(Assert.That(module2Result.ModuleStatus).IsEqualTo(Status.UsedHistory))
    }
