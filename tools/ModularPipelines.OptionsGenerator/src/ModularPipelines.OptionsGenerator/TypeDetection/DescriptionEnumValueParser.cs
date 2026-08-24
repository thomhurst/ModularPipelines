using System.Text.RegularExpressions;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

internal enum DescriptionEnumMatchKind
{
    Explicit,
    ContextualParenthesized
}

internal sealed record DescriptionEnumMatch(string[] Values, DescriptionEnumMatchKind MatchKind);

internal static partial class DescriptionEnumValueParser
{
    public static DescriptionEnumMatch? TryParse(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return null;
        }

        var explicitMatch = ExplicitValuesPattern().Match(description);
        if (explicitMatch.Success)
        {
            return CreateMatch(explicitMatch, DescriptionEnumMatchKind.Explicit);
        }

        var quotedMatch = QuotedOneOfValuesPattern().Match(description);
        if (quotedMatch.Success)
        {
            return CreateMatch(quotedMatch, DescriptionEnumMatchKind.Explicit);
        }

        var parenthesizedMatch = ContextualParenthesizedValuesPattern().Match(description);
        return parenthesizedMatch.Success
            ? CreateMatch(parenthesizedMatch, DescriptionEnumMatchKind.ContextualParenthesized)
            : null;
    }

    public static string[]? TryParseValues(string valuesText)
    {
        valuesText = valuesText.Trim().TrimEnd('.', ';', ':').Trim();
        if (valuesText.Length >= 2
            && ((valuesText[0] == '(' && valuesText[^1] == ')')
                || (valuesText[0] == '[' && valuesText[^1] == ']')))
        {
            valuesText = valuesText[1..^1];
        }

        var candidates = EnumValueSeparatorPattern().Split(valuesText);
        var values = new List<string>(candidates.Length);

        foreach (var candidate in candidates)
        {
            var value = LeadingConjunctionPattern().Replace(candidate.Trim(), "")
                .TrimEnd('.', ';', ':')
                .Trim()
                .Trim('"', '\'', '`');

            if (!EnumValuePattern().IsMatch(value) || !value.Any(char.IsLetter))
            {
                return null;
            }

            values.Add(value);
        }

        var distinctValues = values.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        return distinctValues.Length is >= 2 and <= 20 ? distinctValues : null;
    }

    private static DescriptionEnumMatch? CreateMatch(Match match, DescriptionEnumMatchKind matchKind)
    {
        var values = TryParseValues(match.Groups["values"].Value);
        return values is null ? null : new DescriptionEnumMatch(values, matchKind);
    }

    [GeneratedRegex("""\b(?:must be one of|one of|valid values|allowed values|possible values|accepted values)\b\s*:\s*[\[(]?(?<values>[^\s,|)\]]+(?:(?:\s*(?:\||,)\s*(?:(?:or|and)\s+)?|\s+(?:or|and)\s+)[^\s,|)\]]+)+)""", RegexOptions.IgnoreCase)]
    private static partial Regex ExplicitValuesPattern();

    [GeneratedRegex("""\b(?:must be(?:\s+one of)?|one of)\b\s+(?<values>["'`][A-Za-z0-9][A-Za-z0-9_-]{0,28}["'`](?:(?:\s*,\s*(?:(?:or|and)\s+)?|\s+(?:or|and)\s+)["'`][A-Za-z0-9][A-Za-z0-9_-]{0,28}["'`])+)""", RegexOptions.IgnoreCase)]
    private static partial Regex QuotedOneOfValuesPattern();

    [GeneratedRegex("""\b(?:formats?|types?|modes?|levels?|preferences?)\b[^()\r\n]{0,40}\((?<values>[^\s,|)]+(?:\s*(?:\||,)\s*(?:or\s+|and\s+)?[^\s,|)]+)+)\)""", RegexOptions.IgnoreCase)]
    private static partial Regex ContextualParenthesizedValuesPattern();

    [GeneratedRegex(@"(?:\s*(?:\||,)\s*(?:(?:or|and)\s+)?|\s+(?:or|and)\s+)", RegexOptions.IgnoreCase)]
    private static partial Regex EnumValueSeparatorPattern();

    [GeneratedRegex(@"^(?:or|and)\s+", RegexOptions.IgnoreCase)]
    private static partial Regex LeadingConjunctionPattern();

    [GeneratedRegex(@"^[A-Za-z0-9][A-Za-z0-9_-]{0,28}$")]
    private static partial Regex EnumValuePattern();
}
