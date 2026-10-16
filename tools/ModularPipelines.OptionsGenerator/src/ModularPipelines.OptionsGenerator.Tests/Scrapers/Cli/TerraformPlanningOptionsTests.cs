using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class TerraformPlanningOptionsTests
{
    [Test]
    public async Task Plan_Parses_Every_Option_Section()
    {
        var command = await ParseFixture("plan");

        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(
            ["-destroy", "-refresh-only", "-refresh", "-replace", "-target", "-var", "-var-file",
                "-compact-warnings", "-detailed-exitcode", "-generate-config-out", "-input", "-lock",
                "-lock-timeout", "-no-color", "-out", "-parallelism", "-state"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "-parallelism").CSharpType).IsEqualTo("int?");
        await Assert.That(command.Options.Single(option => option.SwitchName == "-detailed-exitcode").IsFlag).IsTrue();
    }

    [Test]
    [Arguments("plan")]
    [Arguments("apply")]
    [Arguments("destroy")]
    [Arguments("query")]
    public async Task Planning_Commands_Preserve_Repeatable_Variables(string name)
    {
        var command = await ParseFixture(name);

        foreach (var switchName in new[] { "-var", "-var-file" })
        {
            var option = command.Options.Single(option => option.SwitchName == switchName);
            using (Assert.Multiple())
            {
                await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
                await Assert.That(option.IsFlag).IsFalse();
                await Assert.That(option.AcceptsMultipleValues).IsTrue();
                await Assert.That(option.ValueSeparator).IsEqualTo("=");
            }
        }
    }

    [Test]
    [Arguments("plan")]
    [Arguments("apply")]
    [Arguments("destroy")]
    public async Task Boolean_Values_Are_Not_Presence_Only_Flags(string name)
    {
        var command = await ParseFixture(name);

        foreach (var switchName in new[] { "-input", "-lock", "-refresh" })
        {
            var option = command.Options.Single(option => option.SwitchName == switchName);
            using (Assert.Multiple())
            {
                await Assert.That(option.CSharpType).IsEqualTo("bool?");
                await Assert.That(option.IsFlag).IsFalse();
                await Assert.That(option.AcceptsMultipleValues).IsFalse();
                await Assert.That(option.ValueSeparator).IsEqualTo("=");
            }
        }
    }

    [Test]
    public async Task Apply_And_Destroy_Read_Documented_Shared_Options_Without_Plan_Only_Switches()
    {
        foreach (var name in new[] { "apply", "destroy" })
        {
            var command = await ParseFixture(name);
            await Assert.That(command.Options.Any(option => option.SwitchName == "-target")).IsTrue();
            await Assert.That(command.Options.Any(option => option.SwitchName == "-auto-approve")).IsTrue();
            await Assert.That(command.Options.Any(option => option.SwitchName == "-out")).IsFalse();
            await Assert.That(command.Options.Any(option => option.SwitchName == "-detailed-exitcode")).IsFalse();
            await Assert.That(command.Options.Select(option => option.SwitchName).Distinct().Count()).IsEqualTo(command.Options.Count);
            if (name == "destroy")
            {
                await Assert.That(command.PositionalArguments).IsEmpty();
                await Assert.That(command.Options.Any(option => option.SwitchName is "-destroy" or "-refresh-only" or "-replace")).IsFalse();
            }
        }
    }

    [Test]
    public async Task Query_Parses_Customization_And_Other_Options()
    {
        var command = await ParseFixture("query");
        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(
            ["-var", "-var-file", "-generate-config-out", "-json", "-no-color"]);
    }

    [Test]
    [Arguments("'foo=hello world'")]
    [Arguments("\"foo=hello world\"")]
    public async Task Quoted_Values_And_Section_Boundaries_Preserve_Descriptions(string value)
    {
        var command = await new TestScraper().Parse("query", $$"""
            Usage: terraform query [options]

            Query Customization Options:
              -var {{value}}  Set a variable. Use this option more than
                              once to set more than one variable.

            Global options (use these before the subcommand, if any):
              -chdir=DIR  Switch working directory.

            Examples:
              -example  This is not an option.

            Other Options:
              -var 'duplicate=value'  Duplicate option from another section.
              -json  Print JSON.
              -json  Duplicate option.
            """);

        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(["-var", "-json"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "-var").Description)
            .IsEqualTo("Set a variable. Use this option more than once to set more than one variable.");
    }

    internal static async Task<CliCommandDefinition> ParseFixture(string command) =>
        await new TestScraper().Parse(command, await ReadFixture(command));

    private static Task<string> ReadFixture(string command) => File.ReadAllTextAsync(Path.Combine(
        AppContext.BaseDirectory, "Fixtures", $"terraform-1.14.3-{command}-help.txt"));

    private sealed class TestScraper() : TerraformCliScraper(
        new FixtureExecutor(),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<TerraformCliScraper>.Instance)
    {
        public async Task<CliCommandDefinition> Parse(string command, string help) =>
            (await ParseCommandAsync(["terraform", command], help,
                ParseUsageSynopsis(["terraform", command], help), CancellationToken.None))!;
    }

    private sealed class FixtureExecutor : ICliCommandExecutor
    {
        public async Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            string? workingDirectory = null, CancellationToken cancellationToken = default) => new()
            {
                ExitCode = 0,
                StandardOutput = await ReadFixture(arguments.Split(' ')[0]),
                StandardError = "",
            };

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
