using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// Parses positional operands from CLI usage and synopsis text.
/// </summary>
public static class UsageSynopsisParser
{
    private static readonly string[] DefaultUsageHeadings = ["usage"];

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
        IEnumerable<string>? additionalSynopses = null,
        IReadOnlyList<string>? acceptedHeadings = null)
    {
        ArgumentNullException.ThrowIfNull(helpText);
        ArgumentNullException.ThrowIfNull(commandPath);

        var suppliedSynopses = (additionalSynopses ?? [])
            .Where(synopsis => !string.IsNullOrWhiteSpace(synopsis))
            .Distinct(StringComparer.Ordinal)
            .ToList();
        var suppliedSynopsisSet = suppliedSynopses.ToHashSet(StringComparer.Ordinal);
        var synopses = ExtractSynopses(helpText, acceptedHeadings ?? DefaultUsageHeadings)
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
            RequiredAlternativeGroups =
            [
                .. selected.RequiredAlternativeGroups,
                .. GetCrossSynopsisRequiredAlternativeGroups(
                    requirednessCandidates,
                    selected.PositionalArguments),
            ],
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
            .Where(argument => !IsCommandGroupPlaceholder(argument))
            .ToList();

        return result with
        {
            HasOperandTokens = positionalArguments.Count > 0 || result.UnparsedOperandTokens.Count > 0,
            PositionalArguments = positionalArguments,
        };
    }

    internal static bool IsCommandGroupPlaceholder(CliPositionalArgument argument) =>
        CommandGroupPlaceholderNames.Contains(argument.PropertyName);

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

        var phase = CollapseAlternatives(tokens
                .Take(commandMatch.EndIndex + 1))
            .Any(IsPhaseControlToken)
                ? CommandLinePhase.Passthrough
                : CommandLinePhase.EarlyOperand;
        var operandTokens = tokens.Skip(commandMatch.EndIndex + 1);
        if (tokens[commandMatch.EndIndex].TrimEnd().EndsWith(','))
        {
            operandTokens = SkipCommandAliases(operandTokens);
        }

        var materializedOperandTokens = operandTokens.ToArray();
        var parsedOperands = ParseOperandTokens(materializedOperandTokens, phase);
        return new UsageSynopsisParseResult
        {
            Synopsis = synopsis,
            CommandMatched = true,
            MatchedCommandPartCount = commandMatch.PartCount,
            HasOperandTokens = parsedOperands.Arguments.Count > 0
                               || parsedOperands.UnparsedTokens.Count > 0,
            PositionalArguments = CliPositionalArgument.MergeDuplicates(parsedOperands.Arguments),
            UnparsedOperandTokens = parsedOperands.UnparsedTokens,
            SupportsRequiredAlternativeInference = SupportsRequiredAlternativeInference(
                materializedOperandTokens),
            RequiredAlternativeGroups = ParseInlineRequiredAlternativeGroups(
                materializedOperandTokens,
                phase),
        };
    }

    private static IReadOnlyList<UsageRequiredAlternativeGroup> ParseInlineRequiredAlternativeGroups(
        IEnumerable<string> operandTokens,
        CommandLinePhase phase)
    {
        var groups = new List<UsageRequiredAlternativeGroup>();
        foreach (var token in operandTokens)
        {
            var normalizedToken = TrimTrailingOperandPunctuation(token)
                .TrimEnd('.', '…')
                .Trim();
            if (normalizedToken.StartsWith('[') || !IsWrapped(normalizedToken))
            {
                continue;
            }

            var alternatives = SplitTopLevelAlternatives(TrimWrapper(normalizedToken));
            if (alternatives.Count <= 1)
            {
                continue;
            }

            var alternativeMembers = alternatives
                .Select(alternative => GetRequiredAlternativeMembers(
                    ParseOperandTokens(Tokenize(alternative), phase)))
                .ToArray();
            if (alternativeMembers.Any(static members => members.Count != 1))
            {
                continue;
            }

            var members = DistinctAlternativeMembers(alternativeMembers.SelectMany(static members => members));
            if (members.Count > 1
                && members.Any(static member => member.OptionSwitch is not null))
            {
                groups.Add(new UsageRequiredAlternativeGroup { Members = members });
            }
        }

        return groups;
    }

    private static IReadOnlyList<UsageRequiredAlternativeGroup> GetCrossSynopsisRequiredAlternativeGroups(
        IReadOnlyList<UsageSynopsisParseResult> candidates,
        IReadOnlyList<CliPositionalArgument> selectedArguments)
    {
        if (candidates.Count <= 1
            || candidates.Any(static candidate => !candidate.SupportsRequiredAlternativeInference))
        {
            return [];
        }

        var candidateMembers = candidates
            .Select(candidate => GetRequiredAlternativeMembers(new ParsedOperands(
                candidate.PositionalArguments,
                candidate.UnparsedOperandTokens)))
            .ToArray();
        if (candidateMembers.Any(static members => members.Count == 0))
        {
            return [];
        }

        var commonKeys = candidateMembers[0]
            .Select(GetAlternativeMemberKey)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var members in candidateMembers.Skip(1))
        {
            commonKeys.IntersectWith(members.Select(GetAlternativeMemberKey));
        }

        var branchMembers = candidateMembers
            .Select(members => members
                .Where(member => !commonKeys.Contains(GetAlternativeMemberKey(member)))
                .ToArray())
            .ToArray();
        if (branchMembers.Any(static members => members.Length != 1))
        {
            return [];
        }

        var alternatives = DistinctAlternativeMembers(branchMembers.SelectMany(static members => members));
        var selectedPropertyNames = selectedArguments
            .Select(static argument => argument.PropertyName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (alternatives.Any(member => member.PositionalPropertyName is { } propertyName
                                       && !selectedPropertyNames.Contains(propertyName)))
        {
            return [];
        }

        return alternatives.Count > 1
            ? [new UsageRequiredAlternativeGroup { Members = alternatives }]
            : [];
    }

    private static bool SupportsRequiredAlternativeInference(IEnumerable<string> operandTokens)
    {
        foreach (var token in CollapseAlternatives(operandTokens))
        {
            if (IsNonOperandSyntax(token))
            {
                continue;
            }

            return token.StartsWith('-') || IsPlaceholderToken(token);
        }

        return false;
    }

    private static IReadOnlyList<UsageRequiredAlternativeMember> GetRequiredAlternativeMembers(
        ParsedOperands operands) =>
        DistinctAlternativeMembers(operands.Arguments
            .Where(static argument => argument.IsRequired)
            .Select(static argument => argument.AssociatedOptionSwitch is { } optionSwitch
                ? new UsageRequiredAlternativeMember { OptionSwitch = optionSwitch }
                : new UsageRequiredAlternativeMember { PositionalPropertyName = argument.PropertyName }));

    private static IReadOnlyList<UsageRequiredAlternativeMember> DistinctAlternativeMembers(
        IEnumerable<UsageRequiredAlternativeMember> members) =>
        members
            .DistinctBy(GetAlternativeMemberKey, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private static string GetAlternativeMemberKey(UsageRequiredAlternativeMember member) =>
        member.OptionSwitch is { } optionSwitch
            ? $"option:{optionSwitch}"
            : $"operand:{member.PositionalPropertyName}";

    private static IReadOnlyList<string> SplitTopLevelAlternatives(string content)
    {
        var alternatives = new List<string>();
        var closingDelimiters = new Stack<char>();
        var start = 0;
        for (var index = 0; index < content.Length; index++)
        {
            var character = content[index];
            if (TryGetClosingDelimiter(character, out var closingDelimiter))
            {
                closingDelimiters.Push(closingDelimiter);
            }
            else if (closingDelimiters.TryPeek(out var expectedDelimiter)
                     && character == expectedDelimiter)
            {
                closingDelimiters.Pop();
            }
            else if (character == '|' && closingDelimiters.Count == 0)
            {
                alternatives.Add(content[start..index].Trim());
                start = index + 1;
            }
        }

        alternatives.Add(content[start..].Trim());
        return alternatives.Where(static alternative => alternative.Length > 0).ToArray();
    }

    private static ParsedOperands ParseOperandTokens(
        IEnumerable<string> operandTokens,
        CommandLinePhase phase)
    {
        var arguments = new List<CliPositionalArgument>();
        var unparsedTokens = new List<string>();
        var prependOptionTerminatorToNextOperand = false;
        string? associatedOptionSwitch = null;

        foreach (var token in TrimTrailingUsageExplanation(CollapseAlternatives(operandTokens)))
        {
            var operandPhase = TransitionPhase(token, ref phase);

            if (IsStandaloneOptionTerminator(token))
            {
                prependOptionTerminatorToNextOperand = true;
                associatedOptionSwitch = null;
                continue;
            }

            var groupedBehindOptionTerminator =
                TryUnwrapOptionTerminatedOperand(token, out var unwrappedOperand);
            var operandToken = groupedBehindOptionTerminator ? unwrappedOperand : token;
            ClearAssociatedOptionSwitch(
                groupedBehindOptionTerminator,
                ref associatedOptionSwitch);

            if (TryGetOptionSwitch(operandToken, out var optionSwitch))
            {
                associatedOptionSwitch = optionSwitch;
                continue;
            }

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
                    operandPhase,
                    out var nestedArguments))
            {
                nestedArguments = PreserveOptionTerminatorOnNestedGroup(
                    nestedArguments,
                    groupedBehindOptionTerminator || prependOptionTerminatorToNextOperand);
                arguments.AddRange(nestedArguments);
                prependOptionTerminatorToNextOperand = false;
                AdvancePastOptionTerminatedOperand(groupedBehindOptionTerminator, ref phase);

                continue;
            }

            var argument = ParseOperand(operandToken, arguments.Count, operandPhase);
            if (argument is null)
            {
                unparsedTokens.Add(operandToken);
                associatedOptionSwitch = null;
                continue;
            }

            if (associatedOptionSwitch is not null)
            {
                argument = argument with { AssociatedOptionSwitch = associatedOptionSwitch };
            }

            if (groupedBehindOptionTerminator || prependOptionTerminatorToNextOperand)
            {
                argument = argument with { PrependOptionTerminator = true };
            }

            arguments.Add(argument);
            prependOptionTerminatorToNextOperand = false;
            associatedOptionSwitch = null;
            AdvancePastOptionTerminatedOperand(groupedBehindOptionTerminator, ref phase);
        }

        return new ParsedOperands(arguments, unparsedTokens);
    }

    private static IReadOnlyList<CliPositionalArgument> PreserveOptionTerminatorOnNestedGroup(
        IReadOnlyList<CliPositionalArgument> arguments,
        bool prependOptionTerminator)
    {
        if (!prependOptionTerminator || arguments.Count == 0)
        {
            return arguments;
        }

        return arguments
            .Select(static argument => argument with { PrependOptionTerminator = true })
            .ToArray();
    }

    private static void ClearAssociatedOptionSwitch(
        bool groupedBehindOptionTerminator,
        ref string? associatedOptionSwitch)
    {
        if (groupedBehindOptionTerminator)
        {
            associatedOptionSwitch = null;
        }
    }

    private static void AdvancePastOptionTerminatedOperand(
        bool groupedBehindOptionTerminator,
        ref CommandLinePhase phase)
    {
        if (groupedBehindOptionTerminator)
        {
            phase = CommandLinePhase.LateOperand;
        }
    }

    private static CommandLinePhase TransitionPhase(
        string token,
        ref CommandLinePhase phase)
    {
        var operandPhase = phase;
        if (IsOptionControlToken(token))
        {
            phase = CommandLinePhase.Passthrough;
            return phase;
        }

        if (IsPhaseControlToken(token))
        {
            phase = CommandLinePhase.Passthrough;
        }

        return operandPhase;
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

    private static IReadOnlyList<string> ExtractSynopses(
        string helpText,
        IReadOnlyList<string> acceptedHeadings)
    {
        var lines = helpText
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n');
        var synopses = new List<string>();

        for (var index = 0; index < lines.Length; index++)
        {
            var trimmed = lines[index].Trim();
            if (!TryReadSynopsisHeading(trimmed, acceptedHeadings, out var inlineSynopsis))
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
        var commandToken = inlineSynopsis.Split(' ', 2)[0];
        var index = startIndex;
        for (; index < lines.Length; index++)
        {
            var line = lines[index];
            var trimmed = line.Trim();
            if (line.Length != trimmed.Length
                && trimmed.Split(' ', 2)[0].Equals(
                    commandToken,
                    StringComparison.OrdinalIgnoreCase))
            {
                synopses.Add(string.Join(' ', parts));
                parts.Clear();
                parts.Add(trimmed);
                continue;
            }

            if (!IsSynopsisContinuationCore(trimmed))
            {
                break;
            }

            parts.Add(trimmed);
        }

        synopses.Add(string.Join(' ', parts));
        return index - 1;
    }

    private static bool IsSynopsisContinuationCore(string trimmed)
    {
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            return false;
        }

        var firstToken = TrimTrailingOperandPunctuation(trimmed.Split(' ', 2)[0]);
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

    internal static bool IsSynopsisContinuation(string line) =>
        IsSynopsisContinuationCore(line.Trim());

    private static bool TryReadSynopsisHeading(
        string line,
        IReadOnlyList<string> acceptedHeadings,
        out string synopsis)
    {
        synopsis = "";
        var normalizedLine = line.Trim(' ', '\t', '━', '─');
        var heading = acceptedHeadings.FirstOrDefault(candidate =>
            normalizedLine.Equals(candidate, StringComparison.OrdinalIgnoreCase)
            || normalizedLine.StartsWith(candidate, StringComparison.OrdinalIgnoreCase));
        if (heading is null)
        {
            return false;
        }

        var remainder = normalizedLine[heading.Length..];
        if (remainder.Length == 0)
        {
            return true;
        }

        if (remainder[0] != ':' && !char.IsWhiteSpace(remainder[0]))
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
        var initialSynopsisCount = synopses.Count;
        var index = startIndex;
        for (; index < lines.Length; index++)
        {
            var line = lines[index];
            var trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                if (synopses.Count == initialSynopsisCount)
                {
                    continue;
                }

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

                if (!TokenMatchesCommandPart(tokens[tokenIndex], commandPath[pathIndex]))
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
        var trimmed = TrimTrailingOperandPunctuation(token);
        var isRequired = !trimmed.StartsWith('[')
                         || HasRequiredSuffixOutsideOptionalPrefix(trimmed);
        var content = TrimWrapper(trimmed).Trim();
        var canonicalName = SelectCanonicalAlternative(content);
        if (HasMixedOptionOperandAlternatives(content))
        {
            isRequired = false;
        }
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
        var documentationName = GetOperandDocumentationName(trimmed, canonicalName);

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
            Description = $"The {documentationName} operand.",
        };
    }

    private static string GetOperandDocumentationName(string token, string canonicalName)
    {
        var placeholder = $"<{canonicalName}>";
        if (token.Contains(placeholder, StringComparison.Ordinal))
        {
            return placeholder;
        }

        var variadicPlaceholder = $"<{canonicalName}...>";
        return token.Contains(variadicPlaceholder, StringComparison.Ordinal)
            ? variadicPlaceholder
            : canonicalName;
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
        var normalizedToken = TrimTrailingOperandPunctuation(token);
        if (!IsWrapped(normalizedToken))
        {
            return false;
        }

        var content = TrimWrapper(normalizedToken).Trim();
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

    private static string TrimTrailingOperandPunctuation(string token) =>
        token.Trim().TrimEnd(',', ';', ':');

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
        var alternatives = content.Split(
            '|',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (alternatives.Length > 1)
        {
            return alternatives.FirstOrDefault(static alternative =>
                       TrimControlWrappers(alternative) != "-"
                       && IsOperandAlternative(alternative))
                   ?? alternatives.FirstOrDefault(IsOperandAlternative)
                   ?? alternatives[0];
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
        var punctuationBoundaryIndex = tokens.FindIndex(static token =>
            token.Length > 1
            && token[^1] == '.'
            && token[^2] is ']' or '>' or '}' or ')');
        var optionDescriptionBoundaryIndex = tokens.FindIndex(static token =>
            token.StartsWith('-')
            && token.EndsWith(','));
        var boundaryIndex = new[] { punctuationBoundaryIndex, optionDescriptionBoundaryIndex }
            .Where(index => index >= 0)
            .DefaultIfEmpty(-1)
            .Min();
        if (boundaryIndex < 0)
        {
            return tokens;
        }

        if (boundaryIndex == punctuationBoundaryIndex)
        {
            tokens[boundaryIndex] = tokens[boundaryIndex][..^1];
            boundaryIndex++;
        }

        return tokens.Take(boundaryIndex).ToList();
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
        if (IsParentheticalExplanation(token))
        {
            return true;
        }

        var content = TrimControlWrappers(token);
        if (HasMixedOptionOperandAlternatives(content)
            || HasLoneDashOperandAlternatives(content))
        {
            return false;
        }

        if (HasOnlyOptionControlAlternatives(content))
        {
            return true;
        }

        return string.IsNullOrWhiteSpace(content)
               || content.StartsWith('-')
               || IsOptionControlLabel(content)
               || content.All(character => !char.IsLetterOrDigit(character));
    }

    private static bool IsParentheticalExplanation(string token)
    {
        var trimmed = token.Trim();
        return trimmed.StartsWith("(in ", StringComparison.OrdinalIgnoreCase)
               && trimmed.EndsWith(')');
    }

    private static bool IsOptionControlLabel(string content) =>
        OptionControlTokens.Contains(content)
        || content.EndsWith(" flags", StringComparison.OrdinalIgnoreCase);

    private static bool IsOptionControlToken(string token)
    {
        var content = TrimControlWrappers(token);
        return !HasMixedOptionOperandAlternatives(content)
               && !HasLoneDashOperandAlternatives(content)
               && (IsOptionAlternative(content)
                   || OptionControlTokens.Contains(content));
    }

    private static bool IsPhaseControlToken(string token)
    {
        var content = TrimControlWrappers(token);
        return IsOptionControlToken(token)
               || GetAlternatives(content).Any(IsOptionAlternative);
    }

    private static bool TryGetOptionSwitch(string token, out string optionSwitch)
    {
        var content = TrimControlWrappers(token);
        if (HasMixedOptionOperandAlternatives(content)
            || HasLoneDashOperandAlternatives(content))
        {
            optionSwitch = "";
            return false;
        }

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

    private static bool HasMixedOptionOperandAlternatives(string content)
    {
        var alternatives = GetAlternatives(content);
        return !HasOptionValueAlternatives(content)
               && alternatives.Length > 1
               && alternatives.Any(IsOptionAlternative)
               && alternatives.Any(IsOperandAlternative);
    }

    private static bool HasOptionValueAlternatives(string content)
    {
        var normalized = TrimControlWrappers(content);
        if (HasOptionAssignment(normalized))
        {
            return true;
        }

        var valueStartIndex = GetOptionValueStartIndex(normalized);

        return valueStartIndex > 1
               && valueStartIndex < normalized.Length
               && normalized.StartsWith('-')
               && normalized[valueStartIndex] != '|'
               && normalized.IndexOf('|', valueStartIndex + 1) >= 0;
    }

    private static int GetOptionValueStartIndex(string content)
    {
        var valueStartIndex = SkipWhitespace(content, content.IndexOfAny([' ', '\t']));
        while (valueStartIndex >= 0
               && valueStartIndex < content.Length
               && content[valueStartIndex] == '|')
        {
            var aliasStartIndex = SkipWhitespace(content, valueStartIndex + 1);
            if (aliasStartIndex >= content.Length || content[aliasStartIndex] != '-')
            {
                return valueStartIndex;
            }

            var aliasEndIndex = content.IndexOfAny([' ', '\t', '='], aliasStartIndex);
            if (aliasEndIndex < 0)
            {
                return content.Length;
            }

            valueStartIndex = SkipWhitespace(content, aliasEndIndex);
        }

        return valueStartIndex;
    }

    private static int SkipWhitespace(string content, int startIndex)
    {
        while (startIndex >= 0
               && startIndex < content.Length
               && char.IsWhiteSpace(content[startIndex]))
        {
            startIndex++;
        }

        return startIndex;
    }

    private static bool HasOptionAssignment(string content)
    {
        var assignmentIndex = content.IndexOf('=');
        if (assignmentIndex <= 1 || !content.StartsWith('-'))
        {
            return false;
        }

        var branchStartIndex = content.LastIndexOf('|', assignmentIndex) + 1;
        while (branchStartIndex < assignmentIndex && char.IsWhiteSpace(content[branchStartIndex]))
        {
            branchStartIndex++;
        }

        return branchStartIndex + 1 < assignmentIndex
               && content[branchStartIndex] == '-';
    }

    private static bool HasLoneDashOperandAlternatives(string content)
    {
        var alternatives = GetAlternatives(content);
        return alternatives.Length > 1
               && alternatives.Any(static alternative => TrimControlWrappers(alternative) == "-")
               && alternatives.Any(static alternative =>
                   TrimControlWrappers(alternative) != "-"
                   && IsOperandAlternative(alternative));
    }

    private static bool HasOnlyOptionControlAlternatives(string content)
    {
        var alternatives = GetAlternatives(content);
        return alternatives.Length > 1
               && alternatives.All(static alternative =>
               {
                   var normalized = TrimControlWrappers(alternative);
                   return IsOptionAlternative(normalized)
                          || IsOptionControlLabel(normalized);
               });
    }

    private static string[] GetAlternatives(string content) => content.Split(
        '|',
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    private static bool IsOptionAlternative(string alternative)
    {
        var content = TrimControlWrappers(alternative);
        return content.Length > 1 && content.StartsWith('-');
    }

    private static bool IsOperandAlternative(string alternative)
    {
        var content = TrimControlWrappers(alternative);
        return !string.IsNullOrWhiteSpace(content)
               && !IsOptionAlternative(content)
               && !IsOptionControlLabel(content);
    }

    private static string TrimControlWrappers(string token)
    {
        var content = token.Trim();
        while (true)
        {
            content = content.TrimEnd(',', ':', ';');
            if (!IsWrapped(content))
            {
                return content;
            }

            content = TrimWrapper(content).Trim();
        }
    }

    private static bool TokenMatchesCommandPart(string token, string commandPart)
    {
        var normalized = NormalizeLiteral(token);
        return normalized.Equals(commandPart, StringComparison.OrdinalIgnoreCase)
               || normalized.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                   .Contains(commandPart, StringComparer.OrdinalIgnoreCase);
    }

    private static string NormalizeLiteral(string token) =>
        TrimWrapper(token.Trim().TrimEnd(',', ':')).Trim();

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

    internal bool SupportsRequiredAlternativeInference { get; init; }

    public IReadOnlyList<CliPositionalArgument> PositionalArguments { get; init; } = [];

    public IReadOnlyList<string> UnparsedOperandTokens { get; init; } = [];

    public IReadOnlyList<UsageRequiredAlternativeGroup> RequiredAlternativeGroups { get; init; } = [];
}

/// <summary>
/// A required choice recovered from one or more usage synopses.
/// </summary>
public sealed record UsageRequiredAlternativeGroup
{
    /// <summary>
    /// Option switches and positional properties participating in the choice.
    /// </summary>
    public required IReadOnlyList<UsageRequiredAlternativeMember> Members { get; init; }
}

/// <summary>
/// An option or positional operand participating in a required usage choice.
/// </summary>
public sealed record UsageRequiredAlternativeMember
{
    /// <summary>
    /// Option spelling when this member is supplied through a named option.
    /// </summary>
    public string? OptionSwitch { get; init; }

    /// <summary>
    /// Generated positional property name when this member is a true operand.
    /// </summary>
    public string? PositionalPropertyName { get; init; }
}
