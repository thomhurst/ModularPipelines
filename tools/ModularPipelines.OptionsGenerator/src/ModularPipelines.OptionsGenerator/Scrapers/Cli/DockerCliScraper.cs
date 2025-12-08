using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Docker.
/// Docker is a Cobra-based CLI with consistent help formatting.
/// </summary>
public partial class DockerCliScraper : CobraCliScraper
{
    private readonly HashSet<string> _extensionCommands = new(StringComparer.OrdinalIgnoreCase);

    public override string ToolName => "docker";
    public override string NamespacePrefix => "Docker";
    public override string TargetNamespace => "ModularPipelines.Docker";
    public override string OutputDirectory => "src/ModularPipelines.Docker";

    public DockerCliScraper(ICliCommandExecutor executor, ILogger<DockerCliScraper> logger)
        : base(executor, logger)
    {
    }

    /// <summary>
    /// Extract subcommands and detect which ones are third-party extensions.
    /// Docker marks extensions with an asterisk (*) in the help output:
    /// "  buildx*     Docker Buildx"
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        _extensionCommands.Clear();

        // Match command lines that may have an asterisk marker
        // Format: "  command*    Description" or "  command    Description"
        var matches = DockerSubcommandPattern().Matches(helpText);

        foreach (Match match in matches)
        {
            var commandName = match.Groups["name"].Value.Trim();
            var isExtension = match.Groups["star"].Success;

            if (!string.IsNullOrEmpty(commandName) && !commandName.Contains(' '))
            {
                subcommands.Add(commandName);

                if (isExtension)
                {
                    _extensionCommands.Add(commandName);
                    Logger.LogDebug("Detected third-party extension: {Command}", commandName);
                }
            }
        }

        return subcommands;
    }

    /// <summary>
    /// Skip third-party extensions (detected by asterisk marker in help output).
    /// </summary>
    protected override bool IsSkippableSubcommand(string subcommand)
    {
        if (base.IsSkippableSubcommand(subcommand))
        {
            return true;
        }

        // Skip commands that were marked with * in the help output
        return _extensionCommands.Contains(subcommand);
    }

    /// <summary>
    /// Matches Docker subcommand lines, capturing the asterisk if present.
    /// Examples:
    ///   "  build       Build an image from a Dockerfile"
    ///   "  buildx*     Docker Buildx"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)(?<star>\*)?\s{2,}", RegexOptions.Multiline)]
    private static partial Regex DockerSubcommandPattern();
}
