using ModularPipelines.Context;
using ModularPipelines.Attributes;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Commands;

public class CommandParserTests : TestBase
{
    [Test]
    public async Task Empty_Options_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions());
        await Assert.That(result.CommandInput).IsEqualTo("mysupersecrettool do this then that");
    }

    [Test]
    public async Task KeyValues_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            BuildArgs = new List<KeyValue>
            {
                ("Arg1", "Value1"),
                ("Arg2", "Value2"),
                ("Arg3", "Value3"),
            },
        });
        await Assert.That(result.CommandInput).IsEqualTo("mysupersecrettool do this then that --build-arg Arg1=Value1 --build-arg Arg2=Value2 --build-arg Arg3=Value3");
    }

    [Test]
    public async Task Boolean_Switch_Parse_As_Expected_When_True()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            Force = true,
        });
        await Assert.That(result.CommandInput).IsEqualTo("mysupersecrettool do this then that --force");
    }

    [Test]
    [Arguments(null)]
    [Arguments(false)]
    public async Task Boolean_Switch_Parse_As_Expected_When_Not_True(bool? force)
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            Force = force,
        });
        await Assert.That(result.CommandInput).IsEqualTo("mysupersecrettool do this then that");
    }

    [Test]
    public async Task String_Array_Switch_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            Filename =
            [
                "file1.txt",
                "foo.txt",
                "bar.json"
            ],
        });
        await Assert.That(result.CommandInput).IsEqualTo("mysupersecrettool do this then that --filename file1.txt --filename foo.txt --filename bar.json");
    }

    [Test]
    public async Task String_Switch_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            SomeString = "Foo bar",
        });
        await Assert.That(result.CommandInput).IsEqualTo("""
                                                    mysupersecrettool do this then that --some-string "Foo bar"
                                                    """);
    }

    [Test]
    public async Task Int_Switch_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            GracePeriod = 123,
        });
        await Assert.That(result.CommandInput).IsEqualTo("mysupersecrettool do this then that --grace-period 123");
    }

    [Test]
    public async Task Enum_Value_Switch_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            Verbosity = "quiet",
        });
        await Assert.That(result.CommandInput).IsEqualTo("mysupersecrettool do this then that --verbosity quiet");
    }

    [Test]
    public async Task Positional_Parameter_Before_Switches_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            SomeString = "Foo bar",
            Positional1 = "MyFile.txt",
        });
        await Assert.That(result.CommandInput).IsEqualTo("""
                                                    mysupersecrettool do this then that MyFile.txt --some-string "Foo bar"
                                                    """);
    }

    [Test]
    public async Task Positional_Parameter_After_Switches_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            SomeString = "Foo bar",
            Positional2 = "MyFile.txt",
        });
        await Assert.That(result.CommandInput).IsEqualTo("""
                                                    mysupersecrettool do this then that --some-string "Foo bar" MyFile.txt
                                                    """);
    }

    [Test]
    public async Task Multiple_Positional_Arguments_With_Interleaved_Command()
    {
        var result = await GetResult(new PlaceholderToolOptions("ThisPackage", "MyProject.csproj")
        {
            Source = "nuget.org"
        });
        await Assert.That(result.CommandInput).IsEqualTo("dotnet add MyProject.csproj package ThisPackage --source nuget.org");
    }

    [Test]
    public async Task Single_Positional_Argument_Immediately_After_Command()
    {
        var result = await GetResult(new PlaceholderToolOptions3
        {
            Project = "MyProject.csproj"
        });

        await Assert.That(result.CommandInput).IsEqualTo("dotnet add MyProject.csproj");
    }

    private async Task<CommandResult> GetResult(CommandLineToolOptions options)
    {
        var command = await GetService<ICommandContext>();

        var executionOptions = new CommandExecutionOptions
        {
            InternalDryRun = true,
        };

        return await command.ExecuteCommandLineToolAsync(options, executionOptions);
    }

    [CliTool("mysupersecrettool")]
    [CliSubCommand("do", "this", "then", "that")]
    internal record MySuperSecretToolOptions : CommandLineToolOptions
    {
        [CliOption("--build-arg")]
        public IReadOnlyList<KeyValue>? BuildArgs { get; set; }

        [CliFlag("--force")]
        public bool? Force { get; set; }

        [CliOption("--verbosity")]
        public string? Verbosity { get; set; }

        [CliOption("--grace-period")]
        public int? GracePeriod { get; set; }

        [CliOption("--some-string")]
        public string? SomeString { get; set; }

        [CliOption("--filename")]
        public string[]? Filename { get; set; }

        [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
        public string? Positional1 { get; set; }

        [CliArgument(Phase = CommandLinePhase.Passthrough)]
        public string? Positional2 { get; set; }
    }

    [CliTool("dotnet")]
    [CliSubCommand("add")]
    internal record PlaceholderToolOptions(string Package, string Project) : CommandLineToolOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
        public string Project { get; set; } = Project;

        [CliArgument(1, Phase = CommandLinePhase.EarlyOperand)]
        public string Command { get; set; } = "package";

        [CliArgument(2, Phase = CommandLinePhase.EarlyOperand)]
        public string Package { get; set; } = Package;

        [CliOption("--source")]
        public string? Source { get; set; }
    }

    [CliTool("dotnet")]
    [CliSubCommand("add")]
    internal record PlaceholderToolOptions3 : CommandLineToolOptions
    {
        [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
        public string? Project { get; set; }

        [CliOption("--source")]
        public string? Source { get; set; }
    }
}
