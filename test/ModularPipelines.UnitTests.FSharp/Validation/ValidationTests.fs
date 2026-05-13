namespace ModularPipelines.UnitTests.FSharp.Validation

open System
open System.Linq
open ModularPipelines
open ModularPipelines.Context
open ModularPipelines.Exceptions
open ModularPipelines.Extensions
open ModularPipelines.Modules
open ModularPipelines.Validation
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type private SimpleModule() =
    inherit Module<string>()
    override _.ExecuteAsync(_, _) =
        System.Threading.Tasks.Task.FromResult<string>(null)

type private AnotherModule() =
    inherit Module<int>()
    override _.ExecuteAsync(_, _) =
        System.Threading.Tasks.Task.FromResult<int>(42)

[<ModularPipelines.Attributes.DependsOn(typeof<SelfReferencingModule>)>]
type private SelfReferencingModule() =
    inherit Module<string>()
    override _.ExecuteAsync(_, _) =
        System.Threading.Tasks.Task.FromResult("success")

[<ModularPipelines.Attributes.DependsOn(typeof<ModuleB>)>]
type private ModuleA() =
    inherit Module<string>()
    override _.ExecuteAsync(_, _) = System.Threading.Tasks.Task.FromResult<string>(null)

and [<ModularPipelines.Attributes.DependsOn(typeof<ModuleC>)>] ModuleB() =
    inherit Module<string>()
    override _.ExecuteAsync(_, _) = System.Threading.Tasks.Task.FromResult<string>(null)

and [<ModularPipelines.Attributes.DependsOn(typeof<ModuleA>)>] ModuleC() =
    inherit Module<string>()
    override _.ExecuteAsync(_, _) = System.Threading.Tasks.Task.FromResult<string>(null)

type private MissingModule() =
    inherit Module<string>()
    override _.ExecuteAsync(_, _) = System.Threading.Tasks.Task.FromResult<string>(null)

[<ModularPipelines.Attributes.DependsOn(typeof<MissingModule>, Optional = false)>]
type private ModuleWithMissingDep() =
    inherit Module<string>()
    override _.ExecuteAsync(_, _) = System.Threading.Tasks.Task.FromResult<string>(null)

[<ModularPipelines.Attributes.DependsOn(typeof<MissingModule>, Optional = true)>]
type private ModuleWithOptionalDep() =
    inherit Module<string>()
    override _.ExecuteAsync(_, _) = System.Threading.Tasks.Task.FromResult<string>(null)

[<AutoOpen>]
module private ValidationTestHelpers =
    let isBaseException<'T when 'T :> exn> (ex: exn) =
        match ex with
        | :? AggregateException as aggregate -> aggregate.GetBaseException() :? 'T
        | _ -> ex :? 'T

type ValidationTests() =
    [<Test>]
    member _.ValidateAsync_WithValidConfiguration_ReturnsNoErrors() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<SimpleModule>().AddModule<AnotherModule>() |> ignore

        let! result = builder.ValidateAsync() |> Async.AwaitTask

        do! check(Assert.That(result.IsValid).IsTrue())
        do! check(Assert.That(result.HasErrors).IsFalse())
        do! check(Assert.That(result.Errors.Count = 0).IsTrue())
    }

    [<Test>]
    member _.ValidateAsync_WithNoModules_ReturnsError() = async {
        let builder = Pipeline.CreateBuilder()

        let! result = builder.ValidateAsync() |> Async.AwaitTask

        do! check(Assert.That(result.HasErrors).IsTrue())

        do! check(
            Assert.That(result.Errors.Any(fun e -> e.Category = ValidationErrorCategory.ModuleConfiguration))
                .IsTrue()
        )
    }

    [<Test>]
    member _.BuildAsync_WithValidConfiguration_ReturnsPipeline() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<SimpleModule>() |> ignore

        let! pipeline = builder.BuildAsync() |> Async.AwaitTask

        do! check(Assert.That(pipeline).IsNotNull())
        do! check(Assert.That(pipeline.Services).IsNotNull())

        do! pipeline.DisposeAsync().AsTask() |> Async.AwaitTask
    }

    [<Test>]
    member _.BuildAsync_WithNoModules_ThrowsValidationException() = async {
        let builder = Pipeline.CreateBuilder()

        let mutable threw = false

        try
            let! _ = builder.BuildAsync() |> Async.AwaitTask
            ()
        with ex when isBaseException<PipelineValidationException> ex ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.ValidateAsync_WithMissingRequiredDependency_ReturnsNoError_BecauseAutoRegistered() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<ModuleWithMissingDep>() |> ignore

        let! result = builder.ValidateAsync() |> Async.AwaitTask

        let hasDependencyError =
            result.Errors.Any(fun e ->
                e.Category = ValidationErrorCategory.Dependency
                && e.Message.Contains("MissingModule"))

        do! check(Assert.That(hasDependencyError).IsFalse())
    }

    [<Test>]
    member _.ValidateAsync_WithOptionalMissingDependency_ReturnsNoError() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<ModuleWithOptionalDep>() |> ignore

        let! result = builder.ValidateAsync() |> Async.AwaitTask

        let hasDependencyError =
            result.Errors.Any(fun e ->
                e.Category = ValidationErrorCategory.Dependency
                && e.Message.Contains("MissingModule"))

        do! check(Assert.That(hasDependencyError).IsFalse())
    }

    [<Test>]
    member _.RunAsync_AfterBuildAsync_ExecutesPipeline() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<SimpleModule>() |> ignore

        let! pipeline = builder.BuildAsync() |> Async.AwaitTask
        let! summary = pipeline.RunAsync() |> Async.AwaitTask

        do! check(Assert.That(summary).IsNotNull())
        do! check(Assert.That<System.Collections.Generic.IReadOnlyList<IModule>>(summary.Modules).IsNotNull())

        do! pipeline.DisposeAsync().AsTask() |> Async.AwaitTask
    }

    [<Test>]
    member _.ValidationResult_WithError_HasErrorsIsTrue() = async {
        let error = ValidationError(ValidationErrorCategory.Options, "Test error")
        let result = ValidationResult.WithError(error)

        do! check(Assert.That(result.HasErrors).IsTrue())
        do! check(Assert.That(result.IsValid).IsFalse())
        do! check(Assert.That(result.Errors.Count = 1).IsTrue())
    }

    [<Test>]
    member _.ValidationResult_Success_IsValid() = async {
        let result = ValidationResult.Success()

        do! check(Assert.That(result.HasErrors).IsFalse())
        do! check(Assert.That(result.IsValid).IsTrue())
    }

    [<Test>]
    member _.ValidationResult_Merge_CombinesErrors() = async {
        let error1 = ValidationError(ValidationErrorCategory.Options, "Error 1")
        let error2 = ValidationError(ValidationErrorCategory.Dependency, "Error 2")
        let result1 = ValidationResult.WithError(error1)
        let result2 = ValidationResult.WithError(error2)

        result1.Merge(result2)

        do! check(Assert.That(result1.Errors.Count = 2).IsTrue())
    }

    [<Test>]
    member _.ValidationError_ToString_IncludesCategory() = async {
        let error = ValidationError(ValidationErrorCategory.Dependency, "Test message")
        let str = error.ToString()

        do! check(Assert.That(str.Contains("Dependency")).IsTrue())
        do! check(Assert.That(str.Contains("Test message")).IsTrue())
    }

    [<Test>]
    member _.ValidationError_ToString_WithSourceType_IncludesTypeName() = async {
        let error = ValidationError(ValidationErrorCategory.ModuleConfiguration, "Test message", typeof<SimpleModule>)
        let str = error.ToString()

        do! check(Assert.That(str.Contains("SimpleModule")).IsTrue())
    }

    [<Test>]
    member _.ValidateAsync_WithNegativeRetryCount_ReturnsError() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<SimpleModule>() |> ignore
        builder.Options.DefaultRetryCount <- -1

        let! result = builder.ValidateAsync() |> Async.AwaitTask

        do! check(Assert.That(result.HasErrors).IsTrue())

        do! check(
            Assert.That(
                result.Errors.Any(fun e ->
                    e.Category = ValidationErrorCategory.Options
                    && e.Message.Contains("DefaultRetryCount"))
            )
                .IsTrue()
        )
    }

    [<Test>]
    member _.ValidateAsync_WithConflictingCategories_ReturnsError() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<SimpleModule>() |> ignore
        builder.Options.RunOnlyCategories <- ResizeArray([ "Category1" ])
        builder.Options.IgnoreCategories <- ResizeArray([ "Category1" ])

        let! result = builder.ValidateAsync() |> Async.AwaitTask

        do! check(Assert.That(result.HasErrors).IsTrue())

        do! check(
            Assert.That(
                result.Errors.Any(fun e ->
                    e.Category = ValidationErrorCategory.Options
                    && e.Message.Contains("Category1"))
            )
                .IsTrue()
        )
    }

    [<Test>]
    member _.ValidateAsync_WithSelfReferencingModule_ReturnsError() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<SelfReferencingModule>() |> ignore

        let! result = builder.ValidateAsync() |> Async.AwaitTask

        do! check(Assert.That(result.HasErrors).IsTrue())

        do! check(
            Assert.That(
                result.Errors.Any(fun e ->
                    e.Category = ValidationErrorCategory.Dependency
                    && e.Message.Contains("SelfReferencingModule")
                    && e.Message.Contains("cannot reference itself"))
            )
                .IsTrue()
        )
    }

    [<Test>]
    member _.ValidateAsync_WithCircularDependency_ReturnsError() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>() |> ignore

        let! result = builder.ValidateAsync() |> Async.AwaitTask

        do! check(Assert.That(result.HasErrors).IsTrue())

        do! check(
            Assert.That(
                result.Errors.Any(fun e ->
                    e.Category = ValidationErrorCategory.Dependency
                    && (e.Message.Contains("Circular dependency") || e.Message.Contains("Dependency collision")))
            )
                .IsTrue()
        )
    }

    [<Test>]
    member _.BuildAsync_WithSelfReferencingModule_ThrowsValidationException() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<SelfReferencingModule>() |> ignore

        let mutable threw = false

        try
            let! _ = builder.BuildAsync() |> Async.AwaitTask
            ()
        with ex when isBaseException<PipelineValidationException> ex ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.BuildAsync_WithCircularDependency_ThrowsValidationException() = async {
        let builder = Pipeline.CreateBuilder()
        builder.Services.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>() |> ignore

        let mutable threw = false

        try
            let! _ = builder.BuildAsync() |> Async.AwaitTask
            ()
        with ex when isBaseException<PipelineValidationException> ex ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }
