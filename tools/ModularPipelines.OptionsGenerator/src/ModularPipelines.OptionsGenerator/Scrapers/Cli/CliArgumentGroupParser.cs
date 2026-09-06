using System.Text.RegularExpressions;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// Recovers nested argument groups from indentation-based CLI help.
/// </summary>
internal static partial class CliArgumentGroupParser
{
    public static CliArgumentGroup Parse(
        string section,
        Func<string, CliArgumentDefinition?> parseArgument)
    {
        ArgumentNullException.ThrowIfNull(section);
        ArgumentNullException.ThrowIfNull(parseArgument);

        var lines = section.ReplaceLineEndings("\n").Split('\n');
        var declarations = ParseDeclarations(lines, parseArgument);
        if (declarations.Count == 0)
        {
            return new CliArgumentGroup
            {
                Description = NormalizeDocumentation(section),
            };
        }

        var firstPreludeLines = lines[..declarations[0].LineIndex];
        var firstPrelude = NormalizeDocumentation(firstPreludeLines);
        var firstPreludeStartsGroup = StartsArgumentGroup(firstPreludeLines, firstPrelude);
        var root = new ArgumentGroupBuilder(
            declarations.Min(declaration => declaration.Argument.Indentation),
            firstPreludeStartsGroup ? null : firstPrelude);
        var stack = new Stack<ArgumentGroupBuilder>();
        stack.Push(root);
        var previousDescriptionEnd = 0;

        for (var index = 0; index < declarations.Count; index++)
        {
            var declaration = declarations[index];
            var nextLineIndex = index + 1 < declarations.Count
                ? declarations[index + 1].LineIndex
                : lines.Length;
            var descriptionEnd = FindDescriptionEnd(
                lines,
                declaration.LineIndex + 1,
                nextLineIndex,
                declaration.Argument.Indentation);
            var description = NormalizeDocumentation(
                lines[(declaration.LineIndex + 1)..descriptionEnd]);
            var preludeStart = index == 0 ? 0 : previousDescriptionEnd;
            var preludeLines = lines[preludeStart..declaration.LineIndex];
            var prelude = NormalizeDocumentation(preludeLines);
            var preludeIndentation = GetMinimumContentIndentation(preludeLines);
            var preludeStartsGroup = StartsArgumentGroup(preludeLines, prelude);
            previousDescriptionEnd = descriptionEnd;

            MoveToContainingGroup(
                stack,
                declaration.Argument.Indentation,
                preludeIndentation);
            AddArgument(
                stack,
                declaration.Argument with
                {
                    Description = description,
                    SourceOrder = declaration.LineIndex,
                },
                index == 0 && !preludeStartsGroup ? null : prelude,
                preludeStartsGroup);
        }

        return root.Build();
    }

    private static List<ParsedArgumentLine> ParseDeclarations(
        string[] lines,
        Func<string, CliArgumentDefinition?> parseArgument)
    {
        var declarations = new List<ParsedArgumentLine>();
        for (var index = 0; index < lines.Length; index++)
        {
            if (parseArgument(lines[index]) is not { } argument)
            {
                continue;
            }

            if (declarations.Count > 0
                && IsInsideDocumentationBlock(lines, declarations[^1], index, argument.Indentation))
            {
                continue;
            }

            declarations.Add(new ParsedArgumentLine(index, argument));
        }

        return declarations;
    }

    private static bool IsInsideDocumentationBlock(
        IReadOnlyList<string> lines,
        ParsedArgumentLine previousDeclaration,
        int candidateIndex,
        int candidateIndentation)
    {
        var declarationIndentation = previousDeclaration.Argument.Indentation;
        if (candidateIndentation <= declarationIndentation)
        {
            return false;
        }

        return lines
            .Skip(previousDeclaration.LineIndex + 1)
            .Take(candidateIndex - previousDeclaration.LineIndex - 1)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .All(line => CliScraperBase.GetIndentation(line) > declarationIndentation);
    }

    private static void MoveToContainingGroup(
        Stack<ArgumentGroupBuilder> stack,
        int argumentIndentation,
        int preludeIndentation)
    {
        while (stack.Count > 1
               && (argumentIndentation < stack.Peek().Indentation
                   || preludeIndentation < stack.Peek().Indentation))
        {
            stack.Pop();
        }
    }

    private static void AddArgument(
        Stack<ArgumentGroupBuilder> stack,
        CliArgumentDefinition argument,
        string? prelude,
        bool preludeStartsGroup)
    {
        var current = stack.Peek();
        if (argument.Indentation > current.Indentation)
        {
            var child = new ArgumentGroupBuilder(argument.Indentation, prelude);
            current.Groups.Add(child);
            stack.Push(child);
        }
        else if (preludeStartsGroup)
        {
            if (stack.Count > 1)
            {
                stack.Pop();
            }

            var sibling = new ArgumentGroupBuilder(argument.Indentation, prelude);
            stack.Peek().Groups.Add(sibling);
            stack.Push(sibling);
        }
        else if (!string.IsNullOrWhiteSpace(prelude))
        {
            current.AppendDescription(prelude);
        }

        stack.Peek().Arguments.Add(argument);
    }

    private static int FindDescriptionEnd(
        string[] lines,
        int start,
        int end,
        int declarationIndentation)
    {
        for (var index = start; index < end; index++)
        {
            if (!string.IsNullOrWhiteSpace(lines[index])
                && CliScraperBase.GetIndentation(lines[index]) <= declarationIndentation)
            {
                return index;
            }
        }

        return end;
    }

    private static int GetMinimumContentIndentation(IEnumerable<string> lines)
    {
        var indentations = lines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(CliScraperBase.GetIndentation)
            .ToArray();

        return indentations.Length == 0 ? int.MaxValue : indentations.Min();
    }

    private static string? NormalizeDocumentation(IEnumerable<string> lines)
    {
        var text = string.Join(
            " ",
            lines
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line)));

        return string.IsNullOrWhiteSpace(text)
            ? null
            : WhitespacePattern().Replace(text, " ").Trim();
    }

    private static string? NormalizeDocumentation(string text) =>
        NormalizeDocumentation(text.ReplaceLineEndings("\n").Split('\n'));

    private static CliArgumentGroupKind Classify(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return CliArgumentGroupKind.None;
        }

        // Help headings are free-form prose, so classification is deliberately best-effort.
        // Tool adapters can retain the group tree even when their phrasing maps to None.
        var kind = CliArgumentGroupKind.None;
        if (AtMostOnePattern().IsMatch(description))
        {
            kind |= CliArgumentGroupKind.AtMostOne;
        }

        if (AtLeastOnePattern().IsMatch(description))
        {
            kind |= CliArgumentGroupKind.AtLeastOne;
        }

        if (AlternativePattern().IsMatch(description))
        {
            kind |= CliArgumentGroupKind.Alternative;
        }

        if (ResourcePattern().IsMatch(description))
        {
            kind |= CliArgumentGroupKind.Resource;
        }

        return kind;
    }

    private static bool StartsArgumentGroup(
        IEnumerable<string> lines,
        string? description) =>
        Classify(description) != CliArgumentGroupKind.None
        || lines.Any(line => SectionHeadingPattern().IsMatch(line.Trim()));

    private sealed class ArgumentGroupBuilder(int indentation, string? description)
    {
        public int Indentation { get; } = indentation;

        public string? Description { get; private set; } = description;

        public List<CliArgumentDefinition> Arguments { get; } = [];

        public List<ArgumentGroupBuilder> Groups { get; } = [];

        public void AppendDescription(string additionalDescription)
        {
            Description = string.IsNullOrWhiteSpace(Description)
                ? additionalDescription
                : $"{Description} {additionalDescription}";
        }

        public CliArgumentGroup Build() => new()
        {
            Description = Description,
            Kind = Classify(Description),
            Arguments = Arguments,
            Groups = Groups.Select(group => group.Build()).ToArray(),
        };
    }

    private sealed record ParsedArgumentLine(
        int LineIndex,
        CliArgumentDefinition Argument);

    [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant)]
    private static partial Regex WhitespacePattern();

    [GeneratedRegex(@"\bat most one\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex AtMostOnePattern();

    [GeneratedRegex(@"\bat least one\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex AtLeastOnePattern();

    [GeneratedRegex(@"(?:^|[.!?]\s+)or\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex AlternativePattern();

    [GeneratedRegex(@"\b(?:resource|arguments? for)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ResourcePattern();

    [GeneratedRegex(@"^[\p{L}\p{N}].* Flags:?$", RegexOptions.CultureInvariant)]
    private static partial Regex SectionHeadingPattern();
}
