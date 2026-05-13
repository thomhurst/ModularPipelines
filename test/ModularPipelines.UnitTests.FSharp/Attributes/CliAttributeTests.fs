namespace ModularPipelines.UnitTests.Attributes
open ModularPipelines.Helpers.Internal
open TUnit.Core
open ModularPipelines.Attributes
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations

type CliAttributeTests() =
    member private this.ModelProvider = CommandModelProvider()
    member private this.ArgumentBuilder = CommandArgumentBuilder()
    member private this.BuildArguments(optionsObject: obj) =
        let model = this.ModelProvider.GetCommandModel(optionsObject.GetType())
        this.ArgumentBuilder.BuildArguments(model, optionsObject)

    [<Test>]
    member _.CliCommand_Returns_Tool_And_SubCommands() = async {
        let attribute = new CliCommandAttribute("helm", "install");
        let parts: string array = attribute.GetAllParts();
        do! check(Assert.That<string array>(parts).IsEquivalentTo([| "helm"; "install" |]))
    }
    
    [<Test>]
    member _.CliCommand_Returns_Only_Tool_When_No_SubCommands() = async {
        let attribute = new CliCommandAttribute("helm");
        let parts: string array = attribute.GetAllParts();
        do! check(Assert.That<string array>(parts).IsEquivalentTo([| "helm" |]))
    }

    [<Test>]
    member _.CliCommand_Returns_Multiple_SubCommands() = async {
        let attribute = new CliCommandAttribute("kubectl", "get", "pods");
        let parts: string array = attribute.GetAllParts();
        do! check(Assert.That<string array>(parts).IsEquivalentTo([| "kubectl"; "get"; "pods" |]))
    }

    [<Test>]
    member _.CliFlag_Returns_Name_When_ShortForm_Not_Preferred() = async {
        let attribute = CliFlagAttribute("--debug")
        attribute.ShortForm <- "-d"
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.GetEffectiveName()), "--debug"))
    }

   