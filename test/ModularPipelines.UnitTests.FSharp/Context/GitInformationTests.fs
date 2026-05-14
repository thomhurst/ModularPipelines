namespace ModularPipelines.UnitTests.FSharp.Context

open System
open ModularPipelines.Context
open ModularPipelines.Git.Extensions
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type GitInformationTests() =
    inherit TestBase()

    [<Test>]
    member this.Can_Send_Request_With_String_To_Request_Implicit_Conversion() = async {
        let! context = this.GetService<IPipelineHookContext>() |> Async.AwaitTask
        let branch = context.Git().Information.BranchName

        do! check(Assert.That(branch).IsNotNull())
        do! check(Assert.That(not (String.IsNullOrWhiteSpace(branch))).IsTrue())
    }
