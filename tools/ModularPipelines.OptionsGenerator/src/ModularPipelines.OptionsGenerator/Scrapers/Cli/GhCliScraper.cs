using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
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
    private static readonly IReadOnlyDictionary<(string Command, string Operand), string> OperandNames =
        new Dictionary<(string Command, string Operand), string>
        {
            [("agent-task view", "SessionId")] = "SessionIdOrPrNumberOrPrUrlOrPrBranch",
            [("attestation download", "FilePath")] = "FilePathOrImageUri",
            [("attestation verify", "FilePath")] = "FilePathOrImageUri",
            [("browse", "Number")] = "NumberOrPathOrCommitSha",
            [("cache delete", "CacheId")] = "CacheIdOrCacheKey",
            [("discussion comment", "Number")] = "NumberOrDiscussionUrlOrCommentIdOrCommentUrl",
            [("discussion view", "Number")] = "NumberOrDiscussionUrlOrCommentIdOrCommentUrl",
            [("discussion edit", "Number")] = "NumberOrDiscussionUrl",
            [("gist create", "FilenameArgument")] = "FilenameOrPattern",
            [("gist delete", "Id")] = "IdOrUrl",
            [("gist edit", "Id")] = "IdOrUrl",
            [("gist rename", "Id")] = "IdOrUrl",
            [("gist view", "Id")] = "IdOrUrl",
            [("release create", "Filename")] = "FilenameOrPattern",
            [("workflow disable", "WorkflowId")] = "WorkflowIdOrWorkflowName",
            [("workflow enable", "WorkflowId")] = "WorkflowIdOrWorkflowName",
            [("workflow run", "WorkflowId")] = "WorkflowIdOrWorkflowName",
            [("workflow view", "WorkflowId")] = "WorkflowIdOrWorkflowNameOrFilename",
        };

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

    protected override bool IsBooleanValueOption(
        string[] commandParts,
        string switchName,
        string description) =>
        (commandParts is ["release", "create"] or ["release", "edit"]
         && switchName.Equals("--latest", StringComparison.OrdinalIgnoreCase))
        || base.IsBooleanValueOption(commandParts, switchName, description);

    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments) =>
        positionalArguments
            .Where(argument => ShouldKeepOperand(commandParts, argument))
            .Select(argument => NormalizeOperand(commandParts, argument))
            .ToArray();

    protected override UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage)
    {
        var positionalArguments = usage.PositionalArguments
            .Where(argument => ShouldKeepOperand(command.CommandParts, argument))
            .Select(argument => NormalizeOperand(command.CommandParts, argument))
            .ToArray();
        return usage with
        {
            HasOperandTokens = positionalArguments.Length > 0 || usage.UnparsedOperandTokens.Count > 0,
            PositionalArguments = positionalArguments,
        };
    }

    private static bool ShouldKeepOperand(
        IReadOnlyList<string> commandParts,
        CliPositionalArgument argument) =>
        !UsageSynopsisParser.IsCommandGroupPlaceholder(argument)
        || commandParts is ["codespace", "ssh"];

    private static CliPositionalArgument NormalizeOperand(
        IReadOnlyList<string> commandParts,
        CliPositionalArgument argument)
    {
        if (argument.Phase == CommandLinePhase.Passthrough
            && argument.PropertyName is "SshFlags" or "Gitflags")
        {
            return argument with
            {
                CSharpType = "IEnumerable<string>?",
                IsRequired = false,
                IsVariadic = true,
            };
        }

        var command = string.Join(' ', commandParts);
        if (command.Equals("issue edit", StringComparison.Ordinal)
            && argument.PropertyName.Equals("Numbers", StringComparison.Ordinal))
        {
            return argument with
            {
                PropertyName = "NumbersOrUrls",
                CSharpType = "IEnumerable<string>",
                IsVariadic = true,
            };
        }

        var propertyName = GetOperandName(command, argument.PropertyName);
        return argument with { PropertyName = propertyName };
    }

    private static string GetOperandName(string command, string operand)
    {
        if (OperandNames.TryGetValue((command, operand), out var propertyName))
        {
            return propertyName;
        }

        if (!operand.Equals("Number", StringComparison.Ordinal))
        {
            return operand;
        }

        if (command.StartsWith("issue ", StringComparison.Ordinal)
            || command is "pr lock" or "pr unlock")
        {
            return "NumberOrUrl";
        }

        return command.StartsWith("pr ", StringComparison.Ordinal) ? "NumberOrUrlOrBranch" : operand;
    }
}
