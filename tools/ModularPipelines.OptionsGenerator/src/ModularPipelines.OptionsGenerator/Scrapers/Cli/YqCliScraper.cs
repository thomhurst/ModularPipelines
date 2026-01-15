using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for yq - YAML/JSON/XML processor (mikefarah/yq).
/// yq uses a Cobra-style help format.
///
/// yq help format (yq --help):
/// yq is a portable command-line YAML, JSON, XML, CSV, TOML and properties processor
///
/// Usage:
///   yq [flags]
///   yq [command]
///
/// Available Commands:
///   eval        Apply expression to files
///   eval-all    Apply expression to all files
///   ...
///
/// Flags:
///   -C, --colors                  force print with colors
///   ...
/// </summary>
public partial class YqCliScraper : CobraCliScraper
{
    public YqCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<YqCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "yq";

    public override string NamespacePrefix => "Yq";

    public override string TargetNamespace => "ModularPipelines.Yq";

    public override string OutputDirectory => "src/ModularPipelines.Yq";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "shell-completion"
    };
}
