using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class DotNetCliScraperTests
{
    [Test]
    [Arguments("add", true)]
    [Arguments("remove", true)]
    [Arguments("migrate", false)]
    [Arguments("list", false)]
    public async Task Solution_Commands_Preserve_Operands_Before_The_Child_Command(string child, bool hasProjects)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures",
            $"dotnet-11.0.100-rc.1-sln-{child}-help.txt"));
        var command = (await new TestDotNetCliScraper().Parse(["dotnet", "solution", child], help))!;

        await Assert.That(command.PositionalArguments.Count).IsEqualTo(hasProjects ? 2 : 1);
        var solution = command.PositionalArguments[0];
        await Assert.That(solution.PropertyName).IsEqualTo("SlnFile");
        await Assert.That(solution.PositionIndex).IsEqualTo(0);
        await Assert.That(solution.IsRequired).IsFalse();
        await Assert.That(solution.IsVariadic).IsFalse();
        await Assert.That(solution.CSharpType).IsEqualTo("string?");
        await Assert.That(solution.Description).Contains("The solution file to operate on.");

        var generated = (await new OptionsClassGenerator().GenerateAsync(DotNet11Tool(command))).Single().Content;
        await Assert.That(generated).Contains("[CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]");
        await Assert.That(generated).Contains("public string? SlnFile");

        if (hasProjects)
        {
            var projects = command.PositionalArguments[1];
            await Assert.That(projects.PropertyName).IsEqualTo("ProjectPath");
            await Assert.That(projects.PositionIndex).IsEqualTo(1);
            await Assert.That(projects.IsVariadic).IsTrue();
            await Assert.That(projects.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(generated).Contains("[CliArgument(1, Phase = CommandLinePhase.EarlyOperand)]");
            await Assert.That(generated).Contains("public IEnumerable<string>? ProjectPath");
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Workload_Search_Operands_Belong_To_The_Selected_Command(bool version)
    {
        var suffix = version ? "-version" : string.Empty;
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures",
            $"dotnet-11.0.100-rc.1-workload-search{suffix}-help.txt"));
        var path = version ? new[] { "dotnet", "workload", "search", "version" } : ["dotnet", "workload", "search"];
        var command = (await new TestDotNetCliScraper().Parse(path, help))!;

        var operand = command.PositionalArguments.Single();
        await Assert.That(operand.PropertyName).IsEqualTo(version ? "WorkloadVersion" : "SearchString");
        await Assert.That(operand.PositionIndex).IsEqualTo(0);
        await Assert.That(operand.IsRequired).IsFalse();
        await Assert.That(operand.IsVariadic).IsEqualTo(version);
        await Assert.That(operand.CSharpType).IsEqualTo(version ? "IEnumerable<string>?" : "string?");
        await Assert.That(operand.Description).IsEqualTo(version
            ? "Output workload manifest versions associated with the provided workload version."
            : "The text to search for in the IDs and descriptions of available workloads.");

        if (version)
        {
            await Assert.That(command.Description).Contains("If no argument is specified");
            await Assert.That(command.Description).Contains("maui@9.0.0-rc.1.24453.9");
            var enhanced = await OptionTypeEnhancer.CreateDefault(new DotNet11HelpExecutor(help), NullLoggerFactory.Instance)
                .EnhanceAsync(DotNet11Tool(command));
            var take = enhanced.Commands.Single().Options.Single(option => option.SwitchName == "--take");
            await Assert.That(take.CSharpType).IsEqualTo("int?");
            await Assert.That(take.IsNumeric).IsTrue();
            await Assert.That(take.Description).IsEqualTo("[default: 5]");
            var previews = command.Options.Single(option => option.SwitchName == "--include-previews");
            await Assert.That(previews.IsFlag).IsTrue();
        }
    }

    [Test]
    [Arguments("--list-tests", true)]
    [Arguments("--list-tests <text|json>", true)]
    [Arguments("--list-tests [<text|json>]", false)]
    [Arguments("--list-tests [text|json]", false)]
    public async Task List_Tests_Preserves_Optional_Format_Through_Enhancement_And_Generation(
        string declaration, bool optionalInDescription)
    {
        var description = "List the discovered tests instead of running the tests.";
        if (optionalInDescription)
        {
            description += " Optionally accepts a format: 'text' (default) for human-readable output or 'json' for machine-readable output.";
        }

        var help = $"Options:\n  {declaration}  {description} [default: False]\n";
        var command = (await new TestDotNetCliScraper().Parse(["dotnet", "test"], help))!;
        var enhancer = OptionTypeEnhancer.CreateDefault(new DotNet11HelpExecutor(help), NullLoggerFactory.Instance);
        var enhanced = await enhancer.EnhanceAsync(DotNet11Tool(command));
        var option = enhanced.Commands.Single().Options.Single(option => option.SwitchName == "--list-tests");

        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
        await Assert.That(option.PropertyType).IsEqualTo("CliOptionValue?");
        var generated = (await new OptionsClassGenerator().GenerateAsync(enhanced)).Single().Content;
        await Assert.That(generated).Contains("public CliOptionValue? ListTests");
        await Assert.That(generated).Contains("ValueArity = CliOptionValueArity.Optional");
    }

    [Test]
    public async Task Legacy_List_Tests_Remains_A_Flag()
    {
        var command = (await new TestDotNetCliScraper().Parse(["dotnet", "test"],
            "Options:\n  --list-tests  List the discovered tests instead of running the tests. [default: False]\n"))!;
        var option = command.Options.Single(option => option.SwitchName == "--list-tests");
        await Assert.That(option.IsFlag).IsTrue();
        await Assert.That(option.CSharpType).IsEqualTo("bool?");
    }

    [Test]
    public async Task Default_Only_Descriptions_Are_Not_Optional_Value_Placeholders()
    {
        var command = (await new TestDotNetCliScraper().Parse(["dotnet", "workload", "search", "version"],
            "Options:\n  --include-previews  [default: False]  \n  --take <take>  [default: 5]  \n"))!;
        var previews = command.Options.Single(option => option.SwitchName == "--include-previews");
        var take = command.Options.Single(option => option.SwitchName == "--take");
        await Assert.That(previews.IsFlag).IsTrue();
        await Assert.That(take.ValueArity).IsEqualTo(CliOptionValueArity.Required);
        await Assert.That(take.CSharpType).IsEqualTo("int?");
    }

    private static CliToolDefinition DotNet11Tool(CliCommandDefinition command) => new()
    {
        ToolName = "dotnet",
        NamespacePrefix = "DotNet",
        TargetNamespace = "ModularPipelines.DotNet",
        OutputDirectory = "unused",
        Commands = [command],
    };

    private sealed class DotNet11HelpExecutor(string help) : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, string? workingDirectory = null,
            CancellationToken cancellationToken = default) => Task.FromResult(new CliCommandResult
            {
                StandardOutput = help,
                StandardError = string.Empty,
                ExitCode = 0,
            });
    }
}
