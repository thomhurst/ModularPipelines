namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System.Threading.Tasks
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

[<ModuleCategory("compile")>]
type CompileModule() =
    inherit SimpleTestModule<string>()
    override _.Result = "compiled"

[<ModuleCategory("test")>]
[<ModularPipelines.Attributes.DependsOn(typeof<CompileModule>, Optional = true)>]
type TestModuleWithOptionalDep() =
    inherit Module<string>()

    override _.ExecuteAsync(context, _) =
        task {
            let compile = context.GetModuleIfRegistered<CompileModule>()

            if isNull compile then
                return "test-without-compile"
            else
                let! result = compile.CompletionSource.Task
                return if result.IsSkipped then "test-compile-skipped" else $"test-with-{result.ValueOrDefault}"
        }

[<ModuleCategory("test")>]
[<ModularPipelines.Attributes.DependsOn(typeof<CompileModule>, Optional = true)>]
type TestModuleWithOptionalDepForCategoryFilter() =
    inherit Module<string>()

    override _.ExecuteAsync(context, _) =
        task {
            let compile = context.GetModuleIfRegistered<CompileModule>()

            if isNull compile then
                return "test-without-compile"
            else
                let! result = compile.CompletionSource.Task
                return if result.IsSkipped then "test-compile-skipped" else $"test-with-{result.ValueOrDefault}"
        }

type CategoryFilterDependencyTests() =
    inherit TestBase()

    [<Test>]
    member _.Optional_Dependency_Works_When_Filtered_By_Category() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<CompileModule>()
                .AddModule<TestModuleWithOptionalDep>()
                .ConfigurePipelineOptions(fun opt -> opt.RunOnlyCategories <- ResizeArray([ "test" ]))
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))

        let testModule =
            pipelineSummary.Modules
            |> Seq.find (fun moduleInstance -> moduleInstance :? TestModuleWithOptionalDep)
            :?> TestModuleWithOptionalDep

        let! result = testModule.CompletionSource.Task |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ValueOrDefault), "test-compile-skipped"))
    }

    [<Test>]
    member _.Optional_Dependency_Is_Skipped_When_Filtered_By_Category() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<CompileModule>()
                .AddModule<TestModuleWithOptionalDepForCategoryFilter>()
                .ConfigurePipelineOptions(fun opt -> opt.RunOnlyCategories <- ResizeArray([ "test" ]))
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))

        let testModule =
            pipelineSummary.Modules
            |> Seq.find (fun moduleInstance -> moduleInstance :? TestModuleWithOptionalDepForCategoryFilter)
            :?> TestModuleWithOptionalDepForCategoryFilter

        let! result = testModule.CompletionSource.Task |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ValueOrDefault), "test-compile-skipped"))
    }

    [<Test>]
    member _.Both_Categories_Run_Successfully() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<CompileModule>()
                .AddModule<TestModuleWithOptionalDep>()
                .ConfigurePipelineOptions(fun opt -> opt.RunOnlyCategories <- ResizeArray([ "compile"; "test" ]))
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))

        let testModule =
            pipelineSummary.Modules
            |> Seq.find (fun moduleInstance -> moduleInstance :? TestModuleWithOptionalDep)
            :?> TestModuleWithOptionalDep

        let! result = testModule.CompletionSource.Task |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ValueOrDefault), "test-with-compiled"))
    }
