using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI scraper for Git commands.
/// Git uses a different help format than Cobra-style CLIs:
/// - Commands discovered via 'git help -a'
/// - Options via 'git <command> -h' with format: -short, --long   description
/// - Flat command structure (no deep nesting)
/// </summary>
public partial class GitCliScraper : ICliScraper
{
    private readonly ICliCommandExecutor _executor;
    private readonly ILogger<GitCliScraper> _logger;

    public string ToolName => "git";
    public string NamespacePrefix => "Git";
    public string TargetNamespace => "ModularPipelines.Git";
    public string OutputDirectory => "src/ModularPipelines.Git";

    public GitCliScraper(ICliCommandExecutor executor, ILogger<GitCliScraper> logger)
    {
        _executor = executor;
        _logger = logger;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await _executor.IsAvailableAsync("git", cancellationToken);
    }

    public CliToolDefinition CreateToolDefinition()
    {
        return new CliToolDefinition
        {
            ToolName = "git",
            NamespacePrefix = "Git",
            TargetNamespace = "ModularPipelines.Git",
            OutputDirectory = "src/ModularPipelines.Git",
            Commands = [],
            Errors = []
        };
    }

    public async IAsyncEnumerable<CliCommandDefinition> ScrapeAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Discovering git commands via 'git help -a'...");

        if (!await IsAvailableAsync(cancellationToken))
        {
            _logger.LogError("Git is not available on this system");
            yield break;
        }

        // Get all commands from 'git help -a'
        var commands = await DiscoverCommandsAsync(cancellationToken);
        _logger.LogInformation("Discovered {Count} git commands", commands.Count);

        var commandCount = 0;
        foreach (var command in commands)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var definition = await ParseCommandAsync(command, cancellationToken);
            if (definition is not null)
            {
                commandCount++;
                _logger.LogInformation("Yielding command {Count}: git {Command}", commandCount, command);
                yield return definition;
            }
        }

        _logger.LogInformation("Finished scraping git. Total commands: {Count}", commandCount);
    }

    /// <summary>
    /// Discovers all git commands from 'git help -a'.
    /// </summary>
    private async Task<List<string>> DiscoverCommandsAsync(CancellationToken cancellationToken)
    {
        var result = await _executor.ExecuteAsync("git", "help -a", cancellationToken);
        var output = result.StandardOutput ?? result.StandardError ?? "";

        var commands = new List<string>();

        // Parse the help -a output to extract command names
        // Format is like:
        //    add                     Add file contents to the index
        //    am                      Apply a series of patches from a mailbox
        var lines = output.Split('\n');
        var inCommandSection = false;

        foreach (var line in lines)
        {
            // Skip empty lines
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            // Detect section headers
            if (line.Contains("Main Porcelain Commands") ||
                line.Contains("Ancillary Commands") ||
                line.Contains("Interacting with Others") ||
                line.Contains("Low-level Commands"))
            {
                inCommandSection = true;
                continue;
            }

            // Skip non-command lines
            if (line.StartsWith("See ") || line.StartsWith("'git ") || line.Contains("concept"))
            {
                continue;
            }

            // Parse command lines (3 spaces + command name + spaces + description)
            if (inCommandSection)
            {
                var match = CommandLineRegex().Match(line);
                if (match.Success)
                {
                    var commandName = match.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(commandName) && !ShouldSkipCommand(commandName))
                    {
                        commands.Add(commandName);
                    }
                }
            }
        }

        return commands.Distinct().OrderBy(c => c).ToList();
    }

    /// <summary>
    /// Parses a single git command's options from 'git <command> -h'.
    /// </summary>
    private async Task<CliCommandDefinition?> ParseCommandAsync(string command, CancellationToken cancellationToken)
    {
        // Get help text using -h (short help, no pager)
        var result = await _executor.ExecuteAsync("git", $"{command} -h", cancellationToken);

        // Git outputs help to stderr for -h
        var helpText = result.StandardError ?? result.StandardOutput ?? "";

        if (string.IsNullOrWhiteSpace(helpText))
        {
            _logger.LogWarning("No help text for git {Command}", command);
            return null;
        }

        // Extract description from usage line
        var description = ExtractDescription(helpText, command);

        // Parse options
        var options = ParseOptions(helpText);

        if (options.Count == 0)
        {
            _logger.LogDebug("No options found for git {Command}", command);
            // Still create command if it has a description (it might just have positional args)
        }

        var className = $"Git{ToPascalCase(command)}Options";

        return new CliCommandDefinition
        {
            FullCommand = $"git {command}",
            CommandParts = [command],
            ClassName = className,
            ParentClassName = "GitOptions",
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            Options = options,
            SubDomainGroup = null // Git commands are flat, no sub-domains
        };
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

        foreach (var line in lines)
        {
            // Match option lines: -x, --xxx or just --xxx
            var match = OptionLineRegex().Match(line);
            if (!match.Success)
            {
                continue;
            }

            string? shortFlag = null;
            string? longFlag = null;
            var description = "";

            // Check if we have a short flag
            if (match.Groups[1].Success && !string.IsNullOrEmpty(match.Groups[1].Value))
            {
                shortFlag = match.Groups[1].Value.Trim();
            }

            // Long flag
            if (match.Groups[2].Success)
            {
                longFlag = match.Groups[2].Value.Trim();
            }

            // Description
            if (match.Groups[3].Success)
            {
                description = match.Groups[3].Value.Trim();
            }

            // Use long flag preferably, fall back to short
            var primaryFlag = longFlag ?? shortFlag;
            if (string.IsNullOrEmpty(primaryFlag))
            {
                continue;
            }

            // Skip duplicates
            if (seenOptions.Contains(primaryFlag))
            {
                continue;
            }
            seenOptions.Add(primaryFlag);

            // Generate property name
            var propertyName = NormalizePropertyName(primaryFlag);
            if (string.IsNullOrEmpty(propertyName))
            {
                continue;
            }

            // Determine type based on the option
            var (csharpType, isFlag) = InferType(primaryFlag, description, line);

            // SwitchName should be the full flag with dashes (--long-flag)
            var switchName = longFlag ?? shortFlag;

            options.Add(new CliOptionDefinition
            {
                SwitchName = switchName!,
                ShortForm = shortFlag,
                Description = description,
                PropertyName = propertyName,
                CSharpType = csharpType,
                IsRequired = false,
                IsFlag = isFlag
            });
        }

        return options;
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
        if (lowerOpt.StartsWith("no-") ||
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
    private static string? NormalizePropertyName(string optionName)
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
    /// Converts to PascalCase.
    /// </summary>
    private static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Handle special git command names
        input = input.Replace("-", " ").Replace("_", " ");
        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return string.Join("", words.Select(w =>
            char.ToUpperInvariant(w[0]) + (w.Length > 1 ? w[1..].ToLowerInvariant() : "")));
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
            "gitk", "git-gui", "gui", "citool", "gitweb", "instaweb",
            "shell", "http-backend", "daemon",
            // Skip very low-level plumbing
            "cat-file", "check-attr", "check-ignore", "check-mailmap",
            "check-ref-format", "column", "credential", "credential-cache",
            "credential-store", "fmt-merge-msg", "get-tar-commit-id",
            "hash-object", "http-fetch", "http-push", "index-pack",
            "interpret-trailers", "ls-remote", "ls-tree", "mailinfo",
            "mailsplit", "merge-base", "merge-file", "merge-index",
            "merge-one-file", "merge-tree", "mktag", "mktree",
            "multi-pack-index", "name-rev", "pack-objects", "pack-redundant",
            "pack-refs", "patch-id", "prune-packed", "quiltimport",
            "read-tree", "receive-pack", "rev-list", "rev-parse",
            "send-pack", "sh-i18n--envsubst", "sh-setup", "show-index",
            "show-ref", "stripspace", "symbolic-ref", "unpack-file",
            "unpack-objects", "update-index", "update-ref", "update-server-info",
            "upload-archive", "upload-pack", "var", "verify-commit",
            "verify-pack", "verify-tag", "write-tree"
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
    [GeneratedRegex(@"^\s+(?:(-\w),\s+)?(--[\w-]+(?:\[?=[\w<>\[\]]+\]?)?)\s+(.*)$|^\s+(-\w)\s+(.*)$")]
    private static partial Regex OptionLineRegex();
}
