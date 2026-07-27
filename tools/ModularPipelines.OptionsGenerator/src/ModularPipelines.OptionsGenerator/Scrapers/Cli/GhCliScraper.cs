using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for GitHub CLI (gh).
/// gh uses a variant of Cobra-style help format with colons after command names.
///
/// gh help format (gh --help):
/// Work seamlessly with GitHub from the command line.
///
/// USAGE
///   gh &lt;command&gt; &lt;subcommand&gt; [flags]
///
/// CORE COMMANDS
///   auth:        Authenticate gh and git with GitHub
///   browse:      Open the repository in the browser
///   ...
///
/// HELP TOPICS
///   accessibility:  Learn about GitHub CLI's accessibility experiences
///   ...
/// </summary>
public partial class GhCliScraper : CobraCliScraper
{
    private static readonly HashSet<string> RepeatableOptions = new(StringComparer.OrdinalIgnoreCase)
    {
        "--field",
        "--raw-field",
        "--header",
    };

    public GhCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<GhCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "gh";

    public override string NamespacePrefix => "Gh";

    public override string TargetNamespace => "ModularPipelines.GitHub";

    public override string OutputDirectory => "src/ModularPipelines.GitHub";

    /// <summary>
    /// Skip utility commands and help topics.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "alias", "co",
        // Help topics (not real commands)
        "accessibility", "actions", "environment", "exit-codes", "formatting", "mintty", "reference"
    };

    /// <inheritdoc />
    protected override bool IsRepeatableOption(
        string[] commandParts,
        string switchName,
        string typeHint,
        string description,
        string helpText) =>
        RepeatableOptions.Contains(switchName)
        || base.IsRepeatableOption(commandParts, switchName, typeHint, description, helpText);
}
