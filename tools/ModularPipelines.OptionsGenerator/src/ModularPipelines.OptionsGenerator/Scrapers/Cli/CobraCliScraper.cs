using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Cobra-based CLIs (helm, docker, kubectl).
/// Cobra has a consistent help format that makes it ideal for CLI-first parsing.
///
/// Cobra help format:
/// Usage:
///   command [subcommand] [flags]
///
/// Available Commands:
///   subcommand    Description
///
/// Flags:
///   -s, --long type    Description
///       --flag         Boolean flag
///
/// Global Flags:
///   -g, --global type  Global option
/// </summary>
public abstract partial class CobraCliScraper : CliScraperBase
{
    protected CobraCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger logger)
        : base(executor, helpCache, logger)
    {
    }

    // ScrapeAsync is now provided by CliScraperBase - no need to override

    /// <summary>
    /// Extracts subcommand names from Cobra-style help text.
    /// Handles multiple command sections (Common Commands, Management Commands, etc.)
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find ALL command sections (Common Commands, Management Commands, Swarm Commands, Commands)
        var commandsSectionMatches = CommandsSectionPattern().Matches(helpText);
        if (commandsSectionMatches.Count == 0)
        {
            return subcommands;
        }

        foreach (Match commandsSectionMatch in commandsSectionMatches)
        {
            var sectionStart = commandsSectionMatch.Index + commandsSectionMatch.Length;

            // Find where this section ends (next section header or end of text)
            var sectionEnd = helpText.Length;
            var nextSectionMatch = SectionHeaderPattern().Match(helpText, sectionStart);
            if (nextSectionMatch.Success)
            {
                sectionEnd = nextSectionMatch.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);

            // Parse command lines: "  command    description"
            var lines = section.Split('\n');
            foreach (var line in lines)
            {
                var match = SubcommandLinePattern().Match(line);
                if (match.Success)
                {
                    var commandName = match.Groups["name"].Value.Trim();
                    if (!string.IsNullOrEmpty(commandName) && !commandName.Contains(' ') &&
                        seenCommands.Add(commandName))
                    {
                        subcommands.Add(commandName);
                    }
                }
            }
        }

        return subcommands;
    }

    /// <summary>
    /// Parses a Cobra command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray(); // Skip tool name

        if (commandParts.Length == 0)
        {
            // This is just the root command, skip it
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        // Skip commands with invalid parts (e.g., "docker --tlsverify container", "docker run HEALTHCHECK")
        if (commandParts.Any(part => part.StartsWith("--") || !IsValidCommandPart(part)))
        {
            Logger.LogDebug("Skipping invalid command path: {Command}", string.Join(" ", commandPath));
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        // Validate that help output represents a real command (not an error or wrong command)
        if (!IsValidCommandHelp(helpText, commandParts))
        {
            Logger.LogDebug("Skipping command with invalid help output: {Command}", string.Join(" ", commandPath));
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        // Get the first subcommand for grouping
        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;

        // Parse description from synopsis or first line
        var description = ExtractDescription(helpText);

        // Parse options from the help text
        var options = ParseOptions(helpText, commandParts);

        // Parse positional arguments from usage line
        var positionalArgs = ParsePositionalArguments(helpText);

        // Extract enums from options
        var enums = options
            .Where(o => o.EnumDefinition is not null)
            .Select(o => o.EnumDefinition!)
            .ToList();

        var className = GenerateClassName(commandPath);

        var command = new CliCommandDefinition
        {
            FullCommand = string.Join(" ", commandPath),
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = null, // CLI-first, no URL
            Options = options,
            PositionalArguments = positionalArgs,
            SubDomainGroup = subDomain,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Extracts description from help text.
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        var lines = helpText.Split('\n');

        // Skip empty lines and look for the first non-usage, non-section line
        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (string.IsNullOrEmpty(trimmed))
            {
                continue;
            }

            // Skip section headers
            if (trimmed.EndsWith(':') ||
                trimmed.StartsWith("Usage:") ||
                trimmed.StartsWith("Examples:") ||
                trimmed.StartsWith("Available Commands:") ||
                trimmed.StartsWith("Flags:") ||
                trimmed.StartsWith("Global Flags:"))
            {
                continue;
            }

            // Skip usage lines
            if (trimmed.Contains("[command]") ||
                trimmed.Contains("[flags]") ||
                trimmed.StartsWith("> ") ||
                trimmed.StartsWith("$ "))
            {
                continue;
            }

            // Found a description line
            if (trimmed.Length > 10)
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from Cobra-style help text.
    /// Handles both standard Cobra format (Docker, Helm) and kubectl's variant.
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var className = GenerateClassName([ToolName, ..commandParts]);

        // Find Flags, Options, and Global Flags sections
        var flagsSections = ExtractFlagsSections(helpText);

        // Also check for "Options:" section (kubectl style)
        var optionsSections = ExtractOptionsSections(helpText);
        flagsSections.AddRange(optionsSections);

        foreach (var section in flagsSections)
        {
            var lines = section.Split('\n');
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                // Try standard Cobra pattern first (Docker, Helm)
                var match = CobraOptionPattern().Match(line);

                // If not matched, try kubectl pattern: "-s, --long=default:"
                if (!match.Success)
                {
                    match = KubectlOptionPattern().Match(line);
                }

                if (!match.Success)
                {
                    continue;
                }

                var shortForm = match.Groups["short"].Value.Trim();
                var longForm = match.Groups["long"].Value.Trim();
                var typeHint = match.Groups["type"].Value.Trim();
                var description = match.Groups["desc"].Value.Trim();

                // Accumulate multi-line descriptions
                // Look ahead for continuation lines that are indented but don't start with a dash
                i = AccumulateMultiLineDescription(lines, i, ref description);

                if (string.IsNullOrEmpty(longForm))
                {
                    continue;
                }

                // Skip duplicates
                if (seenOptions.Contains(longForm))
                {
                    continue;
                }

                seenOptions.Add(longForm);

                var propertyName = NormalizePropertyName(longForm);
                if (propertyName is null)
                {
                    continue;
                }

                // Determine types - check if typeHint contains default value (kubectl style)
                var actualType = typeHint;
                if (typeHint.Contains('='))
                {
                    // kubectl format: --option=defaultValue: where typeHint might be "false" or "500"
                    var defaultValue = typeHint.TrimEnd(':');
                    if (defaultValue.ToLowerInvariant() is "true" or "false")
                    {
                        actualType = "bool";
                    }
                    else if (int.TryParse(defaultValue, out _))
                    {
                        actualType = "int";
                    }
                    else
                    {
                        actualType = "string";
                    }
                }

                var isFlag = string.IsNullOrEmpty(actualType) || IsKnownBooleanType(actualType);
                var isInteger = IsKnownIntegerType(actualType);
                var isFloat = IsKnownFloatType(actualType);
                var isDuration = IsKnownDurationType(actualType);
                var isArray = IsKnownArrayType(actualType);
                var isKeyValue = IsKnownKeyValueType(actualType);

                // Try to detect enum values
                var enumDef = TryDetectEnumFromDescription(propertyName, className, actualType, description);

                var csharpType = DetermineCSharpType(isFlag, isArray, isKeyValue, isInteger, isFloat, isDuration, enumDef);
                var separator = isFlag ? " " : "=";

                options.Add(new CliOptionDefinition
                {
                    SwitchName = longForm,
                    ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                    PropertyName = propertyName,
                    CSharpType = csharpType,
                    Description = description,
                    IsFlag = isFlag,
                    IsRequired = false,
                    AcceptsMultipleValues = isArray,
                    IsKeyValue = isKeyValue,
                    IsNumeric = isInteger || isFloat,
                    ValueSeparator = separator,
                    EnumDefinition = enumDef
                });
            }
        }

        return options;
    }

    /// <summary>
    /// Extracts the Flags and Global Flags sections from help text.
    /// </summary>
    private static List<string> ExtractFlagsSections(string helpText)
    {
        var sections = new List<string>();

        var flagMatches = FlagsSectionPattern().Matches(helpText);
        foreach (Match match in flagMatches)
        {
            var sectionStart = match.Index + match.Length;

            // Find where this section ends
            var sectionEnd = helpText.Length;
            var nextMatch = SectionHeaderPattern().Match(helpText, sectionStart);
            if (nextMatch.Success)
            {
                sectionEnd = nextMatch.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
            sections.Add(section);
        }

        return sections;
    }

    /// <summary>
    /// Extracts "Options:" sections from help text (kubectl style).
    /// </summary>
    private static List<string> ExtractOptionsSections(string helpText)
    {
        var sections = new List<string>();

        var optionsMatch = OptionsSectionPattern().Match(helpText);
        if (optionsMatch.Success)
        {
            var sectionStart = optionsMatch.Index + optionsMatch.Length;

            // Find where this section ends
            var sectionEnd = helpText.Length;
            var nextMatch = SectionHeaderPattern().Match(helpText, sectionStart);
            if (nextMatch.Success)
            {
                sectionEnd = nextMatch.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
            sections.Add(section);
        }

        return sections;
    }

    /// <summary>
    /// Accumulates multi-line descriptions by looking ahead for continuation lines.
    /// Continuation lines are identified as:
    /// - Indented lines that don't start with a dash (not a new option)
    /// - Not empty lines (stop at blank lines)
    /// - Not section headers (lines ending with ':')
    /// </summary>
    /// <param name="lines">All lines in the section.</param>
    /// <param name="currentIndex">Current line index.</param>
    /// <param name="description">Description to accumulate into (ref).</param>
    /// <returns>The new line index after consuming continuation lines.</returns>
    private static int AccumulateMultiLineDescription(string[] lines, int currentIndex, ref string description)
    {
        var descriptionParts = new List<string>();
        if (!string.IsNullOrEmpty(description))
        {
            descriptionParts.Add(description);
        }

        // Look ahead for continuation lines
        var nextIndex = currentIndex + 1;
        while (nextIndex < lines.Length)
        {
            var nextLine = lines[nextIndex];
            var trimmedNext = nextLine.Trim();

            // Stop conditions:
            // 1. Empty line (paragraph break)
            if (string.IsNullOrWhiteSpace(trimmedNext))
            {
                break;
            }

            // 2. New option line (starts with dash)
            if (trimmedNext.StartsWith('-'))
            {
                break;
            }

            // 3. Section header (ends with ':' and is relatively short)
            if (trimmedNext.EndsWith(':') && trimmedNext.Length < 50)
            {
                break;
            }

            // 4. Line is not indented enough (less than ~20 spaces means it's likely a new element)
            // Continuation lines in Cobra help are typically deeply indented
            var leadingSpaces = nextLine.Length - nextLine.TrimStart().Length;
            if (leadingSpaces < 20 && !string.IsNullOrEmpty(description))
            {
                // If we already have a description and this line is not deeply indented,
                // it might be a new option without dashes or something else
                break;
            }

            // This is a continuation line - add it to the description
            descriptionParts.Add(trimmedNext);
            nextIndex++;
        }

        // Join all parts with space
        description = string.Join(" ", descriptionParts);

        // Return the last consumed index (loop will increment by 1)
        return nextIndex - 1;
    }

    /// <summary>
    /// Tries to detect enum values from description.
    /// </summary>
    private static CliEnumDefinition? TryDetectEnumFromDescription(
        string propertyName,
        string className,
        string typeHint,
        string description)
    {
        // Pattern 1: "One of: json|yaml|table"
        var oneOfMatch = OneOfPattern().Match(description);
        if (oneOfMatch.Success)
        {
            var values = ParseEnumValues(oneOfMatch.Groups["values"].Value);
            if (values.Length > 0)
            {
                return CreateEnumDefinition(propertyName, className, values);
            }
        }

        // Pattern 2: "(json, yaml, table)" or "(json|yaml|table)"
        var parenMatch = ParenthesizedValuesPattern().Match(description);
        if (parenMatch.Success)
        {
            var values = ParseEnumValues(parenMatch.Groups["values"].Value);
            if (values.Length >= 2 && values.Length <= 10 && values.All(IsValidEnumValue))
            {
                return CreateEnumDefinition(propertyName, className, values);
            }
        }

        // Pattern 3: Type hint contains pipe-separated values
        if (typeHint.Contains('|'))
        {
            var values = ParseEnumValues(typeHint);
            if (values.Length >= 2 && values.All(IsValidEnumValue))
            {
                return CreateEnumDefinition(propertyName, className, values);
            }
        }

        return null;
    }

    private static string[] ParseEnumValues(string valuesText)
    {
        return valuesText
            .Replace(" or ", "|")
            .Replace(" and ", "|")
            .Split(['|', ','], StringSplitOptions.RemoveEmptyEntries)
            .Select(v => v.Trim().Trim('"', '\'', '`'))
            .Where(v => !string.IsNullOrWhiteSpace(v) && v.Length < 30)
            .Distinct()
            .ToArray();
    }

    private static bool IsValidEnumValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        // Reject purely numeric values or ranges (e.g., "0", "0-3", "1,2,3")
        // These are typically examples for numeric inputs, not enum choices
        if (value.All(c => char.IsDigit(c) || c == '-' || c == ',' || c == '.'))
        {
            return false;
        }

        // Must start with a letter to be a valid enum name
        // (after normalization, leading digits would create invalid identifiers)
        if (!char.IsLetter(value[0]))
        {
            return false;
        }

        return value.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_');
    }

    private static CliEnumDefinition CreateEnumDefinition(
        string propertyName,
        string className,
        string[] values)
    {
        var enumName = $"{className.Replace("Options", "")}{propertyName}";

        return new CliEnumDefinition
        {
            EnumName = enumName,
            Values = values.Select(v => new CliEnumValue
            {
                MemberName = NormalizeEnumMemberName(v),
                CliValue = v
            }).ToList(),
            Description = $"Allowed values for the --{propertyName.ToLowerInvariant()} option."
        };
    }

    private static string NormalizeEnumMemberName(string value)
    {
        var cleaned = value.Replace("-", "_").Replace(".", "_");
        var parts = cleaned.Split('_', StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", parts.Select(ToPascalCase));
    }

    /// <summary>
    /// Parses positional arguments from usage line.
    /// </summary>
    private static List<CliPositionalArgument> ParsePositionalArguments(string helpText)
    {
        var args = new List<CliPositionalArgument>();

        // Find usage line
        var usageMatch = UsageLinePattern().Match(helpText);
        if (!usageMatch.Success)
        {
            return args;
        }

        var usageLine = usageMatch.Groups["usage"].Value;

        // Find positional arguments in brackets: [RELEASE] [CHART] <NAME>
        var argMatches = PositionalArgPattern().Matches(usageLine);
        var position = 0;

        foreach (Match match in argMatches)
        {
            var argName = match.Groups["name"].Value;
            var isRequired = match.Value.StartsWith('<'); // <NAME> is required, [NAME] is optional
            var isMultiple = match.Value.EndsWith("...");

            var propertyName = NormalizePropertyName(argName);
            if (propertyName is null)
            {
                continue;
            }

            var csharpType = isMultiple ? "IEnumerable<string>?" : "string?";
            if (isRequired)
            {
                csharpType = csharpType.TrimEnd('?');
            }

            args.Add(new CliPositionalArgument
            {
                PropertyName = propertyName,
                PlaceholderName = argName,
                CSharpType = csharpType,
                IsRequired = isRequired,
                PositionIndex = position++,
                Description = null
            });
        }

        return args;
    }

    #region Type Detection - HashSet-based for extensibility

    /// <summary>
    /// Known boolean type hints.
    /// </summary>
    private static readonly HashSet<string> KnownBooleanTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "bool", "boolean"
    };

    /// <summary>
    /// Known integer type hints.
    /// </summary>
    private static readonly HashSet<string> KnownIntegerTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "int", "int32", "int64", "uint", "uint32", "uint64", "integer", "number", "count"
    };

    /// <summary>
    /// Known floating-point type hints.
    /// </summary>
    private static readonly HashSet<string> KnownFloatTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "float", "float32", "float64", "double", "decimal", "number64", "real"
    };

    /// <summary>
    /// Known duration/time type hints.
    /// </summary>
    private static readonly HashSet<string> KnownDurationTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "duration", "time", "timeout", "interval"
    };

    /// <summary>
    /// Known array/list type hints.
    /// </summary>
    private static readonly HashSet<string> KnownArrayTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "list", "array", "strings", "stringarray", "stringslice", "slice"
    };

    /// <summary>
    /// Known key-value/map type hints.
    /// </summary>
    private static readonly HashSet<string> KnownKeyValueTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "map", "stringtostring", "keyvalue", "dict", "dictionary", "mapping"
    };

    private static bool IsKnownBooleanType(string typeHint)
    {
        return string.IsNullOrEmpty(typeHint) || KnownBooleanTypes.Contains(typeHint);
    }

    private static bool IsKnownIntegerType(string typeHint)
    {
        return KnownIntegerTypes.Contains(typeHint);
    }

    private static bool IsKnownFloatType(string typeHint)
    {
        return KnownFloatTypes.Contains(typeHint);
    }

    private static bool IsKnownDurationType(string typeHint)
    {
        return KnownDurationTypes.Contains(typeHint);
    }

    private static bool IsKnownArrayType(string typeHint)
    {
        return KnownArrayTypes.Contains(typeHint);
    }

    private static bool IsKnownKeyValueType(string typeHint)
    {
        return KnownKeyValueTypes.Contains(typeHint);
    }

    #endregion

    private static string DetermineCSharpType(
        bool isFlag,
        bool isArray,
        bool isKeyValue,
        bool isInteger,
        bool isFloat,
        bool isDuration,
        CliEnumDefinition? enumDef)
    {
        if (isFlag) return "bool?";
        if (enumDef is not null) return $"{enumDef.EnumName}?";
        if (isKeyValue) return "KeyValue[]?";
        if (isArray) return "IEnumerable<string>?";
        if (isInteger) return "int?";
        if (isFloat) return "double?";
        if (isDuration) return "string?"; // Duration as string (e.g., "30s", "5m") - CLI handles parsing
        return "string?";
    }

    // Regex patterns

    /// <summary>
    /// Matches command section headers like "Available Commands:", "Commands:",
    /// "Common Commands:", "Management Commands:", "Swarm Commands:", etc.
    /// </summary>
    [GeneratedRegex(@"(?:Available\s+|Common\s+|Management\s+|Swarm\s+)?Commands?:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches section headers like "Flags:", "Usage:", etc.
    /// </summary>
    [GeneratedRegex(@"^[A-Z][\w\s]*:\s*$", RegexOptions.Multiline)]
    private static partial Regex SectionHeaderPattern();

    /// <summary>
    /// Matches subcommand lines: "  command    description"
    /// Also handles Docker's asterisk markers for extensions: "  buildx*    description"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\*?\s{2,}", RegexOptions.Multiline)]
    private static partial Regex SubcommandLinePattern();

    /// <summary>
    /// Matches Cobra-style option lines.
    /// Examples:
    ///   -d, --detach                 Run container in background
    ///   -e, --env list               Set environment variables
    ///       --name string            Assign a name
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w),\s*)?(?<long>--[\w-]+)(?:\s+(?<type>\S+))?(?:\s{2,}(?<desc>.*))?$", RegexOptions.Multiline)]
    private static partial Regex CobraOptionPattern();

    /// <summary>
    /// Matches "Flags:" or "Global Flags:" section headers.
    /// </summary>
    [GeneratedRegex(@"(?:Global\s+)?Flags:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex FlagsSectionPattern();

    /// <summary>
    /// Matches "Options:" section header (kubectl style).
    /// </summary>
    [GeneratedRegex(@"^Options:\s*\n", RegexOptions.Multiline)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches kubectl-style option lines.
    /// Examples:
    ///     -A, --all-namespaces=false:
    ///     --chunk-size=500:
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w),\s*)?(?<long>--[\w-]+)(?:=(?<type>\S+))?:\s*(?<desc>.*)?$", RegexOptions.Multiline)]
    private static partial Regex KubectlOptionPattern();

    /// <summary>
    /// Matches "One of: value1|value2" pattern.
    /// </summary>
    [GeneratedRegex(@"[Oo]ne of[:\s]+(?<values>[\w\-|,\s""'`]+?)(?:\s*\(|$|\.|;)", RegexOptions.IgnoreCase)]
    private static partial Regex OneOfPattern();

    /// <summary>
    /// Matches "(value1, value2)" or "(value1|value2)" patterns.
    /// </summary>
    [GeneratedRegex(@"\((?<values>[\w\-]+(?:\s*[|,]\s*[\w\-]+)+)\)")]
    private static partial Regex ParenthesizedValuesPattern();

    /// <summary>
    /// Matches usage line.
    /// </summary>
    [GeneratedRegex(@"Usage:\s*\n?\s*(?<usage>[^\n]+)", RegexOptions.IgnoreCase)]
    private static partial Regex UsageLinePattern();

    /// <summary>
    /// Matches positional arguments in usage: [NAME] or <NAME> or [NAME...].
    /// </summary>
    [GeneratedRegex(@"[\[<](?<name>[A-Z_-]+)(?:\.\.\.)?[\]>]")]
    private static partial Regex PositionalArgPattern();

    /// <summary>
    /// Validates that a command part looks like a real CLI command/subcommand.
    /// Uses structural validation - real Cobra subcommands follow consistent naming conventions.
    /// </summary>
    private static bool IsValidCommandPart(string part)
    {
        if (string.IsNullOrWhiteSpace(part) || part.Length < 2)
        {
            return false;
        }

        // Cobra CLIs use lowercase subcommand names (uppercase = not a command)
        // This filters: HEALTHCHECK, ENTRYPOINT, ENV, ARG, etc.
        if (part.Any(char.IsUpper))
        {
            return false;
        }

        // Valid subcommands only contain letters, digits, and hyphens
        // This filters: --flags, special chars, etc.
        return part.All(c => char.IsLetter(c) || char.IsDigit(c) || c == '-');
    }

    /// <summary>
    /// Checks if help text represents a valid Cobra command (not an error or wrong command).
    /// Validates that the help output is actually for the expected command.
    /// </summary>
    protected static bool IsValidCommandHelp(string helpText, string[] expectedCommandParts)
    {
        if (string.IsNullOrWhiteSpace(helpText))
        {
            return false;
        }

        // Valid Cobra command help always has "Usage:" section
        if (!helpText.Contains("Usage:"))
        {
            return false;
        }

        // Error messages typically contain these patterns
        if (helpText.Contains("unknown command") ||
            helpText.Contains("Error:") ||
            helpText.Contains("unknown flag") ||
            helpText.Contains("invalid argument"))
        {
            return false;
        }

        // Verify the Usage line contains the expected command (not a parent command's help)
        // This catches garbage like "docker ps templates" which returns "docker ps" help
        if (expectedCommandParts.Length > 0)
        {
            var lastPart = expectedCommandParts[^1];
            var usageLineMatch = UsageLinePattern().Match(helpText);
            if (usageLineMatch.Success)
            {
                var usageLine = usageLineMatch.Groups["usage"].Value;
                // The last command part should appear in the usage line
                if (!usageLine.Contains(lastPart, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
