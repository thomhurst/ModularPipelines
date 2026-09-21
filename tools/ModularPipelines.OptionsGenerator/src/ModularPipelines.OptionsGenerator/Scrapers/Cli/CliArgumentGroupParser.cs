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
        Func<string, CliArgumentDefinition?> parseArgument,
        IReadOnlyList<IReadOnlySet<string>>? optionalOptionGroups = null)
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
        var firstPreludeIndentation = GetMinimumContentIndentation(firstPreludeLines);
        var firstPreludeStartsGroup = StartsArgumentGroup(firstPreludeLines, firstPrelude)
            || firstPreludeIndentation <= declarations[0].Argument.Indentation;
        var root = new ArgumentGroupBuilder(
            Math.Min(firstPreludeIndentation, declarations.Min(declaration => declaration.Argument.Indentation)),
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
            BeginNamedBundle(stack, ref preludeLines);
            var prelude = NormalizeDocumentation(preludeLines);
            var preludeIndentation = GetMinimumContentIndentation(preludeLines);
            previousDescriptionEnd = descriptionEnd;

            MoveToContainingGroup(
                stack,
                declaration.Argument.Indentation,
                preludeIndentation);
            // Plain prose can separate unnamed groups, but cannot remove a sibling
            // argument from an established constraint group without an explicit heading.
            var preludeStartsGroup = StartsArgumentGroup(preludeLines, prelude)
                || (preludeIndentation <= declaration.Argument.Indentation
                    && Classify(stack.Peek().Description) == CliArgumentGroupKind.None);
            // A peer heading can introduce flags indented deeper than the previous
            // group's flags. Compare headings before treating that depth as nesting.
            // Within a classified branch, same-depth constraints still belong to that branch.
            // A synopsis can also establish a shared optional bundle when prose is unclassified.
            var sharesOptionalGroup = stack.Peek().Arguments.Count > 0
                && optionalOptionGroups?.Any(group => group.Contains(declaration.Argument.SwitchName)
                    && stack.Peek().Arguments.All(argument => group.Contains(argument.SwitchName))) == true;
            var isNestedConstraint =
                ((Classify(prelude) & (CliArgumentGroupKind.AtLeastOne | CliArgumentGroupKind.AtMostOne)) != 0
                    || DescribesRequiredBundle(prelude))
                && (sharesOptionalGroup || stack.Skip(1).Any(group =>
                    group.IsNamedBundle || Classify(group.Description) != CliArgumentGroupKind.None));
            while (stack.Count > 1 && preludeStartsGroup
                   && !isNestedConstraint
                   && preludeIndentation <= stack.Peek().HeadingIndentation
                   && declaration.Argument.Indentation > stack.Peek().Indentation
                   && !stack.Peek().IsNamedBundle)
            {
                stack.Pop();
            }

            var parsedArgument = declaration.Argument with
            {
                Description = description,
                SourceOrder = declaration.LineIndex,
            };
            if (TryAddNestedPreludeGroup(stack, parsedArgument, preludeLines))
            {
                continue;
            }
            AddArgument(
                stack,
                parsedArgument,
                index == 0 && !preludeStartsGroup ? null : prelude,
                preludeStartsGroup,
                preludeIndentation);
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

            // Long switch names wrap onto their own line in resource-help bullets.
            // They refer to a later declaration; they do not declare boolean flags.
            if (index > 0 && WrappedArgumentReferencePattern().IsMatch(lines[index - 1]))
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

    private static void BeginNamedBundle(Stack<ArgumentGroupBuilder> stack, ref string[] preludeLines)
    {
        var heading = Array.FindIndex(preludeLines, line => NamedBundleHeadingPattern().IsMatch(line.Trim()));
        if (heading < 0)
        {
            return;
        }

        var prefix = preludeLines[..heading];
        var prefixDescription = NormalizeDocumentation(prefix);
        var prefixKind = Classify(prefixDescription);
        const CliArgumentGroupKind choiceKinds = CliArgumentGroupKind.AtLeastOne | CliArgumentGroupKind.AtMostOne;
        if ((prefixKind & choiceKinds) == 0 && !stack.Any(group => (Classify(group.Description) & choiceKinds) != 0))
        {
            return;
        }
        var indentation = CliScraperBase.GetIndentation(preludeLines[heading]);
        MoveToContainingGroup(stack, indentation, GetMinimumContentIndentation(prefix));
        if ((prefixKind & choiceKinds) != 0)
        {
            BeginArgumentGroup(stack, GetMinimumContentIndentation(prefix) + 1, prefixDescription, true,
                headingIndentation: GetMinimumContentIndentation(prefix));
        }

        var end = heading + 1;
        while (end < preludeLines.Length && !string.IsNullOrWhiteSpace(preludeLines[end]))
        {
            end++;
        }

        // Named configurations are whole branches. Their resource groups and
        // direct flags may share the heading's indentation in gcloud help.
        BeginArgumentGroup(stack, indentation, NormalizeDocumentation(preludeLines[heading..end]), true, namedBundle: true);
        stack.Peek().IsNamedBundle = true;
        preludeLines = preludeLines[end..];
    }

    private static void AddArgument(
        Stack<ArgumentGroupBuilder> stack,
        CliArgumentDefinition argument,
        string? prelude,
        bool preludeStartsGroup,
        int headingIndentation)
    {
        BeginArgumentGroup(stack, argument.Indentation, prelude, preludeStartsGroup,
            headingIndentation: headingIndentation);
        stack.Peek().Arguments.Add(argument);
    }

    private static bool TryAddNestedPreludeGroup(
        Stack<ArgumentGroupBuilder> stack, CliArgumentDefinition argument, string[] preludeLines)
    {
        var childStart = Array.FindIndex(preludeLines, line => !string.IsNullOrWhiteSpace(line)
            && CliScraperBase.GetIndentation(line) == argument.Indentation);
        if (childStart <= 0)
        {
            return false;
        }
        var parentDescription = NormalizeDocumentation(preludeLines[..childStart]);
        var kind = Classify(parentDescription);
        if ((kind & (CliArgumentGroupKind.AtLeastOne | CliArgumentGroupKind.AtMostOne)) == 0)
        {
            return false;
        }

        // A constraint heading can precede the first branch heading before any option appears.
        // Keep the choice boundary just below its heading: heading-level peers are outside,
        // while resource branches can indent their flags further than a sibling flag.
        // "Or" headings continue the existing choice and retain their nested branch depth.
        var parentIndentation = kind.HasFlag(CliArgumentGroupKind.Alternative)
            ? argument.Indentation
            : GetMinimumContentIndentation(preludeLines[..childStart]) + 1;
        BeginArgumentGroup(stack, parentIndentation, parentDescription, true,
            headingIndentation: GetMinimumContentIndentation(preludeLines[..childStart]));
        var branch = new ArgumentGroupBuilder(argument.Indentation,
            NormalizeDocumentation(preludeLines[childStart..]));
        stack.Peek().Groups.Add(branch);
        stack.Push(branch);
        branch.Arguments.Add(argument);
        return true;
    }

    private static void BeginArgumentGroup(
        Stack<ArgumentGroupBuilder> stack, int indentation, string? prelude, bool preludeStartsGroup,
        bool namedBundle = false, int? headingIndentation = null)
    {
        var current = stack.Peek();
        if (indentation == current.Indentation && current.IsNamedBundle && preludeStartsGroup && !namedBundle)
        {
            // Resource prose can sit at the same depth as its provider heading.
            // End this resource before the next peer flag, while retaining the provider.
            indentation++;
        }
        if (indentation > current.Indentation)
        {
            var child = new ArgumentGroupBuilder(indentation, prelude, headingIndentation);
            current.Groups.Add(child);
            stack.Push(child);
        }
        else if (preludeStartsGroup)
        {
            if (stack.Count > 1)
            {
                stack.Pop();
            }

            var sibling = new ArgumentGroupBuilder(indentation, prelude, headingIndentation);
            stack.Peek().Groups.Add(sibling);
            stack.Push(sibling);
        }
        else if (!string.IsNullOrWhiteSpace(prelude))
        {
            current.AppendDescription(prelude);
        }
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
        if (ExactlyOnePattern().IsMatch(description))
        {
            kind |= CliArgumentGroupKind.AtLeastOne | CliArgumentGroupKind.AtMostOne;
        }
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

    internal static bool DescribesRequiredBundle(string? description) =>
        RequiredBundleMarkerPattern().IsMatch(description ?? string.Empty);

    [GeneratedRegex(@"\bThis must be specified(?:[.:]|$)", RegexOptions.IgnoreCase)]
    private static partial Regex RequiredBundleMarkerPattern();
    private static bool StartsArgumentGroup(
        IReadOnlyList<string> lines,
        string? description) =>
        Classify(description) != CliArgumentGroupKind.None
        || DescribesRequiredBundle(description)
        || ContainsConfigurationHeading(lines)
        || lines.Any(line => SectionHeadingPattern().IsMatch(line.Trim()));

    private static bool ContainsConfigurationHeading(IReadOnlyList<string> lines)
    {
        for (var index = 0; index < lines.Count; index++)
        {
            if (!ConfigurationHeadingPattern().IsMatch(lines[index].Trim()))
            {
                continue;
            }

            // gcloud separates heading paragraphs from their flags with a blank line.
            // Similar prose immediately before a flag remains in its existing group.
            while (index < lines.Count && !string.IsNullOrWhiteSpace(lines[index]))
            {
                index++;
            }

            if (index < lines.Count)
            {
                return true;
            }
        }

        return false;
    }

    private sealed class ArgumentGroupBuilder(int indentation, string? description, int? headingIndentation = null)
    {
        public int Indentation { get; } = indentation;

        public int HeadingIndentation { get; } = Math.Min(headingIndentation ?? indentation, indentation);

        public bool IsNamedBundle { get; set; }

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
            Groups = [.. Groups.Select(group => group.Build())],
        };
    }

    private sealed record ParsedArgumentLine(
        int LineIndex,
        CliArgumentDefinition Argument);

    [GeneratedRegex(@"^\s*\S\s+provide the argument\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex WrappedArgumentReferencePattern();

    [GeneratedRegex(@"^(?:(?:Defines the )?configuration for|(?:Bearer token|Basic) authentication with)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex NamedBundleHeadingPattern();

    [GeneratedRegex(@"^(?:(?:[\w-]+\s+)*configuration for\b|options for\b)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ConfigurationHeadingPattern();

    [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant)]
    private static partial Regex WhitespacePattern();

    [GeneratedRegex(@"\bat most one\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex AtMostOnePattern();

    [GeneratedRegex(@"\bexactly one\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ExactlyOnePattern();

    [GeneratedRegex(@"\bat least one\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex AtLeastOnePattern();

    [GeneratedRegex(@"(?:^|[.!?]\s+)or\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex AlternativePattern();

    [GeneratedRegex(@"\b(?:resource|arguments? for)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ResourcePattern();

    [GeneratedRegex(@"^[\p{L}\p{N}].* Flags:?$", RegexOptions.CultureInvariant)]
    private static partial Regex SectionHeadingPattern();
}
