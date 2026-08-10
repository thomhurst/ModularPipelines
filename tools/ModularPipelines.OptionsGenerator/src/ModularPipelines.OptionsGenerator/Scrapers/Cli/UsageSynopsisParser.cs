using ModularPipelines.Attributes;
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

    private static readonly HashSet<string> OptionControlTokens = new(StringComparer.OrdinalIgnoreCase)
    {
        "flag",
        "flags",
        "global option",
        "global options",
        "option",
        "options",
    };

    private static readonly HashSet<string> CommandGroupPlaceholderNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "Command",
        "Commands",
        "Subcommand",
        "Subcommands",
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

        var suppliedSynopses = (additionalSynopses ?? [])
            .Where(synopsis => !string.IsNullOrWhiteSpace(synopsis))
            .Distinct(StringComparer.Ordinal)
            .ToList();
        var suppliedSynopsisSet = suppliedSynopses.ToHashSet(StringComparer.Ordinal);
        var synopses = ExtractSynopses(helpText)
            .Concat(suppliedSynopses)
            .Where(synopsis => !string.IsNullOrWhiteSpace(synopsis))
            .Distinct(StringComparer.Ordinal)
            .ToList();

        var candidates = synopses
            .Select(synopsis => ParseSynopsis(synopsis, commandPath))
            .Where(result => result.CommandMatched)
            .ToList();
        if (candidates.Count == 0)
        {
            return UsageSynopsisParseResult.Empty with
            {
                HasExtractedSynopses = synopses.Count > 0,
                Synopsis = synopses.FirstOrDefault(),
            };
        }

        var rankedCandidates = candidates
            .OrderByDescending(result => result.MatchedCommandPartCount)
            .ThenByDescending(result => result.PositionalArguments.Count)
            .ThenBy(result => result.UnparsedOperandTokens.Count)
            .ToList();
        var selected = rankedCandidates[0];
        var sameCommandCandidates = candidates
            .Where(candidate =>
                candidate.MatchedCommandPartCount == selected.MatchedCommandPartCount)
            .ToList();
        var requirednessCandidates =
            suppliedSynopsisSet.Contains(selected.Synopsis ?? "")
            ? [.. sameCommandCandidates
                .Where(candidate => suppliedSynopsisSet.Contains(candidate.Synopsis ?? ""))]
            : sameCommandCandidates;

        return selected with
        {
            HasExtractedSynopses = true,
            MatchedSynopsisCount = candidates.Count,
            HasAmbiguousMatch = rankedCandidates
                .Skip(1)
                .Any(candidate => HasSameScore(selected, candidate)),
            PositionalArguments = RelaxArgumentsMissingFromAlternatives(
                selected.PositionalArguments,
                requirednessCandidates),
        };
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

    internal static UsageSynopsisParseResult RemoveCommandGroupPlaceholders(
        UsageSynopsisParseResult result)
    {
        var positionalArguments = result.PositionalArguments
            .Where(argument => !CommandGroupPlaceholderNames.Contains(argument.PropertyName))
            .ToList();

        return result with
        {
            HasOperandTokens = positionalArguments.Count > 0 || result.UnparsedOperandTokens.Count > 0,
            PositionalArguments = positionalArguments,
        };
    }

    private static UsageSynopsisParseResult ParseSynopsis(
        string synopsis,
        IReadOnlyList<string> commandPath)
    {
        var tokens = Tokenize(synopsis);
        var commandMatch = FindCommand(tokens, commandPath);
        if (commandMatch is null)
        {
            return UsageSynopsisParseResult.Unmatched(synopsis);
        }

        var phase = tokens
            .Take(commandMatch.EndIndex + 1)
            .Any(IsOptionControlToken)
                ? CommandLinePhase.Passthrough
                : CommandLinePhase.EarlyOperand;
        var operandTokens = tokens.Skip(commandMatch.EndIndex + 1);
        if (tokens[commandMatch.EndIndex].TrimEnd().EndsWith(','))
        {
            operandTokens = SkipCommandAliases(operandTokens);
        }

        var parsedOperands = ParseOperandTokens(operandTokens, phase);
        return new UsageSynopsisParseResult
        {
            Synopsis = synopsis,
            CommandMatched = true,
            MatchedCommandPartCount = commandMatch.PartCount,
            HasOperandTokens = parsedOperands.Arguments.Count > 0
                               || parsedOperands.UnparsedTokens.Count > 0,
            PositionalArguments = CliPositionalArgument.MergeDuplicates(parsedOperands.Arguments),
            UnparsedOperandTokens = parsedOperands.UnparsedTokens,
        };
    }

    private static ParsedOperands ParseOperandTokens(
        IEnumerable<string> operandTokens,
        CommandLinePhase phase)
    {
        var arguments = new List<CliPositionalArgument>();
        var unparsedTokens = new List<string>();
        var prependOptionTerminatorToNextOperand = false;

        foreach (var token in TrimTrailingUsageExplanation(CollapseAlternatives(operandTokens)))
        {
            phase = IsOptionControlToken(token) ? CommandLinePhase.Passthrough : phase;

            if (IsStandaloneOptionTerminator(token))
            {
                prependOptionTerminatorToNextOperand = true;
                continue;
            }

            var groupedBehindOptionTerminator =
                TryUnwrapOptionTerminatedOperand(token, out var unwrappedOperand);
            var operandToken = groupedBehindOptionTerminator ? unwrappedOperand : token;
            if (TryApplyStandaloneRepeat(operandToken, arguments))
            {
                continue;
            }

            if (IsNonOperandSyntax(operandToken))
            {
                continue;
            }

            if (TryParseNestedOperandGroup(
                    operandToken,
                    arguments.Count,
                    phase,
                    out var nestedArguments))
            {
                arguments.AddRange(nestedArguments);
                prependOptionTerminatorToNextOperand = false;
                continue;
            }

            var argument = ParseOperand(operandToken, arguments.Count, phase);
            if (argument is null)
            {
                unparsedTokens.Add(operandToken);
                continue;
            }

            if (groupedBehindOptionTerminator || prependOptionTerminatorToNextOperand)
            {
                argument = argument with { PrependOptionTerminator = true };
            }

            arguments.Add(argument);
            prependOptionTerminatorToNextOperand = false;
        }

        return new ParsedOperands(arguments, unparsedTokens);
    }

    private readonly record struct ParsedOperands(
        IReadOnlyList<CliPositionalArgument> Arguments,
        IReadOnlyList<string> UnparsedTokens);

    private static IEnumerable<string> SkipCommandAliases(IEnumerable<string> operandTokens)
    {
        using var enumerator = operandTokens.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (!enumerator.Current.TrimEnd().EndsWith(','))
            {
                break;
            }
        }

        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }

    private static bool HasSameScore(
        UsageSynopsisParseResult left,
        UsageSynopsisParseResult right) =>
        left.MatchedCommandPartCount == right.MatchedCommandPartCount
        && left.PositionalArguments.Count == right.PositionalArguments.Count
        && left.UnparsedOperandTokens.Count == right.UnparsedOperandTokens.Count;

    private static IReadOnlyList<CliPositionalArgument> RelaxArgumentsMissingFromAlternatives(
        IReadOnlyList<CliPositionalArgument> selectedArguments,
        List<UsageSynopsisParseResult> alternatives)
    {
        if (alternatives.Count <= 1)
        {
            return selectedArguments;
        }

        return selectedArguments
            .Select(argument => alternatives.All(alternative =>
                alternative.PositionalArguments.Any(candidate =>
                    candidate.IsRequired
                    && candidate.PropertyName.Equals(
                        argument.PropertyName,
                        StringComparison.OrdinalIgnoreCase)))
                    ? argument
                    : argument with
                    {
                        CSharpType = $"{argument.CSharpType.TrimEnd('?')}?",
                        IsRequired = false,
                    })
            .ToList();
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
                index = ReadInlineSynopsis(
                    lines,
                    index + 1,
                    inlineSynopsis,
                    synopses);
                continue;
            }

            index = ReadIndentedSynopses(lines, index + 1, synopses);
        }

        return synopses;
    }

    private static int ReadInlineSynopsis(
        string[] lines,
        int startIndex,
        string inlineSynopsis,
        List<string> synopses)
    {
        var parts = new List<string> { inlineSynopsis };
        var index = startIndex;
        for (; index < lines.Length; index++)
        {
            var line = lines[index];
            var trimmed = line.Trim();
            if (!IsSynopsisContinuation(line, trimmed))
            {
                break;
            }

            parts.Add(trimmed);
        }

        synopses.Add(string.Join(' ', parts));
        return index - 1;
    }

    private static bool IsSynopsisContinuation(string line, string trimmed)
    {
        if (string.IsNullOrWhiteSpace(trimmed)
            || line.Length == trimmed.Length)
        {
            return false;
        }

        var firstToken = trimmed.Split(' ', 2)[0];
        if (LooksLikeSectionHeading(trimmed) && !IsWrapped(firstToken))
        {
            return false;
        }

        if (IsPlaceholderToken(firstToken))
        {
            return true;
        }

        return firstToken.StartsWith('-')
               || firstToken is "|" or "...";
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
        if (!remainder.StartsWith(':')
            && synopsis.StartsWith("example ", StringComparison.OrdinalIgnoreCase))
        {
            synopsis = "";
            return false;
        }

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
            var closingDelimiters = new Stack<char>();
            while (index < synopsis.Length)
            {
                var character = synopsis[index];
                if (closingDelimiters.Count == 0 && char.IsWhiteSpace(character))
                {
                    break;
                }

                if (TryGetClosingDelimiter(character, out var closingDelimiter))
                {
                    closingDelimiters.Push(closingDelimiter);
                }
                else if (closingDelimiters.TryPeek(out var expectedDelimiter)
                         && character == expectedDelimiter)
                {
                    closingDelimiters.Pop();
                }

                index++;
            }

            tokens.Add(synopsis[start..index]);
        }

        return tokens;
    }

    private static CommandMatch? FindCommand(
        IReadOnlyList<string> tokens,
        IReadOnlyList<string> commandPath)
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
                    return new CommandMatch(
                        tokenIndex,
                        commandPath.Count - pathStart);
                }
            }
        }

        return null;
    }

    private static CliPositionalArgument? ParseOperand(
        string token,
        int positionIndex,
        CommandLinePhase phase)
    {
        var trimmed = token.Trim().TrimEnd(',', ';');
        var isRequired = !trimmed.StartsWith('[')
                         || HasRequiredSuffixOutsideOptionalPrefix(trimmed);
        var content = TrimWrapper(trimmed).Trim();
        var canonicalName = SelectCanonicalAlternative(content);
        if (TrySelectRequiredCompoundPlaceholder(trimmed, out var requiredPlaceholder))
        {
            isRequired = true;
            canonicalName = requiredPlaceholder;
        }

        var isVariadic = trimmed.EndsWith("...", StringComparison.Ordinal)
                         || trimmed.EndsWith('…')
                         || content.EndsWith("...", StringComparison.Ordinal)
                         || content.EndsWith('…')
                         || canonicalName.EndsWith("...", StringComparison.Ordinal)
                         || canonicalName.EndsWith('…')
                         || canonicalName.EndsWith(" ...", StringComparison.Ordinal)
                         || IsForwardedOptionTail(trimmed, content);
        canonicalName = canonicalName.TrimEnd('.', '…').Trim();

        if (canonicalName.StartsWith('-'))
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
            CSharpType = GetCSharpType(isRequired, isVariadic),
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag: false),
            IsRequired = isRequired,
            IsVariadic = isVariadic,
            PositionIndex = positionIndex,
            Phase = phase,
            Description = $"The {canonicalName} operand.",
        };
    }

    private static bool IsForwardedOptionTail(string token, string content) =>
        token.StartsWith('[')
        && content.Contains(' ')
        && content.EndsWith(" OPTIONS", StringComparison.OrdinalIgnoreCase);

    private static bool TryParseNestedOperandGroup(
        string token,
        int positionIndex,
        CommandLinePhase phase,
        out IReadOnlyList<CliPositionalArgument> arguments)
    {
        arguments = [];
        if (!IsWrapped(token))
        {
            return false;
        }

        var content = TrimWrapper(token).Trim();
        if (!content.Contains('[') || content.Contains('|'))
        {
            return false;
        }

        var nestedTokens = Tokenize(content);
        if (nestedTokens.Count <= 1)
        {
            return false;
        }

        var parsedArguments = new List<CliPositionalArgument>();
        string? associatedOptionSwitch = null;
        foreach (var nestedToken in nestedTokens)
        {
            var isOptionSwitch = TryGetOptionSwitch(nestedToken, out var optionSwitch);
            if (isOptionSwitch)
            {
                associatedOptionSwitch = optionSwitch;
            }

            if (IsNonOperandSyntax(nestedToken))
            {
                if (!isOptionSwitch)
                {
                    associatedOptionSwitch = null;
                }

                continue;
            }

            var argument = ParseOperand(
                nestedToken,
                positionIndex + parsedArguments.Count,
                phase);
            if (argument is null)
            {
                return false;
            }

            parsedArguments.Add(argument with
            {
                CSharpType = GetCSharpType(isRequired: false, argument.IsVariadic),
                IsRequired = false,
                AssociatedOptionSwitch = associatedOptionSwitch,
            });
            associatedOptionSwitch = null;
        }

        arguments = parsedArguments;
        return true;
    }

    private static bool HasRequiredSuffixOutsideOptionalPrefix(string token)
    {
        if (!token.StartsWith('['))
        {
            return false;
        }

        var closingBracketIndex = token.IndexOf(']');
        return closingBracketIndex >= 0
               && token[(closingBracketIndex + 1)..].Any(char.IsLetterOrDigit);
    }

    private static bool TrySelectRequiredCompoundPlaceholder(
        string token,
        out string requiredPlaceholder)
    {
        var optionalDepth = 0;
        var requiredPlaceholders = new List<string>();
        var hasOptionalPlaceholder = false;

        for (var index = 0; index < token.Length; index++)
        {
            if (token[index] == '[')
            {
                optionalDepth++;
                continue;
            }

            if (token[index] == ']')
            {
                optionalDepth = Math.Max(0, optionalDepth - 1);
                continue;
            }

            if (token[index] != '<')
            {
                continue;
            }

            var end = token.IndexOf('>', index + 1);
            if (end < 0)
            {
                break;
            }

            var placeholder = token[(index + 1)..end].Trim();
            if (!string.IsNullOrWhiteSpace(placeholder))
            {
                if (optionalDepth == 0)
                {
                    requiredPlaceholders.Add(placeholder);
                }
                else
                {
                    hasOptionalPlaceholder = true;
                }
            }

            index = end;
        }

        requiredPlaceholder = requiredPlaceholders.Count > 0
            ? requiredPlaceholders[^1]
            : string.Empty;
        return hasOptionalPlaceholder && requiredPlaceholders.Count > 0;
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

    private static IReadOnlyList<string> TrimTrailingUsageExplanation(IEnumerable<string> sourceTokens)
    {
        var tokens = sourceTokens.ToList();
        var boundaryIndex = tokens.FindIndex(static token =>
            token.Length > 1
            && token[^1] == '.'
            && token[^2] is ']' or '>' or '}' or ')');
        if (boundaryIndex < 0)
        {
            return tokens;
        }

        tokens[boundaryIndex] = tokens[boundaryIndex][..^1];
        return tokens.Take(boundaryIndex + 1).ToList();
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
        if (token is not ("..." or "…" or "[...]" or "[…]") || arguments.Count == 0)
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

    private static bool TryUnwrapOptionTerminatedOperand(
        string token,
        out string operand)
    {
        var content = TrimWrapper(token).Trim();
        if (!content.StartsWith("--", StringComparison.Ordinal)
            || content.Length == 2
            || !char.IsWhiteSpace(content[2]))
        {
            operand = "";
            return false;
        }

        operand = content[2..].Trim();
        return !string.IsNullOrWhiteSpace(operand);
    }

    private static bool IsStandaloneOptionTerminator(string token) =>
        TrimWrapper(token).Trim() == "--";

    private static bool IsControlToken(string token)
    {
        var content = TrimControlWrappers(token);
        return string.IsNullOrWhiteSpace(content)
               || content.StartsWith('-')
               || ControlTokens.Contains(content)
               || content.All(character => !char.IsLetterOrDigit(character));
    }

    private static bool IsNonOperandSyntax(string token)
    {
        var content = TrimControlWrappers(token);
        return string.IsNullOrWhiteSpace(content)
               || content.StartsWith('-')
               || OptionControlTokens.Contains(content)
               || content.All(character => !char.IsLetterOrDigit(character));
    }

    private static bool IsOptionControlToken(string token)
    {
        var content = TrimControlWrappers(token);
        return content.StartsWith('-')
               || OptionControlTokens.Contains(content);
    }

    private static bool TryGetOptionSwitch(string token, out string optionSwitch)
    {
        var content = TrimControlWrappers(token);
        var endIndex = content.IndexOfAny([' ', '\t', '=']);
        optionSwitch = content.TrimEnd(',', ':');
        if (optionSwitch.Length > 1
            && endIndex < 0
            && optionSwitch.StartsWith('-')
            && optionSwitch != "--")
        {
            return true;
        }

        optionSwitch = "";
        return false;
    }

    private static string TrimControlWrappers(string token)
    {
        var content = token.Trim();
        while (IsWrapped(content))
        {
            content = TrimWrapper(content).Trim();
        }

        return content;
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

    private sealed record CommandMatch(int EndIndex, int PartCount);
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

    public bool HasExtractedSynopses { get; init; }

    public int MatchedCommandPartCount { get; init; }

    public int MatchedSynopsisCount { get; init; }

    public bool HasAmbiguousMatch { get; init; }

    public bool HasOperandTokens { get; init; }

    public IReadOnlyList<CliPositionalArgument> PositionalArguments { get; init; } = [];

    public IReadOnlyList<string> UnparsedOperandTokens { get; init; } = [];
}
