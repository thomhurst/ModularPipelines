using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class CommandParserTests : TestBase
{
    [Test]
    public async Task Empty_Options_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions());
        await Assert.That(result.CommandInput).Is.EqualTo("mysupersecrettool do this then that");
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
        await Assert.That(result.CommandInput).Is.EqualTo("mysupersecrettool do this then that --build-arg Arg1=Value1 --build-arg Arg2=Value2 --build-arg Arg3=Value3");
    }

    [Test]
    public async Task Boolean_Switch_Parse_As_Expected_When_True()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            Force = true,
        });
        await Assert.That(result.CommandInput).Is.EqualTo("mysupersecrettool do this then that --force");
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
        await Assert.That(result.CommandInput).Is.EqualTo("mysupersecrettool do this then that");
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
        await Assert.That(result.CommandInput).Is.EqualTo("mysupersecrettool do this then that --filename file1.txt --filename foo.txt --filename bar.json");
    }

    [Test]
    public async Task String_Switch_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            SomeString = "Foo bar",
        });
        await Assert.That(result.CommandInput).Is.EqualTo("""
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
        await Assert.That(result.CommandInput).Is.EqualTo("mysupersecrettool do this then that --grace-period 123");
    }

    [Test]
    public async Task Enum_Value_Switch_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            Verbosity = Verbosity.Quiet,
        });
        await Assert.That(result.CommandInput).Is.EqualTo("mysupersecrettool do this then that --verbosity quiet");
    }

    [Test]
    public async Task Positional_Parameter_Before_Switches_Parse_As_Expected()
    {
        var result = await GetResult(new MySuperSecretToolOptions
        {
            SomeString = "Foo bar",
            Positional1 = "MyFile.txt",
        });
        await Assert.That(result.CommandInput).Is.EqualTo("""
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
        await Assert.That(result.CommandInput).Is.EqualTo("""
                                                    mysupersecrettool do this then that --some-string "Foo bar" MyFile.txt
                                                    """);
    }

    [Test]
    public async Task Placeholder_Positional_Parameter_Parse_As_Expected()
    {
        var result = await GetResult(new PlaceholderToolOptions("ThisPackage", "MyProject.csproj")
        {
            Source = "nuget.org"
        });
        await Assert.That(result.CommandInput).Is.EqualTo("dotnet add MyProject.csproj package ThisPackage --source nuget.org");
    }

    [Test]
    [Arguments("")]
    [Arguments(null)]
    public async Task Placeholder_Positional_WhenEmpty_Throws(string? package)
    {
        await Assert.That(() => GetResult(new PlaceholderToolOptions(package!, "MyProject.csproj")
        {
            Source = "nuget.org"
        })).Throws.Exception().OfType<ArgumentException>();
    }

    [Test]
    public async Task No_Matching_Placeholder_Positional_Throws()
    {
        await Assert.That(() => GetResult(new PlaceholderToolOptions2("ThisPackage", "MyProject.csproj")
        {
            Source = "nuget.org"
        })).Throws.Exception().OfType<ArgumentException>();
    }

    private async Task<CommandResult> GetResult(CommandLineToolOptions options)
    {
        var command = await GetService<ICommand>();

        options.InternalDryRun = true;

        return await command.ExecuteCommandLineTool(options);
    }

    [CommandPrecedingArguments("do", "this", "then", "that")]
    private record MySuperSecretToolOptions() : CommandLineToolOptions("mysupersecrettool")
    {
        [CommandSwitch("--build-arg")]
        public IEnumerable<KeyValue>? BuildArgs { get; set; }

        [BooleanCommandSwitch("--force")]
        public bool? Force { get; set; }

        [CommandSwitch("--verbosity")]
        public string? Verbosity { get; set; }

        [CommandSwitch("--grace-period")]
        public int? GracePeriod { get; set; }

        [CommandSwitch("--some-string")]
        public string? SomeString { get; set; }

        [CommandSwitch("--filename")]
        public string[]? Filename { get; set; }

        [PositionalArgument(Position = Position.BeforeSwitches)]
        public string? Positional1 { get; set; }

        [PositionalArgument(Position = Position.AfterSwitches)]
        public string? Positional2 { get; set; }
    }

    [CommandPrecedingArguments("add", "[<PROJECT>]", "package", "<PACKAGE_NAME>")]
    private record PlaceholderToolOptions(string Package, string Project) : CommandLineToolOptions("dotnet")
    {
        [PositionalArgument(PlaceholderName = "[<PROJECT>]")]
        public string Project { get; set; } = Project;

        [PositionalArgument(PlaceholderName = "<PACKAGE_NAME>")]
        public string Package { get; set; } = Package;

        [CommandSwitch("--source")]
        public string? Source { get; set; }
    }

    [CommandPrecedingArguments("add", "[<PROJECT>]", "package", "<PACKAGE_NAME>")]
    private record PlaceholderToolOptions2(string Package, string Project) : CommandLineToolOptions("dotnet")
    {
        [PositionalArgument(PlaceholderName = "[<PROJECT>]")]
        public string Project { get; set; } = Project;

        [PositionalArgument(PlaceholderName = "<WRONG_PLACEHOLDER>")]
        public string Package { get; set; } = Package;

        [CommandSwitch("--source")]
        public string? Source { get; set; }
    }
}
