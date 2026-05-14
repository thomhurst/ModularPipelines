namespace ModularPipelines.UnitTests.FSharp.Context

open System
open System.Collections.ObjectModel
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Options
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

[<CliTool("mytool")>]
[<CliCommand("mytool", "sub", "command")>]
type private TestAttributeOptions() =
    inherit CommandLineToolOptions()

    [<CliFlag("--force")>]
    member val Force = Nullable<bool>() with get, set

    [<CliOption("--output")>]
    member val Output: string = null with get, set

    override this.``<Clone>$``() =
        let clonedOptions = TestAttributeOptions()
        clonedOptions.Force <- this.Force
        clonedOptions.Output <- this.Output
        clonedOptions :> CommandLineToolOptions

[<CliTool("processor")>]
[<CliCommand("processor")>]
type private TestPositionalOptions() =
    inherit CommandLineToolOptions()

    [<CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)>]
    member val FilePath: string = null with get, set

    [<CliArgument(1, Placement = ArgumentPlacement.AfterOptions)>]
    member val ConfigPath: string = null with get, set

    override this.``<Clone>$``() =
        let clonedOptions = TestPositionalOptions()
        clonedOptions.FilePath <- this.FilePath
        clonedOptions.ConfigPath <- this.ConfigPath
        clonedOptions :> CommandLineToolOptions

type CommandLineBuilderTests() =
    inherit TestBase()

    [<Test>]
    member this.Build_FromGenericOptions_ReturnsCorrectCommandLine() = async {
        let! builder = this.GetService<ICommandLineBuilder>() |> Async.AwaitTask
        let options = GenericCommandLineToolOptions("echo", Arguments = [| "hello"; "world" |])
        let result = builder.Build(options)

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.Tool), "echo"))
        do! check(Assert.That((result.Arguments |> Seq.toArray) = [| "hello"; "world" |]).IsTrue())
    }

    [<Test>]
    member this.Build_FromGenericOptions_WithRunSettings_AddsDoubleDash() = async {
        let! builder = this.GetService<ICommandLineBuilder>() |> Async.AwaitTask

        let options =
            GenericCommandLineToolOptions(
                "dotnet",
                Arguments = [| "test" |],
                RunSettings = [| "--filter"; "Category=Unit" |]
            )

        let result = builder.Build(options)

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.Tool), "dotnet"))
        do! check(Assert.That((result.Arguments |> Seq.toArray) = [| "test"; "--"; "--filter"; "Category=Unit" |]).IsTrue())
    }

    [<Test>]
    member this.Build_FromAttributeBasedOptions_ResolvesToolAndSubcommands() = async {
        let! builder = this.GetService<ICommandLineBuilder>() |> Async.AwaitTask
        let options = TestAttributeOptions(Force = Nullable true, Output = "/path/to/output")
        let result = builder.Build(options)
        let arguments = result.Arguments |> Seq.toArray

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.Tool), "mytool"))
        do! check(Assert.That(arguments |> Array.contains "sub").IsTrue())
        do! check(Assert.That(arguments |> Array.contains "command").IsTrue())
        do! check(Assert.That(arguments |> Array.contains "--force").IsTrue())
        do! check(Assert.That(arguments |> Array.contains "--output").IsTrue())
        do! check(Assert.That(arguments |> Array.contains "/path/to/output").IsTrue())
    }

    [<Test>]
    member this.Build_WithPositionalArguments_PlacesCorrectly() = async {
        let! builder = this.GetService<ICommandLineBuilder>() |> Async.AwaitTask
        let options = TestPositionalOptions(FilePath = "test.cs", ConfigPath = "config.json")
        let result = builder.Build(options)
        let arguments = result.Arguments |> Seq.toArray
        let fileIndex = arguments |> Array.tryFindIndex ((=) "test.cs")
        let configIndex = arguments |> Array.tryFindIndex ((=) "config.json")

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.Tool), "processor"))
        do! check(Assert.That(fileIndex.IsSome).IsTrue())
        do! check(Assert.That(configIndex.IsSome).IsTrue())
        do! check(Assert.That(fileIndex.Value < configIndex.Value).IsTrue())
    }

    [<Test>]
    member this.Build_ReturnsImmutableCommandLine() = async {
        let! builder = this.GetService<ICommandLineBuilder>() |> Async.AwaitTask
        let options = GenericCommandLineToolOptions("echo", Arguments = [| "original" |])
        let result = builder.Build(options)

        do! check(Assert.That(result.Arguments :? ReadOnlyCollection<string>).IsTrue())
    }

    [<Test>]
    member this.Build_ToString_FormatsCorrectly() = async {
        let! builder = this.GetService<ICommandLineBuilder>() |> Async.AwaitTask
        let options = GenericCommandLineToolOptions("git", Arguments = [| "status"; "-s" |])
        let result = builder.Build(options)

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.ToString()), "git status -s"))
    }

    [<Test>]
    member this.Build_SkipsDuplicateToolInArguments() = async {
        let! builder = this.GetService<ICommandLineBuilder>() |> Async.AwaitTask
        let options = GenericCommandLineToolOptions("git", Arguments = [| "git"; "status" |])
        let result = builder.Build(options)
        let arguments = result.Arguments |> Seq.toArray

        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.Tool), "git"))
        do! check(Assert.That(arguments |> Array.filter ((=) "git") |> Array.isEmpty).IsTrue())
        do! check(Assert.That(arguments |> Array.contains "status").IsTrue())
    }
