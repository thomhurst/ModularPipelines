namespace ModularPipelines.UnitTests.FSharp.Execution

open System.Collections.Generic
open ModularPipelines.Extensions
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module private NotInParallelTestsWithConstraintKeysData =
    let tracker = NotInParallelTracker()

[<ModularPipelines.Attributes.NotInParallel("A")>]
type ModuleWithAConstraintKey1() =
    inherit NotInParallelTestModule()
    override _.Tracker = NotInParallelTestsWithConstraintKeysData.tracker
    override _.ConflictingModuleNames = ["ModuleWithAConstraintKey2"] :> IEnumerable<string>

[<ModularPipelines.Attributes.NotInParallel("A")>]
type ModuleWithAConstraintKey2() =
    inherit NotInParallelTestModule()
    override _.Tracker = NotInParallelTestsWithConstraintKeysData.tracker
    override _.ConflictingModuleNames = ["ModuleWithAConstraintKey1"] :> IEnumerable<string>

[<ModularPipelines.Attributes.NotInParallel("B")>]
type ModuleWithBConstraintKey1() =
    inherit NotInParallelTestModule()
    override _.Tracker = NotInParallelTestsWithConstraintKeysData.tracker
    override _.ConflictingModuleNames = ["ModuleWithBConstraintKey2"] :> IEnumerable<string>

[<ModularPipelines.Attributes.NotInParallel("B")>]
type ModuleWithBConstraintKey2() =
    inherit NotInParallelTestModule()
    override _.Tracker = NotInParallelTestsWithConstraintKeysData.tracker
    override _.ConflictingModuleNames = ["ModuleWithBConstraintKey1"] :> IEnumerable<string>

type NotInParallelTestsWithConstraintKeys() =
    inherit TestBase()

    [<Test>]
    member _.NotInParallel_If_Same_ConstraintKey() = async {
        NotInParallelTestsWithConstraintKeysData.tracker.Reset()

        let! _ = TestPipelineHostBuilder.Create()
                    .AddModule<ModuleWithAConstraintKey1>()
                    .AddModule<ModuleWithAConstraintKey2>()
                    .AddModule<ModuleWithBConstraintKey1>()
                    .AddModule<ModuleWithBConstraintKey2>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask

        do! check(Assert.That<string>(NotInParallelTestsWithConstraintKeysData.tracker.Violations).IsEmpty())
    }
