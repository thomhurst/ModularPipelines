namespace ModularPipelines.UnitTests.Attributes

open ModularPipelines.Helpers.Internal
open TUnit.Core
open ModularPipelines.Attributes
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations

[<CLIMutable>]
type private TestCliOptionsWithFlag =
    {
        [<CliFlag("--debug")>]
        Debug: System.Nullable<bool>
    }

[<CLIMutable>]
type private TestCliOptionsWithOption =
    {
        [<CliOption("--namespace")>]
        Namespace: string
    }

[<CLIMutable>]
type private TestCliOptionsWithEqualsSeparator =
    {
        [<CliOption("--set", Format = OptionFormat.EqualsSeparated)>]
        Set: string
    }

[<CLIMutable>]
type private TestCliOptionsWithMultipleValues =
    {
        [<CliOption("--values", AllowMultiple = true)>]
        Values: string array
    }

[<CLIMutable>]
type private TestCliOptionsWithArgumentAfterOptions =
    {
        [<CliArgument(0)>]
        ReleaseName: string

        [<CliFlag("--debug")>]
        Debug: System.Nullable<bool>
    }

[<CLIMutable>]
type private TestCliOptionsWithArgumentBeforeOptions =
    {
        [<CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)>]
        Path: string

        [<CliFlag("--debug")>]
        Debug: System.Nullable<bool>
    }

[<CLIMutable>]
type private TestCliOptionsWithOptionalArgument =
    {
        [<CliArgument(0)>]
        ReleaseName: string

        [<CliFlag("--debug")>]
        Debug: System.Nullable<bool>
    }

[<CLIMutable>]
type private TestCliOptionsWithMultipleArguments =
    {
        [<CliArgument(0)>]
        ReleaseName: string

        [<CliArgument(1)>]
        ChartReference: string
    }

[<CLIMutable>]
type private TestCliOptionsComplete =
    {
        [<CliArgument(0)>]
        ReleaseName: string

        [<CliArgument(1)>]
        ChartReference: string

        [<CliFlag("--debug")>]
        Debug: System.Nullable<bool>

        [<CliOption("--namespace")>]
        Namespace: string

        [<CliOption("--set", Format = OptionFormat.EqualsSeparated, AllowMultiple = true)>]
        Set: string array
    }

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

   