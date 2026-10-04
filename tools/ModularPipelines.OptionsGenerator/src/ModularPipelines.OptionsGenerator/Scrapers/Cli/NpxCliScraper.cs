using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>Generates npx options from its installed npm exec help.</summary>
internal sealed partial class NpxCliScraper(
    ICliCommandExecutor executor,
    IHelpTextCache helpCache,
    ILogger<NpxCliScraper> logger) : CliScraperBase(executor, helpCache, logger)
{
    public override string ToolName => "npx";

    public override string NamespacePrefix => "Npx";

    public override string TargetNamespace => "ModularPipelines.Node";

    public override string OutputDirectory => "src/ModularPipelines.Node";

    public override CliToolDefinition CreateToolDefinition() => base.CreateToolDefinition() with
    {
        CommandCoverage = new CliCommandCoveragePolicy
        {
            MinimumCommandCount = 1,
            SentinelCommands = ["npx"],
        },
    };

    protected override UsageSynopsisParseResult ParseUsageSynopsis(string[] commandPath, string helpText) =>
        base.ParseUsageSynopsis(commandPath, ExecSynopsisPattern().Replace(helpText, "npx"));

    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var usage = ParseUsageSynopsis(commandPath, helpText);
        return Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
        {
            FullCommand = "npx",
            CommandParts = [],
            ClassName = "NpxExecuteOptions",
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = NpmCliScraper.ExtractDescription(helpText),
            DocumentationUrl = "https://docs.npmjs.com/cli/commands/npx",
            Options = NpmCliScraper.ParseOptions(helpText),
            PositionalArguments = usage.PositionalArguments,
            SubDomainGroup = null,
            Enums = [],
        });
    }

    [GeneratedRegex(@"^npm exec\b", RegexOptions.Multiline)]
    private static partial Regex ExecSynopsisPattern();
}
