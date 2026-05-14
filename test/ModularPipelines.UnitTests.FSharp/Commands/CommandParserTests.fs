namespace ModularPipelines.UnitTests.FSharp.Commands

open System
open System.Collections.Generic
open System.Threading
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.DotNet.Options
open ModularPipelines.Models
open ModularPipelines.Options
open ModularPipelines.TestHelpers
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

[<CliTool("mysupersecrettool")>]
[<CliCommand("mysupersecrettool", "do", "this", "then", "that")>]
type private MySuperSecretToolOptions() =
    inherit CommandLineToolOptions()

    [<CliOption("--build-arg")>]
    member val BuildArgs: IEnumerable<KeyValue> = null with get, set

    [<CliFlag("--force")>]
    member val Force = Nullable<bool>() with get, set

    [<CliOption("--verbosity")>]
    member val Verbosity: string = null with get, set

    [<CliOption("--grace-period")>]
    member val GracePeriod = Nullable<int>() with get, set

    [<CliOption("--some-string")>]
    member val SomeString: string = null with get, set

    [<CliOption("--filename")>]
    member val Filename: string array = null with get, set

    [<CliArgument(Placement = ArgumentPlacement.BeforeOptions)>]
    member val Positional1: string = null with get, set

    [<CliArgument(Placement = ArgumentPlacement.AfterOptions)>]
    member val Positional2: string = null with get, set

    override this.``<Clone>$``() =
        let clonedOptions = MySuperSecretToolOptions()
        clonedOptions.BuildArgs <- this.BuildArgs
        clonedOptions.Force <- this.Force
        clonedOptions.Verbosity <- this.Verbosity
        clonedOptions.GracePeriod <- this.GracePeriod
        clonedOptions.SomeString <- this.SomeString
        clonedOptions.Filename <- this.Filename
        clonedOptions.Positional1 <- this.Positional1
        clonedOptions.Positional2 <- this.Positional2
        clonedOptions :> CommandLineToolOptions

[<CliTool("dotnet")>]
[<CliCommand("dotnet", "add")>]
type private PlaceholderToolOptions(package: string, project: string) =
    inherit CommandLineToolOptions()

    [<CliArgument(0, Placement = ArgumentPlacement.ImmediatelyAfterCommand)>]
    member val Project = project with get, set

    [<CliArgument(1, Placement = ArgumentPlacement.BeforeOptions)>]
    member val Command = "package" with get, set

    [<CliArgument(2, Placement = ArgumentPlacement.BeforeOptions)>]
    member val Package = package with get, set

    [<CliOption("--source")>]
    member val Source: string = null with get, set

    override this.``<Clone>$``() =
        let clonedOptions = PlaceholderToolOptions(this.Package, this.Project)
        clonedOptions.Command <- this.Command
        clonedOptions.Source <- this.Source
        clonedOptions :> CommandLineToolOptions

[<CliTool("dotnet")>]
[<CliCommand("dotnet", "add")>]
type private PlaceholderToolOptions3() =
    inherit CommandLineToolOptions()

    [<CliArgument(Placement = ArgumentPlacement.ImmediatelyAfterCommand)>]
    member val Project: string = null with get, set

    [<CliOption("--source")>]
    member val Source: string = null with get, set

    override this.``<Clone>$``() =
        let clonedOptions = PlaceholderToolOptions3()
        clonedOptions.Project <- this.Project
        clonedOptions.Source <- this.Source
        clonedOptions :> CommandLineToolOptions

type CommandParserTests() =
    inherit TestBase()

    member private this.GetResult(options: CommandLineToolOptions) =
        task {
            let! command = this.GetService<ICommand>()

            let executionOptions = CommandExecutionOptions(InternalDryRun = true)

            return! command.ExecuteCommandLineTool(options, executionOptions, CancellationToken.None)
        }

    [<Test>]
    member this.Empty_Options_Parse_As_Expected() = async {
        let! result = this.GetResult(MySuperSecretToolOptions()) |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.CommandInput), "mysupersecrettool do this then that"))
    }

    [<Test>]
    member this.KeyValues_Parse_As_Expected() = async {
        let options = MySuperSecretToolOptions()
        options.BuildArgs <-
            [|
                KeyValue("Arg1", "Value1")
                KeyValue("Arg2", "Value2")
                KeyValue("Arg3", "Value3")
            |]

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(result.CommandInput),
                "mysupersecrettool do this then that --build-arg Arg1=Value1 --build-arg Arg2=Value2 --build-arg Arg3=Value3"
            )
        )
    }

    [<Test>]
    member this.Boolean_Switch_Parse_As_Expected_When_True() = async {
        let options = MySuperSecretToolOptions()
        options.Force <- Nullable true

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.CommandInput), "mysupersecrettool do this then that --force"))
    }

    [<Test>]
    member this.Boolean_Switch_Parse_As_Expected_When_Null() = async {
        let options = MySuperSecretToolOptions()
        options.Force <- Nullable()

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.CommandInput), "mysupersecrettool do this then that"))
    }

    [<Test>]
    member this.Boolean_Switch_Parse_As_Expected_When_False() = async {
        let options = MySuperSecretToolOptions()
        options.Force <- Nullable false

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.CommandInput), "mysupersecrettool do this then that"))
    }

    [<Test>]
    member this.String_Array_Switch_Parse_As_Expected() = async {
        let options = MySuperSecretToolOptions()
        options.Filename <- [| "file1.txt"; "foo.txt"; "bar.json" |]

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(result.CommandInput),
                "mysupersecrettool do this then that --filename file1.txt --filename foo.txt --filename bar.json"
            )
        )
    }

    [<Test>]
    member this.String_Switch_Parse_As_Expected() = async {
        let options = MySuperSecretToolOptions()
        options.SomeString <- "Foo bar"

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(result.CommandInput),
                "mysupersecrettool do this then that --some-string \"Foo bar\""
            )
        )
    }

    [<Test>]
    member this.Int_Switch_Parse_As_Expected() = async {
        let options = MySuperSecretToolOptions()
        options.GracePeriod <- Nullable 123

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.CommandInput), "mysupersecrettool do this then that --grace-period 123"))
    }

    [<Test>]
    member this.Enum_Value_Switch_Parse_As_Expected() = async {
        let options = MySuperSecretToolOptions()
        options.Verbosity <- "quiet"

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.CommandInput), "mysupersecrettool do this then that --verbosity quiet"))
    }

    [<Test>]
    member this.Positional_Parameter_Before_Switches_Parse_As_Expected() = async {
        let options = MySuperSecretToolOptions()
        options.SomeString <- "Foo bar"
        options.Positional1 <- "MyFile.txt"

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(result.CommandInput),
                "mysupersecrettool do this then that MyFile.txt --some-string \"Foo bar\""
            )
        )
    }

    [<Test>]
    member this.Positional_Parameter_After_Switches_Parse_As_Expected() = async {
        let options = MySuperSecretToolOptions()
        options.SomeString <- "Foo bar"
        options.Positional2 <- "MyFile.txt"

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(result.CommandInput),
                "mysupersecrettool do this then that --some-string \"Foo bar\" MyFile.txt"
            )
        )
    }

    [<Test>]
    member this.Multiple_Positional_Arguments_With_Interleaved_Command() = async {
        let options = PlaceholderToolOptions("ThisPackage", "MyProject.csproj")
        options.Source <- "nuget.org"

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.CommandInput), "dotnet add MyProject.csproj package ThisPackage --source nuget.org"))
    }

    [<Test>]
    member this.Single_Positional_Argument_Immediately_After_Command() = async {
        let options = PlaceholderToolOptions3()
        options.Project <- "MyProject.csproj"

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.CommandInput), "dotnet add MyProject.csproj"))
    }

    [<Test>]
    member this.DotNet_Nuget_Delete_With_Two_Positional_Arguments() = async {
        let options = DotNetNugetDeleteOptions()
        options.PackageName <- "MyPackageName"
        options.Version <- "1.0.0"

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(result.CommandInput), "dotnet nuget delete MyPackageName 1.0.0"))
    }

    [<Test>]
    member this.DotNet_Nuget_Delete_With_Source_Option() = async {
        let options = DotNetNugetDeleteOptions()
        options.PackageName <- "MyPackageName"
        options.Version <- "1.0.0"
        options.Source <- "https://api.nuget.org/v3/index.json"

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(result.CommandInput),
                "dotnet nuget delete MyPackageName 1.0.0 --source https://api.nuget.org/v3/index.json"
            )
        )
    }

    [<Test>]
    member this.DotNet_Nuget_Delete_With_ApiKey_Option() = async {
        let options = DotNetNugetDeleteOptions()
        options.PackageName <- "MyPackageName"
        options.Version <- "1.0.0"
        options.ApiKey <- "my-secret-key"

        let! result = this.GetResult(options) |> Async.AwaitTask
        do! check(
            StringEqualsAssertionExtensions.IsEqualTo(
                Assert.That(result.CommandInput),
                "dotnet nuget delete MyPackageName 1.0.0 --api-key my-secret-key"
            )
        )
    }
