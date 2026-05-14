namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System
open System.Threading
open System.Threading.Tasks
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Enums
open ModularPipelines.Exceptions
open ModularPipelines.Extensions
open ModularPipelines.Models
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module DynamicDependencyDeclarationTestsData =
    type BaseModule() =
        inherit SimpleTestModule<string>()
        override _.Result = "base"

    type OptionalDependencyModule() =
        inherit SimpleTestModule<string>()
        override _.Result = "optional"

    type LazyModule() =
        inherit SimpleTestModule<string>()
        override _.Result = "lazy"

    type ConditionalModule() =
        inherit SimpleTestModule<string>()
        override _.Result = "conditional"

    type ModuleWithProgrammaticDependency() =
        inherit Module<string>()

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps.DependsOn<BaseModule>() |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "programmatic"
            }

    type ModuleWithOptionalDependency() =
        inherit Module<string>()

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps.DependsOnOptional<OptionalDependencyModule>() |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "optional-dep"
            }

    type ModuleWithActiveConditionalDependency() =
        inherit Module<string>()

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps.DependsOnIf<ConditionalModule>(true) |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "conditional-active"
            }

    type ModuleWithInactiveConditionalDependency() =
        inherit Module<string>()

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps.DependsOnIf<ConditionalModule>(false) |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "conditional-inactive"
            }

    type ModuleWithPredicateConditionalDependency() =
        inherit Module<string>()

        static member val ShouldDependOnConditional = true with get, set

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps.DependsOnIf<ConditionalModule>(fun () -> ModuleWithPredicateConditionalDependency.ShouldDependOnConditional)
            |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "predicate-conditional"
            }

    type ModuleWithLazyDependency() =
        inherit Module<string>()

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps.DependsOnLazy<LazyModule>() |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "lazy-dep"
            }

    [<DependsOn(typeof<BaseModule>)>]
    type ModuleWithBothAttributeAndProgrammaticDependencies() =
        inherit Module<string>()

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps.DependsOnOptional<OptionalDependencyModule>() |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "combined"
            }

    type ModuleWithChainedDependencies() =
        inherit Module<string>()

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps
                .DependsOn<BaseModule>()
                .DependsOnOptional<OptionalDependencyModule>()
                .DependsOnLazy<LazyModule>()
            |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "chained"
            }

    type ModuleWithMissingRequiredDependency() =
        inherit Module<string>()

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps.DependsOn<BaseModule>() |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "missing-dep"
            }

    type ModuleWithTypeDependency() =
        inherit Module<string>()

        override _.DeclareDependencies(deps: IDependencyDeclaration) =
            deps.DependsOn(typeof<BaseModule>) |> ignore

        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
            task {
                do! Task.Yield()
                return "type-dep"
            }

type DynamicDependencyDeclarationTests() =
    inherit TestBase()

    [<Test>]
    member _.Programmatic_Required_Dependency_Works_When_Registered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.BaseModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithProgrammaticDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Programmatic_Required_Dependency_Throws_When_Not_Registered() = async {
        let mutable threw = false

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithMissingRequiredDependency>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with _ ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Programmatic_Type_Dependency_Works() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.BaseModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithTypeDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Optional_Dependency_Works_When_Registered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.OptionalDependencyModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithOptionalDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Optional_Dependency_Does_Not_Fail_When_Not_Registered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithOptionalDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Conditional_Dependency_Works_When_Condition_True_And_Registered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.ConditionalModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithActiveConditionalDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Conditional_Dependency_Throws_When_Condition_True_And_Not_Registered() = async {
        let mutable threw = false

        try
            let! _ =
                TestPipelineHostBuilder.Create()
                    .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithActiveConditionalDependency>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
            ()
        with _ ->
            threw <- true

        do! check(Assert.That(threw).IsTrue())
    }

    [<Test>]
    member _.Conditional_Dependency_Not_Added_When_Condition_False() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithInactiveConditionalDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    [<NotInParallel(Order = 1)>]
    member _.Conditional_Predicate_Dependency_Works_When_Predicate_Returns_True() = async {
        DynamicDependencyDeclarationTestsData.ModuleWithPredicateConditionalDependency.ShouldDependOnConditional <- true

        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.ConditionalModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithPredicateConditionalDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    [<NotInParallel(Order = 2)>]
    member _.Conditional_Predicate_Dependency_Not_Added_When_Predicate_Returns_False() = async {
        DynamicDependencyDeclarationTestsData.ModuleWithPredicateConditionalDependency.ShouldDependOnConditional <- false

        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithPredicateConditionalDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Lazy_Dependency_Does_Not_Fail_When_Not_Registered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithLazyDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Lazy_Dependency_Works_When_Registered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.LazyModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithLazyDependency>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Combined_Attribute_And_Programmatic_Dependencies_Work() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.BaseModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.OptionalDependencyModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithBothAttributeAndProgrammaticDependencies>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Combined_Dependencies_Work_With_Only_Attribute_Dependency_Registered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.BaseModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithBothAttributeAndProgrammaticDependencies>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Chained_Dependency_Declarations_Work() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.BaseModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.OptionalDependencyModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.LazyModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithChainedDependencies>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.Chained_Dependency_Declarations_Work_With_Only_Required_Registered() = async {
        let! pipelineSummary =
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicDependencyDeclarationTestsData.BaseModule>()
                .AddModule<DynamicDependencyDeclarationTestsData.ModuleWithChainedDependencies>()
                .ExecutePipelineAsync()
            |> Async.AwaitTask

        do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
    }

    [<Test>]
    member _.DependencyDeclaration_DependsOn_Returns_Same_Instance_For_Chaining() = async {
        let declaration = DependencyDeclaration()
        let result = declaration.DependsOn<DynamicDependencyDeclarationTestsData.BaseModule>()
        do! check(Assert.That(Object.ReferenceEquals(result, declaration)).IsTrue())
    }

    [<Test>]
    member _.DependencyDeclaration_DependsOnOptional_Returns_Same_Instance_For_Chaining() = async {
        let declaration = DependencyDeclaration()
        let result = declaration.DependsOnOptional<DynamicDependencyDeclarationTestsData.BaseModule>()
        do! check(Assert.That(Object.ReferenceEquals(result, declaration)).IsTrue())
    }

    [<Test>]
    member _.DependencyDeclaration_DependsOnIf_Returns_Same_Instance_For_Chaining() = async {
        let declaration = DependencyDeclaration()
        let result = declaration.DependsOnIf<DynamicDependencyDeclarationTestsData.BaseModule>(true)
        do! check(Assert.That(Object.ReferenceEquals(result, declaration)).IsTrue())
    }

    [<Test>]
    member _.DependencyDeclaration_DependsOnLazy_Returns_Same_Instance_For_Chaining() = async {
        let declaration = DependencyDeclaration()
        let result = declaration.DependsOnLazy<DynamicDependencyDeclarationTestsData.BaseModule>()
        do! check(Assert.That(Object.ReferenceEquals(result, declaration)).IsTrue())
    }

    [<Test>]
    member _.DependencyDeclaration_Throws_For_Non_Module_Type() = async {
        let declaration = DependencyDeclaration()
        let mutable message = null

        try
            declaration.DependsOn(typeof<string>) |> ignore
        with :? InvalidModuleTypeException as ex ->
            message <- ex.Message

        do! check(Assert.That(message <> null).IsTrue())
        do! check(Assert.That(message.Contains("is not a Module")).IsTrue())
    }

    [<Test>]
    member _.DependencyDeclaration_Required_Has_Correct_DependencyType() = async {
        let declaration = DependencyDeclaration()
        declaration.DependsOn<DynamicDependencyDeclarationTestsData.BaseModule>() |> ignore
        let deps = declaration.Dependencies
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(deps.Count), 1))
        do! check(Assert.That(deps.[0].Kind).IsEqualTo(DependencyType.Required))
        do! check(Assert.That(deps.[0].IsOptional).IsEqualTo(false))
    }

    [<Test>]
    member _.DependencyDeclaration_Optional_Has_Correct_DependencyType() = async {
        let declaration = DependencyDeclaration()
        declaration.DependsOnOptional<DynamicDependencyDeclarationTestsData.BaseModule>() |> ignore
        let deps = declaration.Dependencies
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(deps.Count), 1))
        do! check(Assert.That(deps.[0].Kind).IsEqualTo(DependencyType.Optional))
        do! check(Assert.That(deps.[0].IsOptional).IsEqualTo(true))
    }

    [<Test>]
    member _.DependencyDeclaration_Lazy_Has_Correct_DependencyType() = async {
        let declaration = DependencyDeclaration()
        declaration.DependsOnLazy<DynamicDependencyDeclarationTestsData.BaseModule>() |> ignore
        let deps = declaration.Dependencies

        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(deps.Count), 1))
        do! check(Assert.That(deps.[0].Kind).IsEqualTo(DependencyType.Lazy))
        do! check(Assert.That(deps.[0].IsOptional).IsEqualTo(true))
    }

    [<Test>]
    member _.DependencyDeclaration_Conditional_Has_Correct_DependencyType() = async {
        let declaration = DependencyDeclaration()
        declaration.DependsOnIf<DynamicDependencyDeclarationTestsData.BaseModule>(true) |> ignore
        let deps = declaration.Dependencies

        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(deps.Count), 1))
        do! check(Assert.That(deps.[0].Kind).IsEqualTo(DependencyType.Conditional))
        do! check(Assert.That(deps.[0].IsOptional).IsEqualTo(false))
    }

    [<Test>]
    member _.DependencyDeclaration_Conditional_False_Does_Not_Add_Dependency() = async {
        let declaration = DependencyDeclaration()
        declaration.DependsOnIf<DynamicDependencyDeclarationTestsData.BaseModule>(false) |> ignore
        let deps = declaration.Dependencies
        do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(deps.Count), 0))
    }
