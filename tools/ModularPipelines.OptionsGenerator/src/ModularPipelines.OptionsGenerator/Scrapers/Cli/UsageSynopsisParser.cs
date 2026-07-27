using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// Parses positional operands from CLI usage and synopsis text.
/// </summary>
public static class UsageSynopsisParser
{
    private static readonly HashSet<string> ControlTokens = new(StringComparer.OrdinalIgnoreCase)
    {
        "arg",
        "args",
        "argument",
        "arguments",
        "command",
        "commands",
        "flag",
        "flags",
        "global option",
        "global options",
        "help",
        "option",
        "options",
        "subcommand",
        "subcommands",
    };

    /// <summary>
    /// Parses the best matching synopsis for a command.
    /// </summary>
    public static UsageSynopsisParseResult Parse(
        string helpText,
        IReadOnlyList<string> commandPath,
        IEnumerable<string>? additionalSynopses = null)
    {
        ArgumentNullException.ThrowIfNull(helpText);
        ArgumentNullException.ThrowIfNull(commandPath);

        var synopses = ExtractSynopses(helpText)
            .Concat(additionalSynopses ?? [])
            .Where(synopsis => !string.IsNullOrWhiteSpace(synopsis))
            .Distinct(StringComparer.Ordinal)
            .ToList();

        return synopses
            .Select(synopsis => ParseSynopsis(synopsis, commandPath))
            .Where(result => result.CommandMatched)
            .OrderByDescending(result => result.PositionalArguments.Count)
            .ThenBy(result => result.UnparsedOperandTokens.Count)
            .FirstOrDefault()
            ?? UsageSynopsisParseResult.Empty;
    }

    /// <summary>
    /// Returns true when a discovered token is placeholder syntax, not a command name.
    /// </summary>
    public static bool IsPlaceholderToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        var trimmed = token.Trim();
        return IsWrapped(trimmed)
               || trimmed.Contains('|')
               || trimmed.EndsWith("...", StringComparison.Ordinal)
               || (trimmed.Length > 1 && trimmed.All(character =>
                   !char.IsLetter(character) || char.IsUpper(character)));
    }

    private static UsageSynopsisParseResult ParseSynopsis(
        string synopsis,
        IReadOnlyList<string> commandPath)
    {
        var tokens = Tokenize(synopsis);
        var commandEnd = FindCommandEnd(tokens, commandPath);
        if (commandEnd < 0)
        {
            return UsageSynopsisParseResult.Unmatched(synopsis);
        }

        var arguments = new List<CliPositionalArgument>();
        var unparsedTokens = new List<string>();
        foreach (var token in CollapseAlternatives(tokens.Skip(commandEnd + 1)))
        {
            if (TryApplyStandaloneRepeat(token, arguments) || IsControlToken(token))
            {
                continue;
            }

            var argument = ParseOperand(token, arguments.Count);
            if (argument is null)
            {
                unparsedTokens.Add(token);
                continue;
            }

            arguments.Add(argument);
        }

        return new UsageSynopsisParseResult
        {
            Synopsis = synopsis,
            CommandMatched = true,
            HasOperandTokens = arguments.Count > 0 || unparsedTokens.Count > 0,
            PositionalArguments = CliPositionalArgument.MergeDuplicates(arguments),
            UnparsedOperandTokens = unparsedTokens,
        };
    }

    private static IReadOnlyList<string> ExtractSynopses(string helpText)
    {
        var lines = helpText
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n');
        var synopses = new List<string>();

        for (var index = 0; index < lines.Length; index++)
        {
            var trimmed = lines[index].Trim();
            if (!TryReadUsageHeading(trimmed, out var inlineSynopsis))
            {
                continue;
            }

            if (!string.IsNullOrWhiteSpace(inlineSynopsis))
            {
                synopses.Add(inlineSynopsis);
                continue;
            }

            index = ReadIndentedSynopses(lines, index + 1, synopses);
        }

        return synopses;
    }

    private static bool TryReadUsageHeading(string line, out string synopsis)
    {
        synopsis = "";
        if (!line.StartsWith("usage", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var remainder = line["usage".Length..];
        if (remainder.Length > 0 && remainder[0] != ':' && !char.IsWhiteSpace(remainder[0]))
        {
            return false;
        }

        synopsis = remainder.TrimStart(':', ' ', '\t');
        return true;
    }

    private static int ReadIndentedSynopses(string[] lines, int startIndex, List<string> synopses)
    {
        var index = startIndex;
        for (; index < lines.Length; index++)
        {
            var line = lines[index];
            var trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                break;
            }

            if (LooksLikeSectionHeading(trimmed))
            {
                break;
            }

            synopses.Add(trimmed);
        }

        return index;
    }

    private static bool LooksLikeSectionHeading(string line) =>
        line.EndsWith(':')
        || (line.Length < 50
            && line.Any(char.IsLetter)
            && line.Where(char.IsLetter).All(char.IsUpper));

    private static IReadOnlyList<string> Tokenize(string synopsis)
    {
        var tokens = new List<string>();
        var index = 0;
        while (index < synopsis.Length)
        {
            while (index < synopsis.Length && char.IsWhiteSpace(synopsis[index]))
            {
                index++;
            }

            if (index >= synopsis.Length)
            {
                break;
            }

            var start = index;
            if (TryGetClosingDelimiter(synopsis[index], out var closingDelimiter))
            {
                index++;
                while (index < synopsis.Length && synopsis[index] != closingDelimiter)
                {
                    index++;
                }

                if (index < synopsis.Length)
                {
                    index++;
                }

                while (index < synopsis.Length && !char.IsWhiteSpace(synopsis[index]))
                {
                    index++;
                }
            }
            else
            {
                while (index < synopsis.Length && !char.IsWhiteSpace(synopsis[index]))
                {
                    index++;
                }
            }

            tokens.Add(synopsis[start..index]);
        }

        return tokens;
    }

    private static int FindCommandEnd(IReadOnlyList<string> tokens, IReadOnlyList<string> commandPath)
    {
        for (var pathStart = 0; pathStart < commandPath.Count; pathStart++)
        {
            var pathIndex = pathStart;
            for (var tokenIndex = 0; tokenIndex < tokens.Count; tokenIndex++)
            {
                if (IsControlToken(tokens[tokenIndex]))
                {
                    continue;
                }

                if (!NormalizeLiteral(tokens[tokenIndex]).Equals(
                        commandPath[pathIndex],
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                pathIndex++;
                if (pathIndex == commandPath.Count)
                {
                    return tokenIndex;
                }
            }
        }

        return -1;
    }

    private static CliPositionalArgument? ParseOperand(string token, int positionIndex)
    {
        var trimmed = token.Trim().TrimEnd(',', ';');
        var isRequired = !trimmed.StartsWith('[');
        var content = TrimWrapper(trimmed).Trim();
        var canonicalName = SelectCanonicalAlternative(content);
        var isVariadic = canonicalName.EndsWith("...", StringComparison.Ordinal)
                         || canonicalName.EndsWith("…", StringComparison.Ordinal)
                         || canonicalName.EndsWith(" ...", StringComparison.Ordinal);
        canonicalName = canonicalName.TrimEnd('.', '…').Trim();

        if (canonicalName.StartsWith('-') || ControlTokens.Contains(canonicalName))
        {
            return null;
        }

        var propertyName = NormalizeOperandName(canonicalName);
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return null;
        }

        return new CliPositionalArgument
        {
            PropertyName = propertyName,
            PlaceholderName = content,
            CSharpType = GetCSharpType(isRequired, isVariadic),
            IsRequired = isRequired,
            IsVariadic = isVariadic,
            PositionIndex = positionIndex,
            Description = $"The {canonicalName} operand.",
        };
    }

    private static string SelectCanonicalAlternative(string content)
    {
        var pipeIndex = content.IndexOf('|');
        if (pipeIndex >= 0)
        {
            return content[..pipeIndex].Trim();
        }

        return content
            .Replace("/", " Or ", StringComparison.Ordinal)
            .Replace("\\", " Or ", StringComparison.Ordinal);
    }

    private static IReadOnlyList<string> CollapseAlternatives(IEnumerable<string> sourceTokens)
    {
        var tokens = sourceTokens.ToList();
        var collapsed = new List<string>();
        for (var index = 0; index < tokens.Count; index++)
        {
            var token = tokens[index];
            while (index + 2 < tokens.Count && tokens[index + 1] == "|")
            {
                token = $"{token}|{tokens[index + 2]}";
                index += 2;
            }

            collapsed.Add(token);
        }

        return collapsed;
    }

    private static string? NormalizeOperandName(string content)
    {
        var cleaned = new string(content
            .Select(character => char.IsLetterOrDigit(character) ? character : ' ')
            .ToArray());
        var words = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length == 0
            ? null
            : string.Concat(words.Select(GeneratorUtils.ToPascalCase));
    }

    private static bool TryApplyStandaloneRepeat(
        string token,
        List<CliPositionalArgument> arguments)
    {
        if (token is not ("..." or "…") || arguments.Count == 0)
        {
            return false;
        }

        arguments[^1] = arguments[^1] with
        {
            CSharpType = GetCSharpType(arguments[^1].IsRequired, isVariadic: true),
            IsVariadic = true,
        };
        return true;
    }

    private static bool IsControlToken(string token)
    {
        var content = TrimWrapper(token).Trim();
        return string.IsNullOrWhiteSpace(content)
               || content.StartsWith('-')
               || ControlTokens.Contains(content)
               || content.All(character => !char.IsLetterOrDigit(character));
    }

    private static string NormalizeLiteral(string token) =>
        TrimWrapper(token).Trim().TrimEnd(',', ':');

    private static string TrimWrapper(string token)
    {
        if (!IsWrapped(token))
        {
            return token;
        }

        return token[1..^1];
    }

    private static bool IsWrapped(string token) =>
        token.Length >= 2
        && TryGetClosingDelimiter(token[0], out var closingDelimiter)
        && token[^1] == closingDelimiter;

    private static bool TryGetClosingDelimiter(char openingDelimiter, out char closingDelimiter)
    {
        closingDelimiter = openingDelimiter switch
        {
            '[' => ']',
            '<' => '>',
            '{' => '}',
            '(' => ')',
            _ => '\0',
        };
        return closingDelimiter != '\0';
    }

    private static string GetCSharpType(bool isRequired, bool isVariadic)
    {
        var type = isVariadic ? "IEnumerable<string>" : "string";
        return isRequired ? type : $"{type}?";
    }
}

/// <summary>
/// Result of parsing a CLI usage synopsis.
/// </summary>
public sealed record UsageSynopsisParseResult
{
    internal static UsageSynopsisParseResult Empty { get; } = new();

    internal static UsageSynopsisParseResult Unmatched(string synopsis) => new()
    {
        Synopsis = synopsis,
    };

    public string? Synopsis { get; init; }

    public bool CommandMatched { get; init; }

    public bool HasOperandTokens { get; init; }

    public IReadOnlyList<CliPositionalArgument> PositionalArguments { get; init; } = [];

    public IReadOnlyList<string> UnparsedOperandTokens { get; init; } = [];
}
