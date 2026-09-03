using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI scraper for Git commands.
/// Git uses a different help format than Cobra-style CLIs:
/// - Commands discovered via 'git help -a'
/// - Options via 'git <command> -h' with format: -short, --long   description
/// - Mostly flat command structure, with usage-derived child commands
/// </summary>
public partial class GitCliScraper : CliScraperBase, IDisposable
{
    private readonly object _helpRepositoryLock = new();
    private readonly SemaphoreSlim _helpCommandUsage = new(1, 1);
    private string? _helpRepositoryDirectory;
    private Task<string>? _helpRepositoryTask;
    private bool _disposed;

    public override string ToolName => "git";
    public override string NamespacePrefix => "Git";
    public override string TargetNamespace => "ModularPipelines.Git";
    public override string OutputDirectory => "src/ModularPipelines.Git";

    public override bool GenerateCommandFacade => false;

    public GitCliScraper(
        ICliCommandExecutor executor,
        IHelpTextCache helpCache,
        ILogger<GitCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override CliToolDefinition CreateToolDefinition() =>
        base.CreateToolDefinition() with
        {
            DocumentationOutputDirectory = null,
            GenerateCode = false,
        };

    public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return base.IsAvailableAsync(cancellationToken);
    }

    protected override async Task<string?> GetHelpTextAsync(
        string[] commandPath,
        CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        var cacheKey = string.Join(' ', commandPath);
        if (HelpCache.TryGet(cacheKey, out var cached))
        {
            return cached;
        }

        var arguments = commandPath.Length == 1
            ? "help -a"
            : $"{string.Join(' ', commandPath.Skip(1))} -h";
        var usesHelpRepository = commandPath.Length > 2;
        await _helpCommandUsage.WaitAsync(cancellationToken);

        CliCommandResult result;
        try
        {
            ThrowIfDisposed();
            var workingDirectory = usesHelpRepository
                ? await GetHelpRepositoryAsync()
                : null;
            result = await ExecuteAndRecordHelpCommandAsync(
                commandPath,
                ToolName,
                arguments,
                cancellationToken,
                workingDirectory);
        }
        finally
        {
            _helpCommandUsage.Release();
        }

        // Git sends short help to either stream and commonly exits with its usage code.
        var helpText = result.CombinedOutput;
        if (string.IsNullOrWhiteSpace(helpText))
        {
            Logger.LogWarning("No help text for command: {Command}", cacheKey);
            return null;
        }

        HelpCache.Set(cacheKey, helpText);
        return helpText;
    }

    private void ThrowIfDisposed()
    {
        lock (_helpRepositoryLock)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }
    }

    private Task<string> GetHelpRepositoryAsync()
    {
        lock (_helpRepositoryLock)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return _helpRepositoryTask ??= CreateHelpRepositoryAsync();
        }
    }

    private async Task<string> CreateHelpRepositoryAsync()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"git-scraper-{Guid.NewGuid():N}");
        _helpRepositoryDirectory = directory;
        Directory.CreateDirectory(directory);
        var result = await Executor.ExecuteAsync(
            ToolName,
            "init --quiet",
            CancellationToken.None,
            directory).ConfigureAwait(false);
        if (!result.Success)
        {
            throw new InvalidOperationException(
                $"Could not initialize the temporary Git help repository: {result.CombinedOutput}");
        }

        return directory;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Task<string>? initialization;
        lock (_helpRepositoryLock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            initialization = _helpRepositoryTask;
        }

        _helpCommandUsage.Wait();
        string? directory;
        try
        {
            try
            {
                initialization?.GetAwaiter().GetResult();
            }
            catch
            {
                // The caller observes initialization failures; disposal still owns cleanup.
            }

            lock (_helpRepositoryLock)
            {
                directory = _helpRepositoryDirectory;
                _helpRepositoryDirectory = null;
                _helpRepositoryTask = null;
            }
        }
        finally
        {
            _helpCommandUsage.Release();
        }

        if (directory is null || !Directory.Exists(directory))
        {
            return;
        }

        try
        {
            Directory.Delete(directory, recursive: true);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            Logger.LogWarning(
                exception,
                "Could not delete temporary Git help repository: {Directory}",
                directory);
        }

        GC.SuppressFinalize(this);
    }

    protected override IEnumerable<string> ExtractSubcommands(
        string[] commandPath,
        string helpText) =>
        commandPath.Length == 1
            ? ExtractTopLevelCommands(helpText)
            : ExtractSubcommands(commandPath.Skip(1).ToArray(), helpText);

    protected override bool HelpMatchesCommandPath(string[] commandPath, string helpText)
    {
        if (commandPath.Length == 1)
        {
            return true;
        }

        return helpText
            .Split('\n')
            .Select(line => UsageInvocationRegex().Match(line))
            .Where(match => match.Success)
            .Select(match => match.Groups[1].Value.Trim())
            .Any(invocation => TryConsumeCommandPath(invocation, commandPath, out _));
    }

    protected override bool HasOptions(string helpText) => true;

    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken) =>
        ParseCommandAsync(
            commandPath,
            helpText,
            ParseUsageSynopsis(commandPath, helpText),
            cancellationToken);

    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        UsageSynopsisParseResult usage,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray();
        var command = string.Join(' ', commandParts);
        var options = ParseOptions(helpText);
        var className = $"Git{string.Concat(commandParts.Select(ToPascalCase))}Options";
        var parentClassName = commandParts.Length == 1
            ? "GitOptions"
            : $"Git{string.Concat(commandParts.SkipLast(1).Select(ToPascalCase))}Options";

        return Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
        {
            FullCommand = $"git {command}",
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = parentClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = ExtractDescription(helpText, command),
            Options = options,
            PositionalArguments = usage.PositionalArguments,
            SubDomainGroup = null, // The handwritten Git facade owns command grouping.
        });
    }

    internal static IReadOnlyList<string> ExtractSubcommands(string command, string helpText) =>
        ExtractSubcommands([command], helpText);

    private static IReadOnlyList<string> ExtractSubcommands(
        IReadOnlyList<string> commandParts,
        string helpText)
    {
        var expectedCommandParts = new[] { "git" }.Concat(commandParts).ToArray();
        var requiredSubcommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var optionalSubcommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var line in helpText.Split('\n'))
        {
            var usage = UsageInvocationRegex().Match(line);
            if (!usage.Success)
            {
                continue;
            }

            var invocation = usage.Groups[1].Value.Trim();
            if (!TryConsumeCommandPath(invocation, expectedCommandParts, out var remainder))
            {
                continue;
            }

            if (TryExtractLeadingSubcommand(
                    remainder.AsSpan(),
                    out var candidate,
                    out _,
                    out var isOptional))
            {
                (isOptional ? optionalSubcommands : requiredSubcommands).Add(candidate);
            }
        }

        return requiredSubcommands
            .Concat(requiredSubcommands.Count == 0 ? [] : optionalSubcommands)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static IReadOnlyList<string> ExtractTopLevelCommands(string helpText)
    {
        var commands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var inCommandSection = false;

        foreach (var line in helpText.Split('\n'))
        {
            if (line.Contains("Main Porcelain Commands")
                || line.Contains("Ancillary Commands")
                || line.Contains("Interacting with Others")
                || line.Contains("Low-level Commands"))
            {
                inCommandSection = true;
                continue;
            }

            if (!inCommandSection
                || line.StartsWith("See ", StringComparison.Ordinal)
                || line.StartsWith("'git ", StringComparison.Ordinal)
                || line.Contains("concept", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var match = CommandLineRegex().Match(line);
            if (match.Success && !ShouldSkipCommand(match.Groups[1].Value))
            {
                commands.Add(match.Groups[1].Value);
            }
        }

        return commands.Order(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    private static bool TryConsumeCommandPath(
        string invocation,
        IReadOnlyList<string> expectedCommandParts,
        out string remainder)
    {
        var remaining = invocation.AsSpan();
        foreach (var expectedPart in expectedCommandParts)
        {
            if (!TryExtractLeadingSubcommand(
                    remaining,
                    out var actualPart,
                    out var consumed,
                    out _)
                || !actualPart.Equals(expectedPart, StringComparison.OrdinalIgnoreCase))
            {
                remainder = string.Empty;
                return false;
            }

            remaining = remaining[consumed..];
        }

        remainder = remaining.ToString();
        return true;
    }

    private static bool TryExtractLeadingSubcommand(
        ReadOnlySpan<char> text,
        out string command,
        out int consumed,
        out bool isOptional)
    {
        var remaining = text.TrimStart();
        var offset = text.Length - remaining.Length;
        while (!remaining.IsEmpty && remaining[0] == '[')
        {
            if (!TryReadBracketGroup(remaining, out var content, out var groupLength))
            {
                command = string.Empty;
                consumed = 0;
                isOptional = false;
                return false;
            }

            var optionalCommand = ExtractCommandToken(content, rejectAlternatives: true);
            if (optionalCommand is not null)
            {
                command = optionalCommand;
                consumed = offset + groupLength;
                isOptional = true;
                return true;
            }

            remaining = remaining[groupLength..].TrimStart();
            offset = text.Length - remaining.Length;
        }

        var requiredCommand = ExtractCommandToken(remaining, rejectAlternatives: false);
        if (requiredCommand is null)
        {
            command = string.Empty;
            consumed = 0;
            isOptional = false;
            return false;
        }

        command = requiredCommand;
        consumed = offset + requiredCommand.Length;
        isOptional = false;
        return true;
    }

    private static bool TryReadBracketGroup(
        ReadOnlySpan<char> text,
        out ReadOnlySpan<char> content,
        out int consumed)
    {
        var depth = 0;
        for (var index = 0; index < text.Length; index++)
        {
            depth += text[index] switch
            {
                '[' => 1,
                ']' => -1,
                _ => 0,
            };

            if (depth == 0)
            {
                content = text[1..index];
                consumed = index + 1;
                return true;
            }
        }

        content = default;
        consumed = 0;
        return false;
    }

    private static string? ExtractCommandToken(
        ReadOnlySpan<char> text,
        bool rejectAlternatives)
    {
        var value = text.TrimStart();
        if (value.IsEmpty
            || value[0] is '-' or '<' or '('
            || (rejectAlternatives && value.Contains('|')))
        {
            return null;
        }

        var length = 0;
        while (length < value.Length
               && (char.IsLetterOrDigit(value[length]) || value[length] is '-' or '_'))
        {
            length++;
        }

        if (length == 0 || !char.IsLetter(value[0]))
        {
            return null;
        }

        return value[..length].ToString();
    }

    /// <summary>
    /// Parses options from git's -h output format.
    /// Format: -short, --long   description
    /// Or: --long   description
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var lines = helpText.Split('\n');

        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];
            if (!TryParseOption(line, out var option, out var negatedLongFlag)
                || !seenOptions.Add(option.SwitchName))
            {
                continue;
            }

            if (string.IsNullOrEmpty(option.Description)
                && TryGetWrappedDescription(lines, index, out var description))
            {
                option = AddDescription(option, description);
                index++;
            }

            options.Add(option);
            if (negatedLongFlag is not null && seenOptions.Add(negatedLongFlag))
            {
                options.Add(CreateNegatedOption(option, negatedLongFlag));
            }
        }

        return options;
    }

    private static bool TryGetWrappedDescription(
        IReadOnlyList<string> lines,
        int declarationIndex,
        out string description)
    {
        description = "";
        if (declarationIndex + 1 >= lines.Count)
        {
            return false;
        }

        var declaration = lines[declarationIndex];
        var candidate = lines[declarationIndex + 1];
        description = candidate.Trim();
        return description.Length > 0
               && !description.StartsWith('-')
               && GetIndentation(candidate) > GetIndentation(declaration);
    }

    private static int GetIndentation(string value) =>
        value.TakeWhile(char.IsWhiteSpace).Count();

    private static CliOptionDefinition AddDescription(
        CliOptionDefinition option,
        string description)
    {
        return option with
        {
            Description = description,
            IsSecret = GeneratorUtils.IsSecretOption(option.PropertyName, option.IsFlag),
        };
    }

    private static bool TryParseOption(
        string line,
        out CliOptionDefinition option,
        out string? negatedLongFlag)
    {
        option = null!;
        negatedLongFlag = null;
        var match = OptionLineRegex().Match(line);
        if (!match.Success)
        {
            return false;
        }

        var identity = GetOptionIdentity(match);
        if (identity is null)
        {
            return false;
        }

        var (shortFlag, primaryFlag, propertyName, negatedFlag) = identity.Value;
        negatedLongFlag = negatedFlag;
        var description = GetDescription(match);
        var valueSyntax = GetGroupValue(match, "value");
        var (csharpType, isFlag) = InferOptionType(primaryFlag, description, line, valueSyntax);

        option = new CliOptionDefinition
        {
            SwitchName = primaryFlag,
            ShortForm = shortFlag,
            Description = description,
            PropertyName = propertyName,
            CSharpType = csharpType,
            IsRequired = false,
            IsFlag = isFlag,
            ValueArity = valueSyntax?.StartsWith('[') == true
                ? CliOptionValueArity.Optional
                : CliOptionValueArity.Required,
            ValueSeparator = valueSyntax switch
            {
                { } value when value.Contains('=') => "=",
                { } value when value.StartsWith('[') => string.Empty,
                _ => " ",
            },
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
        };
        return true;
    }

    private static OptionIdentity? GetOptionIdentity(Match match)
    {
        var shortFlag = GetShortFlag(match);
        var advertisedLongFlag = GetGroupValue(match, "long");
        var primaryFlag = advertisedLongFlag is null
            ? shortFlag
            : NormalizeNegatableLongFlag(advertisedLongFlag);
        var propertyName = primaryFlag is null ? null : NormalizeGitPropertyName(primaryFlag);
        return primaryFlag is null || propertyName is null
            ? null
            : new OptionIdentity(
                shortFlag,
                primaryFlag,
                propertyName,
                advertisedLongFlag is null ? null : GetNegatedLongFlag(advertisedLongFlag));
    }

    private static (string CSharpType, bool IsFlag) InferOptionType(
        string primaryFlag,
        string description,
        string line,
        string? valueSyntax)
    {
        var (csharpType, isFlag) = InferType(primaryFlag, description, line);
        return valueSyntax is null
            ? (csharpType, isFlag)
            : (isFlag ? "string?" : csharpType, false);
    }

    private static string? GetGroupValue(Match match, string name) =>
        match.Groups[name] is { Success: true, Value.Length: > 0 } group
            ? group.Value.Trim()
            : null;

    private static string? GetShortFlag(Match match) =>
        GetGroupValue(match, "short") ?? GetGroupValue(match, "shortOnly");

    private static string GetDescription(Match match) =>
        GetGroupValue(match, "description") ?? GetGroupValue(match, "shortDescription") ?? "";

    private readonly record struct OptionIdentity(
        string? ShortFlag,
        string PrimaryFlag,
        string PropertyName,
        string? NegatedFlag);

    private static CliOptionDefinition CreateNegatedOption(
        CliOptionDefinition option,
        string negatedLongFlag)
    {
        var negatedPropertyName = NormalizeGitPropertyName(negatedLongFlag)!;
        return option with
        {
            SwitchName = negatedLongFlag,
            ShortForm = null,
            PropertyName = negatedPropertyName,
            CSharpType = "bool?",
            Description = $"Negates {option.SwitchName}. {option.Description}",
            IsFlag = true,
            ValueArity = CliOptionValueArity.Required,
            ValueSeparator = " ",
            IsSecret = GeneratorUtils.IsSecretOption(negatedPropertyName, isFlag: true),
        };
    }

    /// <summary>
    /// Extracts a description from the usage line.
    /// </summary>
    private static string ExtractDescription(string helpText, string command)
    {
        // Try to find the usage line and extract a meaningful description
        var lines = helpText.Split('\n');
        var usageLine = lines.FirstOrDefault(l => l.TrimStart().StartsWith("usage:", StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(usageLine))
        {
            // Usage line format: usage: git command [options] [args]
            return $"Execute git {command} command";
        }

        return $"Git {command} command";
    }

    /// <summary>
    /// Infers C# type from option name and description.
    /// </summary>
    private static (string CSharpType, bool IsBoolean) InferType(string optionName, string description, string fullLine)
    {
        var lowerDesc = description.ToLowerInvariant();
        var lowerOpt = optionName.ToLowerInvariant();

        // Check if it takes a value (has <placeholder> or =value)
        if (fullLine.Contains('<') && fullLine.Contains('>'))
        {
            // Has a placeholder, it's a value option
            if (fullLine.Contains("<n>") || fullLine.Contains("<num>") ||
                fullLine.Contains("<count>") || fullLine.Contains("<number>") ||
                lowerDesc.Contains("number") || lowerDesc.Contains("count"))
            {
                return ("int?", false);
            }

            return ("string?", false);
        }

        // Check for [=<value>] pattern (optional value)
        if (fullLine.Contains("[="))
        {
            return ("string?", false);
        }

        // Check for common boolean patterns
        if (fullLine.Contains("--[no-]", StringComparison.Ordinal) ||
            lowerOpt.StartsWith("no-") ||
            lowerDesc.Contains("disable") ||
            lowerDesc.Contains("enable") ||
            lowerDesc.Contains("toggle") ||
            lowerDesc == "" || // Options without description are usually flags
            (lowerDesc.Length < 50 && !lowerDesc.Contains("set") && !lowerDesc.Contains("specify")))
        {
            return ("bool?", true);
        }

        // Default to string for anything else
        return ("string?", false);
    }

    /// <summary>
    /// Normalizes an option name to a C# property name.
    /// </summary>
    private static string? NormalizeGitPropertyName(string optionName)
    {
        var cleaned = optionName.TrimStart('-');
        if (string.IsNullOrWhiteSpace(cleaned))
        {
            return null;
        }

        // Remove <placeholder> patterns (e.g., --prune[=<date>] -> prune)
        var placeholderIndex = cleaned.IndexOf('<');
        if (placeholderIndex >= 0)
        {
            cleaned = cleaned[..placeholderIndex];
        }

        // Handle special characters
        cleaned = cleaned.Replace("=", "").Replace("[", "").Replace("]", "");

        var parts = cleaned.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return null;
        }

        var result = string.Join("", parts.Select(ToPascalCase));

        // Handle identifiers that start with a number
        if (!string.IsNullOrEmpty(result) && char.IsDigit(result[0]))
        {
            result = ConvertLeadingDigitToWord(result);
        }

        return result;
    }

    private static string NormalizeNegatableLongFlag(string optionName) =>
        optionName.StartsWith("--[no-]", StringComparison.Ordinal)
            ? "--" + optionName[7..]
            : optionName;

    private static string? GetNegatedLongFlag(string optionName) =>
        optionName.StartsWith("--[no-]", StringComparison.Ordinal)
            ? "--no-" + optionName[7..]
            : null;

    /// <summary>
    /// Converts a leading digit to a word (e.g., "3way" -> "ThreeWay").
    /// </summary>
    private static string ConvertLeadingDigitToWord(string input)
    {
        if (string.IsNullOrEmpty(input) || !char.IsDigit(input[0]))
        {
            return input;
        }

        var digitWords = new Dictionary<char, string>
        {
            ['0'] = "Zero",
            ['1'] = "One",
            ['2'] = "Two",
            ['3'] = "Three",
            ['4'] = "Four",
            ['5'] = "Five",
            ['6'] = "Six",
            ['7'] = "Seven",
            ['8'] = "Eight",
            ['9'] = "Nine"
        };

        if (digitWords.TryGetValue(input[0], out var word))
        {
            return word + input[1..];
        }

        return input;
    }

    /// <summary>
    /// Determines if a command should be skipped.
    /// </summary>
    private static bool ShouldSkipCommand(string command)
    {
        // Skip internal/plumbing commands that start with certain patterns
        var skipPrefixes = new[] { "git-", "credential-", "remote-" };
        var skipCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "gitk", "git-gui", "gui", "citool", "gitweb",
            "shell", "http-backend",
            // Skip very low-level plumbing
            "check-attr", "check-mailmap",
            "check-ref-format", "column", "credential", "credential-cache",
            "credential-store", "fmt-merge-msg", "get-tar-commit-id",
            "http-fetch", "http-push", "index-pack",
            "interpret-trailers", "ls-remote", "mailinfo",
            "mailsplit", "merge-file", "merge-index",
            "merge-one-file", "merge-tree", "mktag", "mktree",
            "multi-pack-index", "name-rev", "pack-objects", "pack-redundant",
            "pack-refs", "patch-id", "prune-packed", "quiltimport",
            "receive-pack",
            "send-pack", "sh-i18n--envsubst", "sh-setup", "show-index",
            "stripspace", "unpack-file", "unpack-objects",
            "upload-archive", "upload-pack", "var", "verify-commit", "verify-tag"
        };

        if (skipCommands.Contains(command))
        {
            return true;
        }

        foreach (var prefix in skipPrefixes)
        {
            if (command.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Regex to match command lines in 'git help -a' output.
    /// Format: 3 spaces + command name + spaces + description
    /// </summary>
    [GeneratedRegex(@"^\s{3}(\S+)\s+")]
    private static partial Regex CommandLineRegex();

    /// <summary>
    /// Regex to match option lines in git help output.
    /// Formats:
    /// -x, --xxx   description
    /// --xxx   description
    /// -x   description
    /// </summary>
    [GeneratedRegex(@"^\s+(?:(?<short>-\w),\s+)?(?<long>--(?:\[no-\])?[\w-]+)(?<value>\[?=\S+\]?|\s+(?:<[^>]+>|\.{3}|\([^\s|)]+(?:\|[^\s|)]+)+\)\S*))?(?:\s+(?<description>.*))?$|^\s+(?<shortOnly>-\w)(?<value>\[?=\S+\]?|\[<[^>]+>\]|\s+(?:<[^>]+>|\.{3}|\([^\s|)]+(?:\|[^\s|)]+)+\)\S*))?(?:\s+(?<shortDescription>.*))?$")]
    private static partial Regex OptionLineRegex();

    [GeneratedRegex(@"^\s*(?:usage:|or:)\s+(.+)$", RegexOptions.IgnoreCase)]
    private static partial Regex UsageInvocationRegex();
}
