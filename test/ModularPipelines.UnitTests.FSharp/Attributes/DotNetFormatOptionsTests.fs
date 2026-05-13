namespace ModularPipelines.UnitTests.FSharp.Attributes

open ModularPipelines.DotNet.Options
open ModularPipelines.Helpers.Internal
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type DotNetFormatOptionsTests() =
    member private _.ModelProvider = CommandModelProvider()
    member private _.ArgumentBuilder = CommandArgumentBuilder()
    member private this.BuildArguments(optionsObject: obj) =
        let model = this.ModelProvider.GetCommandModel(optionsObject.GetType())
        this.ArgumentBuilder.BuildArguments(model, optionsObject)

    [<Test>]
    member this.ExcludeDiagnostics_Passes_Each_Id_Separately() = async {
        let options = DotNetFormatOptions()
        options.ExcludeDiagnostics <- [| "CS0246"; "CS1503" |]

        let args = this.BuildArguments(options)

        do! check(Assert.That((args |> Seq.toArray) = [|
            "--exclude-diagnostics"; "CS0246"
            "--exclude-diagnostics"; "CS1503"
        |]).IsTrue())
    }
