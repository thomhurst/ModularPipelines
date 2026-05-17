namespace ModularPipelines.UnitTests.FSharp.Execution

open System.Collections.Generic
open ModularPipelines.Extensions
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

module private NotInParallelTestsWithMultipleConstraintKeysData =
    let tracker = NotInParallelTracker()

[<ModularPipelines.Attributes.NotInParallel("A")>]
type Module1() =
    inherit NotInParallelTestModule()
    override _.Tracker = NotInParallelTestsWithMultipleConstraintKeysData.tracker
    override _.ConflictingModuleNames = ["Module2"] :> IEnumerable<string>

[<ModularPipelines.Attributes.NotInParallel("A", "B")>]
type Module2() =
    inherit NotInParallelTestModule()
    override _.Tracker = NotInParallelTestsWithMultipleConstraintKeysData.tracker
    override _.ConflictingModuleNames = ["Module1"; "Module3"] :> IEnumerable<string>

[<ModularPipelines.Attributes.NotInParallel("B", "C")>]
type Module3() =
    inherit NotInParallelTestModule()
    override _.Tracker = NotInParallelTestsWithMultipleConstraintKeysData.tracker
    override _.ConflictingModuleNames = ["Module2"] :> IEnumerable<string>

[<ModularPipelines.Attributes.NotInParallel("D")>]
type Module4() =
    inherit NotInParallelTestModule()
    override _.Tracker = NotInParallelTestsWithMultipleConstraintKeysData.tracker
    override _.ConflictingModuleNames = [] :> IEnumerable<string>
    override _.DelayMs = 50

type NotInParallelTestsWithMultipleConstraintKeys() =
    inherit TestBase()

    [<Test>]
    member _.NotInParallel_If_Any_Modules_Executing_With_Any_Of_Same_ConstraintKey() = async {
        NotInParallelTestsWithMultipleConstraintKeysData.tracker.Reset()

        let! _ = TestPipelineHostBuilder.Create()
                    .AddModule<Module1>()
                    .AddModule<Module2>()
                    .AddModule<Module3>()
                    .AddModule<Module4>()
                    .ExecutePipelineAsync()
                |> Async.AwaitTask
        do! check(Assert.That<string>(NotInParallelTestsWithMultipleConstraintKeysData.tracker.Violations).IsEmpty())
    }
