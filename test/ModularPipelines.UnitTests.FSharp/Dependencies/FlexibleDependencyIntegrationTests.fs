namespace ModularPipelines.UnitTests.FSharp.Dependencies

//open System
//open System.Collections.Concurrent
//open System.Collections.Generic
//open System.Threading
//open System.Threading.Tasks
//open Microsoft.Extensions.DependencyInjection
//open ModularPipelines.Attributes
//open ModularPipelines.Context
//open ModularPipelines.Enums
//open ModularPipelines.Extensions
//open ModularPipelines.Modules
//open ModularPipelines.TestHelpers
//open TUnit.Assertions
//open TUnit.Assertions.Extensions
//open TUnit.Assertions.FSharp.Operations
//open TUnit.Core

//module FlexibleDependencyIntegrationTestsData =
//    let executionOrderQueue = ConcurrentQueue<string>()

//    let clearExecutionOrder() = executionOrderQueue.Clear()

//    let getExecutionOrder() = executionOrderQueue.ToArray() |> Array.toList

//    let recordExecution moduleName = executionOrderQueue.Enqueue(moduleName)

//    [<AttributeUsage(AttributeTargets.Class, Inherited = true)>]
//    type CriticalAttribute() =
//        inherit Attribute()

//    [<ModuleTag("database")>]
//    type DatabaseModuleA() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof DatabaseModuleA)
//                return "DatabaseA"
//            }

//    [<ModuleTag("database")>]
//    type DatabaseModuleB() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof DatabaseModuleB)
//                return "DatabaseB"
//            }

//    [<ModuleTag("other")>]
//    type NonDatabaseModule() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof NonDatabaseModule)
//                return "NonDatabase"
//            }

//    [<DependsOnModulesWithTag("database")>]
//    type AfterDatabaseModule() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof AfterDatabaseModule)
//                return "AfterDatabase"
//            }

//    [<DependsOnModulesWithTag("nonexistent")>]
//    type ModuleDependingOnNonExistentTag() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof ModuleDependingOnNonExistentTag)
//                return "DependsOnNonExistent"
//            }

//    [<ModuleTag("database")>]
//    [<ModuleTag("slow")>]
//    [<ModuleTag("critical")>]
//    type ModuleWithMultipleTags() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof ModuleWithMultipleTags)
//                return "MultipleTags"
//            }

//    [<DependsOnModulesWithTag("slow")>]
//    type AfterSlowModule() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof AfterSlowModule)
//                return "AfterSlow"
//            }

//    [<ModuleCategory("infrastructure")>]
//    type InfrastructureModuleA() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof InfrastructureModuleA)
//                return "InfrastructureA"
//            }

//    [<ModuleCategory("infrastructure")>]
//    type InfrastructureModuleB() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof InfrastructureModuleB)
//                return "InfrastructureB"
//            }

//    [<ModuleCategory("build")>]
//    type BuildModule() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof BuildModule)
//                return "Build"
//            }

//    [<DependsOnModulesInCategory("infrastructure")>]
//    type AfterInfrastructureModule() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof AfterInfrastructureModule)
//                return "AfterInfrastructure"
//            }

//    [<DependsOnModulesInCategory("nonexistent")>]
//    type ModuleDependingOnNonExistentCategory() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof ModuleDependingOnNonExistentCategory)
//                return "DependsOnNonExistentCategory"
//            }

//    [<Critical>]
//    type CriticalModuleA() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof CriticalModuleA)
//                return "CriticalA"
//            }

//    [<Critical>]
//    type CriticalModuleB() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof CriticalModuleB)
//                return "CriticalB"
//            }

//    type NonCriticalModule() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof NonCriticalModule)
//                return "NonCritical"
//            }

//    [<Critical>]
//    type BaseCriticalModule() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof BaseCriticalModule)
//                return "BaseCritical"
//            }

//    type DerivedCriticalModule() =
//        inherit BaseCriticalModule()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof DerivedCriticalModule)
//                return "DerivedCritical"
//            }

//    [<DependsOnModulesWithAttribute<CriticalAttribute>>]
//    type AfterCriticalModule() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof AfterCriticalModule)
//                return "AfterCritical"
//            }

//    type ModuleWithOverrideTags() =
//        inherit Module<string>()
//        override _.Tags = HashSet<string>([ "database"; "override-tag" ]) :> IReadOnlySet<string>
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof ModuleWithOverrideTags)
//                return "OverrideTags"
//            }

//    type ModuleWithOverrideCategory() =
//        inherit Module<string>()
//        override _.Category = "infrastructure"
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof ModuleWithOverrideCategory)
//                return "OverrideCategory"
//            }

//    type PlainModule() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof PlainModule)
//                return "Plain"
//            }

//    [<DependsOnModulesWithTag("database")>]
//    [<DependsOnModulesInCategory("infrastructure")>]
//    [<DependsOnCriticalModules>]
//    type ModuleWithMultipleFlexibleDependencies() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof ModuleWithMultipleFlexibleDependencies)
//                return "MultipleFlexibleDeps"
//            }

//    [<ModuleTag("database")>]
//    [<DependsOnModulesWithTag("database")>]
//    [<ModuleTag("phase1")>]
//    type AfterDatabaseModuleWithPhase1Tag() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof AfterDatabaseModuleWithPhase1Tag)
//                return "AfterDbWithPhase1"
//            }

//    [<DependsOnModulesWithTag("phase1")>]
//    type AfterPhase1Module() =
//        inherit Module<string>()
//        override _.ExecuteAsync(_: IModuleContext, _: CancellationToken) =
//            task {
//                do! Task.Yield()
//                recordExecution (nameof AfterPhase1Module)
//                return "AfterPhase1"
//            }

//[<NotInParallel(nameof FlexibleDependencyIntegrationTests)>]
//type FlexibleDependencyIntegrationTests() =
//    inherit TestBase()

//    [<Before(HookType.Test)>]
//    member _.Setup() =
//        FlexibleDependencyIntegrationTestsData.clearExecutionOrder()

//    [<Test>]
//    member _.DependsOnModulesWithTag_WaitsForTaggedModules() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.DatabaseModuleA>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.DatabaseModuleB>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.NonDatabaseModule>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterDatabaseModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let afterDbIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterDatabaseModule)
//        let dbAIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.DatabaseModuleA)
//        let dbBIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.DatabaseModuleB)

//        do! check(Assert.That(afterDbIndex).IsGreaterThan(dbAIndex))
//        do! check(Assert.That(afterDbIndex).IsGreaterThan(dbBIndex))
//    }

//    [<Test>]
//    member _.DependsOnModulesWithTag_NoMatchingModules_StillSucceeds() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.NonDatabaseModule>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.ModuleDependingOnNonExistentTag>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
//    }

//    [<Test>]
//    member _.DependsOnModulesWithTag_MultipleTagsOnModule_MatchesCorrectly() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.ModuleWithMultipleTags>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterSlowModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let multiTagIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.ModuleWithMultipleTags)
//        let afterSlowIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterSlowModule)

//        do! check(Assert.That(afterSlowIndex).IsGreaterThan(multiTagIndex))
//    }

//    [<Test>]
//    member _.DependsOnModulesInCategory_WaitsForCategorizedModules() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.InfrastructureModuleA>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.InfrastructureModuleB>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.BuildModule>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterInfrastructureModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let afterInfraIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterInfrastructureModule)
//        let infraAIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.InfrastructureModuleA)
//        let infraBIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.InfrastructureModuleB)

//        do! check(Assert.That(afterInfraIndex).IsGreaterThan(infraAIndex))
//        do! check(Assert.That(afterInfraIndex).IsGreaterThan(infraBIndex))
//    }

//    [<Test>]
//    member _.DependsOnModulesInCategory_NoMatchingModules_StillSucceeds() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.BuildModule>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.ModuleDependingOnNonExistentCategory>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
//    }

//    [<Test>]
//    member _.DependsOnModulesWithAttribute_WaitsForAttributedModules() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.CriticalModuleA>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.CriticalModuleB>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.NonCriticalModule>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterCriticalModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let afterCriticalIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterCriticalModule)
//        let criticalAIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.CriticalModuleA)
//        let criticalBIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.CriticalModuleB)

//        do! check(Assert.That(afterCriticalIndex).IsGreaterThan(criticalAIndex))
//        do! check(Assert.That(afterCriticalIndex).IsGreaterThan(criticalBIndex))
//    }

//    [<Test>]
//    member _.DependsOnModulesWithAttribute_InheritedAttribute_IsRecognized() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.DerivedCriticalModule>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterCriticalModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let derivedIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.DerivedCriticalModule)
//        let afterCriticalIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterCriticalModule)

//        do! check(Assert.That(afterCriticalIndex).IsGreaterThan(derivedIndex))
//    }

//    [<Test>]
//    member _.DependsOnModulesWithAttribute_NoMatchingModules_StillSucceeds() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.NonCriticalModule>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterCriticalModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))
//    }

//    [<Test>]
//    member _.ModuleWithOverrideTags_IsRecognizedByTagDependency() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.ModuleWithOverrideTags>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterDatabaseModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let overrideIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.ModuleWithOverrideTags)
//        let afterDbIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterDatabaseModule)

//        do! check(Assert.That(afterDbIndex).IsGreaterThan(overrideIndex))
//    }

//    [<Test>]
//    member _.ModuleWithOverrideCategory_IsRecognizedByCategoryDependency() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.ModuleWithOverrideCategory>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterInfrastructureModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let overrideIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.ModuleWithOverrideCategory)
//        let afterInfraIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterInfrastructureModule)

//        do! check(Assert.That(afterInfraIndex).IsGreaterThan(overrideIndex))
//    }

//    [<Test>]
//    member _.ModuleWithRegistrationTags_IsRecognizedByTagDependency() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .ConfigureServices(fun _ services ->
//                    services.AddModule<FlexibleDependencyIntegrationTestsData.PlainModule>().WithTags("database") |> ignore)
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterDatabaseModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let plainIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.PlainModule)
//        let afterDbIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterDatabaseModule)

//        do! check(Assert.That(afterDbIndex).IsGreaterThan(plainIndex))
//    }

//    [<Test>]
//    member _.ModuleWithRegistrationCategory_IsRecognizedByCategoryDependency() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .ConfigureServices(fun _ services ->
//                    services.AddModule<FlexibleDependencyIntegrationTestsData.PlainModule>().WithCategory("infrastructure") |> ignore)
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterInfrastructureModule>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let plainIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.PlainModule)
//        let afterInfraIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterInfrastructureModule)

//        do! check(Assert.That(afterInfraIndex).IsGreaterThan(plainIndex))
//    }

//    [<Test>]
//    member _.ModuleWithBothAttributeAndRegistrationTags_MergesTags() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .ConfigureServices(fun _ services ->
//                    services.AddModule<FlexibleDependencyIntegrationTestsData.DatabaseModuleA>().WithTags("slow") |> ignore)
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterSlowModule>>
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let dbAIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.DatabaseModuleA)
//        let afterSlowIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterSlowModule)

//        do! check(Assert.That(afterSlowIndex).IsGreaterThan(dbAIndex))
//    }

//    [<Test>]
//    member _.CombinedDependencies_ModuleWithMultipleFlexibleDependencies() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.DatabaseModuleA>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.InfrastructureModuleA>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.CriticalModuleA>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.ModuleWithMultipleFlexibleDependencies>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let combinedIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.ModuleWithMultipleFlexibleDependencies)
//        let dbAIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.DatabaseModuleA)
//        let infraAIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.InfrastructureModuleA)
//        let criticalAIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.CriticalModuleA)

//        do! check(Assert.That(combinedIndex).IsGreaterThan(dbAIndex))
//        do! check(Assert.That(combinedIndex).IsGreaterThan(infraAIndex))
//        do! check(Assert.That(combinedIndex).IsGreaterThan(criticalAIndex))
//    }

//    [<Test>]
//    member _.ChainedFlexibleDependencies_ExecuteInCorrectOrder() = async {
//        let! result =
//            TestPipelineHostBuilder.Create()
//                .AddModule<FlexibleDependencyIntegrationTestsData.DatabaseModuleA>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterDatabaseModuleWithPhase1Tag>()
//                .AddModule<FlexibleDependencyIntegrationTestsData.AfterPhase1Module>()
//                .ExecutePipelineAsync()
//            |> Async.AwaitTask

//        do! check(Assert.That(result.Status).IsEqualTo(Status.Successful))

//        let order = FlexibleDependencyIntegrationTestsData.getExecutionOrder()
//        let dbAIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.DatabaseModuleA)
//        let afterDbIndex = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterDatabaseModuleWithPhase1Tag)
//        let afterPhase1Index = order.IndexOf(nameof FlexibleDependencyIntegrationTestsData.AfterPhase1Module)

//        do! check(Assert.That(afterDbIndex).IsGreaterThan(dbAIndex))
//        do! check(Assert.That(afterPhase1Index).IsGreaterThan(afterDbIndex))
//    }
