namespace ModularPipelines.UnitTests.FSharp.Attributes

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

    [<Test>]
    member _.CliFlag_Returns_ShortForm_When_Preferred() = async {
        let attribute = CliFlagAttribute("--debug")
        attribute.ShortForm <- "-d"
        attribute.PreferShortForm <- true
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.GetEffectiveName()), "-d"))
    }

    [<Test>]
    member _.CliFlag_Returns_Name_When_ShortForm_Null_And_Preferred() = async {
        let attribute = CliFlagAttribute("--debug")
        attribute.PreferShortForm <- true
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.GetEffectiveName()), "--debug"))
    }

    [<Test>]
    [<Arguments(OptionFormat.SpaceSeparated, " ")>]
    [<Arguments(OptionFormat.EqualsSeparated, "=")>]
    [<Arguments(OptionFormat.ColonSeparated, ":")>]
    [<Arguments(OptionFormat.NoSeparator, "")>]
    member _.CliOption_GetSeparator_Returns_Correct_Separator(format: OptionFormat, expected: string) = async {
        let attribute = CliOptionAttribute("--namespace")
        attribute.Format <- format
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.GetSeparator()), expected))
    }

    [<Test>]
    member _.CliOption_CustomSeparator_Overrides_Format() = async {
        let attribute = CliOptionAttribute("--namespace")
        attribute.Format <- OptionFormat.SpaceSeparated
        attribute.CustomSeparator <- "::"
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.GetSeparator()), "::"))
    }

    [<Test>]
    member _.CliOption_Returns_Name_When_ShortForm_Not_Preferred() = async {
        let attribute = CliOptionAttribute("--namespace")
        attribute.ShortForm <- "-n"
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.GetEffectiveName()), "--namespace"))
    }

    [<Test>]
    member _.CliOption_Returns_ShortForm_When_Preferred() = async {
        let attribute = CliOptionAttribute("--namespace")
        attribute.ShortForm <- "-n"
        attribute.PreferShortForm <- true
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(attribute.GetEffectiveName()), "-n"))
    }

    [<Test>]
    member _.CliArgument_Defaults_To_AfterOptions_Placement() = async {
        let attribute = CliArgumentAttribute(0)
        do! check(Assert.That(attribute.Placement = ArgumentPlacement.AfterOptions).IsTrue())
    }

    [<Test>]
    member _.CliArgument_Position_Is_Set_Correctly() = async {
        let attribute = CliArgumentAttribute(2)
        do! check(Assert.That(attribute.Position = 2).IsTrue())
    }

    [<Test>]
    member this.Parser_Handles_CliFlag() = async {
        let options = { Debug = System.Nullable true }
        let list = this.BuildArguments(options)
        do! check(Assert.That<string array>(list |> Seq.toArray).IsEquivalentTo([| "--debug" |]))
    }

    [<Test>]
    member this.Parser_Omits_CliFlag_When_False() = async {
        let options = { Debug = System.Nullable false }
        let list = this.BuildArguments(options)
        do! check(Assert.That((Seq.length list) = 0).IsTrue())
    }

    [<Test>]
    member this.Parser_Omits_CliFlag_When_Null() = async {
        let options = { Debug = System.Nullable() }
        let list = this.BuildArguments(options)
        do! check(Assert.That((Seq.length list) = 0).IsTrue())
    }

    [<Test>]
    member this.Parser_Handles_CliOption_With_Space_Separator() = async {
        let options = { Namespace = "default" }
        let list = this.BuildArguments(options)
        do! check(Assert.That<string array>(list |> Seq.toArray).IsEquivalentTo([| "--namespace"; "default" |]))
    }

    [<Test>]
    member this.Parser_Handles_CliOption_With_Equals_Separator() = async {
        let options = { Set = "key=value" }
        let list = this.BuildArguments(options)
        do! check(Assert.That<string array>(list |> Seq.toArray).IsEquivalentTo([| "--set=key=value" |]))
    }

    [<Test>]
    member this.Parser_Handles_CliOption_With_Multiple_Values() = async {
        let options = { Values = [| "file1.yaml"; "file2.yaml" |] }
        let list = this.BuildArguments(options)
        do! check(Assert.That<string array>(list |> Seq.toArray).IsEquivalentTo([| "--values"; "file1.yaml"; "--values"; "file2.yaml" |]))
    }

    [<Test>]
    member this.Parser_Handles_CliArgument_After_Options() = async {
        let options = { ReleaseName = "myrelease"; Debug = System.Nullable true }
        let list = this.BuildArguments(options)
        do! check(Assert.That<string array>(list |> Seq.toArray).IsEquivalentTo([| "--debug"; "myrelease" |]))
    }

    [<Test>]
    member this.Parser_Handles_CliArgument_Before_Options() = async {
        let options = { Path = "/some/path"; Debug = System.Nullable true }
        let list = this.BuildArguments(options)
        do! check(Assert.That<string array>(list |> Seq.toArray).IsEquivalentTo([| "/some/path"; "--debug" |]))
    }

    [<Test>]
    member this.Parser_Omits_Null_CliArgument() = async {
        let options = { ReleaseName = null; Debug = System.Nullable true }
        let list = this.BuildArguments(options)
        do! check(Assert.That<string array>(list |> Seq.toArray).IsEquivalentTo([| "--debug" |]))
    }

    [<Test>]
    member this.Parser_Orders_Multiple_Arguments_By_Position() = async {
        let options = { ReleaseName = "myrelease"; ChartReference = "bitnami/nginx" }
        let list = this.BuildArguments(options)
        do! check(Assert.That<string array>(list |> Seq.toArray).IsEquivalentTo([| "myrelease"; "bitnami/nginx" |]))
    }

    [<Test>]
    member this.Parser_Handles_Mixed_Flags_Options_And_Arguments() = async {
        let options =
            {
                ReleaseName = "myrelease"
                ChartReference = "bitnami/nginx"
                Namespace = "production"
                Debug = System.Nullable true
                Set = [| "key1=val1"; "key2=val2" |]
            }

        let list = this.BuildArguments(options)

        let firstItem = list |> Seq.tryHead

        do! check(Assert.That(firstItem.IsSome).IsTrue())
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(firstItem.Value), "--debug"))
        do! check(Assert.That(list |> Seq.contains "--namespace").IsTrue())
        do! check(Assert.That(list |> Seq.contains "production").IsTrue())
        do! check(Assert.That(list |> Seq.contains "--set=key1=val1").IsTrue())
        do! check(Assert.That(list |> Seq.contains "--set=key2=val2").IsTrue())
        do! check(Assert.That(list |> Seq.contains "myrelease").IsTrue())
        do! check(Assert.That(list |> Seq.contains "bitnami/nginx").IsTrue())
    }

   
