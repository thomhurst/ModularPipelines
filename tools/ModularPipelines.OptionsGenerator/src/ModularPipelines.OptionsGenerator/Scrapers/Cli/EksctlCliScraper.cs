using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for eksctl - Amazon EKS cluster management.
/// eksctl uses Cobra for its CLI.
///
/// eksctl help format (eksctl --help):
/// The official CLI for Amazon EKS
///
/// Usage: eksctl [command] [flags]
///
/// Commands:
///   eksctl associate         Associate resources with a cluster
///   eksctl completion        Generates shell completion scripts
///   eksctl create            Create resource(s)
///   eksctl delete            Delete resource(s)
///   ...
/// </summary>
public partial class EksctlCliScraper : CobraCliScraper
{
    public EksctlCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<EksctlCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "eksctl";

    public override string NamespacePrefix => "Eksctl";

    public override string TargetNamespace => "ModularPipelines.Eksctl";

    public override string OutputDirectory => "src/ModularPipelines.Eksctl";

    /// <summary>
    /// Eksctl prints the complete command path on every command line. Return only
    /// the final segment so the shared recursive discovery can append it once.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        foreach (Match match in EksctlCommandLinePattern().Matches(helpText))
        {
            var commandPath = match.Groups["path"].Value;
            yield return commandPath[(commandPath.LastIndexOf(' ') + 1)..];
        }
    }

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "info"
    };

    [GeneratedRegex(@"^\s*eksctl (?<path>[\w-]+(?: [\w-]+)*) {2,}", RegexOptions.Multiline)]
    private static partial Regex EksctlCommandLinePattern();
}
