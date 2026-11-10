using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;
using TUnit.Assertions.Enums;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    private static readonly string[] ArtifactNumbers = ["2", "3"];
    private static readonly string[] ArtifactUrls = ["https://example.test/artifact/2", "https://example.test/artifact/3"];

    [Test]
    [Arguments("edit", "{<issue> <artifact-number> | <artifact-url>}", false, false)]
    [Arguments("view", "{<artifact-url> | <issue> <artifact-number>}", false, false)]
    [Arguments("edit", "[<issue> <artifact-number> | <artifact-url>]", false, false)]
    [Arguments("delete", "{<issue> <artifact-number>... | <artifact-url>...}", true, true)]
    [Arguments("download", "{<issue> [<artifact-number>...] | <artifact-url>...}", true, true)]
    public async Task Gh_Artifact_Alternatives_Validate_And_Render_Separate_Operands(
        string commandName, string operands, bool repeatedNumber, bool repeatedUrl)
    {
        var commands = new List<CliCommandDefinition>();
        var scraper = new GhCliScraper(new ArtifactHelpExecutor(commandName, operands),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<GhCliScraper>.Instance);
        await foreach (var parsed in scraper.ScrapeAsync())
        {
            commands.Add(parsed);
        }
        var command = commands.Single(parsed => parsed.FullCommand == $"gh issue artifact {commandName}");
        var generated = await Generate([], command.PositionalArguments, command.RequiredAlternativeGroups);
        var options = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var optionalNumber = commandName == "download";
        bool Valid() => Validator.TryValidateObject(instance, new(instance), [], true);
        void Set(string property, object? value) => options.GetProperty(property)!.SetValue(instance, value);
        string[] Render() => [.. new CommandArgumentBuilder().BuildArguments(
            [.. command.PositionalArguments.Select(argument => new ArgumentPart(argument.PropertyName,
                options.GetProperty(argument.PropertyName)!.GetValue,
                new CliArgumentAttribute(argument.PositionIndex) { Phase = argument.Phase }))],
            instance)];

        await Assert.That(Valid()).IsEqualTo(operands.StartsWith('['));
        Set("Issue", "142");
        await Assert.That(Valid()).IsEqualTo(optionalNumber);
        if (optionalNumber)
        {
            await Assert.That(Render()).IsEquivalentTo(["142"], CollectionOrdering.Matching);
        }
        Set("ArtifactNumber", repeatedNumber ? ArtifactNumbers : "2");
        await Assert.That(Valid()).IsTrue();
        string[] expectedPair = repeatedNumber ? ["142", "2", "3"] : ["142", "2"];
        await Assert.That(Render()).IsEquivalentTo(expectedPair, CollectionOrdering.Matching);
        Set("Issue", null);
        await Assert.That(Valid()).IsFalse();
        Set("ArtifactNumber", null);
        Set("ArtifactUrl", repeatedUrl ? ArtifactUrls : "https://example.test/artifact/2");
        await Assert.That(Valid()).IsTrue();
        string[] expectedUrls = repeatedUrl
            ? ["https://example.test/artifact/2", "https://example.test/artifact/3"]
            : ["https://example.test/artifact/2"];
        await Assert.That(Render()).IsEquivalentTo(expectedUrls, CollectionOrdering.Matching);
        Set("Issue", "142");
        await Assert.That(Valid()).IsFalse();
    }

    [Test]
    public async Task Optional_Positional_Branches_Require_Complete_Nonblank_Members_When_Rendered()
    {
        var scraper = new GhCliScraper(new ArtifactHelpExecutor("edit", "[<issue> <artifact-number> | <artifact-url>]"),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<GhCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var parsed in scraper.ScrapeAsync())
        {
            commands.Add(parsed);
        }
        var command = commands.Single(parsed => parsed.FullCommand == "gh issue artifact edit");
        var generated = await Generate([], command.PositionalArguments, command.RequiredAlternativeGroups);
        var optionsType = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(optionsType)!;
        foreach (var (issue, number, url, valid) in new (string?, string?, string?, bool)[]
                 { (null, null, null, true), ("", null, null, false), (" \t", null, null, false),
                     (null, "", null, false), (null, " \t", null, false), ("", " \t", null, false),
                     ("142", "", null, false), (" \t", "2", null, false), ("142", "2", null, true),
                     (null, null, "https://example.test/artifact/2", true) })
        {
            optionsType.GetProperty("Issue")!.SetValue(instance, issue);
            optionsType.GetProperty("ArtifactNumber")!.SetValue(instance, number);
            optionsType.GetProperty("ArtifactUrl")!.SetValue(instance, url);
            var arguments = new CommandArgumentBuilder().BuildArguments(
                [.. command.PositionalArguments.Select(argument => new ArgumentPart(argument.PropertyName,
                    optionsType.GetProperty(argument.PropertyName)!.GetValue,
                    new CliArgumentAttribute(argument.PositionIndex) { Phase = argument.Phase }))], instance);
            await Assert.That(arguments).IsEquivalentTo(
                new[] { issue, number, url }.Where(value => value is not null), CollectionOrdering.Matching);
            await Assert.That(Validator.TryValidateObject(instance, new(instance), [], true)).IsEqualTo(valid)
                .Because($"Issue '{issue ?? "<omitted>"}', number '{number ?? "<omitted>"}', URL '{url ?? "<omitted>"}'.");
        }
    }

    [Test]
    [Arguments("{<issue> <artifact-number> | <artifact-url>}")]
    [Arguments("[<issue> <artifact-number> | <artifact-url>]")]
    public async Task Positional_Alternative_Groups_Keep_Outer_Requiredness_And_Trailing_Order(string group)
    {
        var result = UsageSynopsisParser.Parse($"Usage: tool run <prefix> {group} <suffix>", ["tool", "run"]);
        await Assert.That(result.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Prefix", "Issue", "ArtifactNumber", "ArtifactUrl", "Suffix"], CollectionOrdering.Matching);
        await Assert.That(result.PositionalArguments.Select(argument => argument.PositionIndex))
            .IsEquivalentTo([0, 1, 2, 3, 4], CollectionOrdering.Matching);
        await Assert.That(result.PositionalArguments.Where(argument => argument.IsRequired).Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Prefix", "Suffix"]);
        var choice = result.RequiredAlternativeGroups.Single();
        await Assert.That(choice.IsRequired).IsEqualTo(group.StartsWith('{'));
        await Assert.That(choice.Groups.Single().Members.Select(member => member.PositionalPropertyName!))
            .IsEquivalentTo(["Issue", "ArtifactNumber"]);
    }

    private sealed class ArtifactHelpExecutor(string commandName, string operands) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, string? workingDirectory = null,
            CancellationToken cancellationToken = default)
        {
            var help = arguments switch
            {
                "--help" => "USAGE\n  gh <command>\n\nCORE COMMANDS\n  issue: Work with issues\n",
                "issue --help" => "USAGE\n  gh issue <command>\n\nCOMMANDS\n  artifact: Work with artifacts\n",
                "issue artifact --help" => $"USAGE\n  gh issue artifact <command>\n\nCOMMANDS\n  {commandName}: Work with an artifact\n",
                _ when arguments == $"issue artifact {commandName} --help" => $"USAGE\n  gh issue artifact {commandName} {operands} [flags]\n",
                _ => throw new InvalidOperationException($"Unexpected help request: {arguments}"),
            };
            return Task.FromResult(new CliCommandResult { ExitCode = 0, StandardOutput = help, StandardError = "" });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
