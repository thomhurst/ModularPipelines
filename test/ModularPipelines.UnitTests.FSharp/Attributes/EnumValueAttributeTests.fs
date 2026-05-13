namespace ModularPipelines.UnitTests.FSharp.Attributes

open ModularPipelines.Attributes
open ModularPipelines.Helpers.Internal
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type Number =
    | [<EnumValue("1")>] One = 0
    | [<EnumValue("2")>] Two = 1
    | [<EnumValue("3")>] Three = 2

[<CLIMutable>]
type private NumberWrapper =
    {
        [<CliOption("--number")>]
        Number: Number
    }

type EnumValueAttributeTests() =
    member private _.ModelProvider = CommandModelProvider()
    member private _.ArgumentBuilder = CommandArgumentBuilder()
    member private this.BuildArguments(optionsObject: obj) =
        let model = this.ModelProvider.GetCommandModel(optionsObject.GetType())
        this.ArgumentBuilder.BuildArguments(model, optionsObject)

    [<Test>]
    [<Arguments(Number.One, "1")>]
    [<Arguments(Number.Two, "2")>]
    [<Arguments(Number.Three, "3")>]
    member this.Can_Parse_EnumValueAttribute(number: Number, expected: string) = async {
        let options = { Number = number }

        let list = this.BuildArguments(options)
        do! check(Assert.That(list |> Seq.contains "--number").IsTrue())
        do! check(Assert.That(list |> Seq.contains expected).IsTrue())
        do! check(Assert.That((list |> Seq.toArray) = [| "--number"; expected |]).IsTrue())
    }
