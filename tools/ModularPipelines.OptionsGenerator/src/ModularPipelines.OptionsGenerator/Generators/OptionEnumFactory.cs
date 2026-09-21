using System.Buffers;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

internal static class OptionEnumFactory
{
    private static readonly SearchValues<char> HintSyntaxDelimiters = SearchValues.Create("<>{}[]()");

    // Inferred hints need a lexical guard; authoritative structured choices may
    // contain numeric or symbolic literals and must retain their original values.
    public static CliEnumDefinition? TryCreateFromHint(
        string className,
        string propertyName,
        string switchName,
        IReadOnlyList<string> values) =>
        values.Any(value => !value.Any(char.IsLetter) || value.AsSpan().IndexOfAny(HintSyntaxDelimiters) >= 0)
            ? null
            : TryCreate(className, propertyName, switchName, values);

    public static string UnwrapChoiceHint(string hint)
    {
        var trimmed = hint.Trim();
        return trimmed.Length >= 2
               && (trimmed[0], trimmed[^1]) is ('<', '>') or ('{', '}') or ('[', ']') or ('(', ')')
            ? trimmed[1..^1].Trim()
            : trimmed;
    }

    public static CliEnumDefinition? TryCreate(
        string className,
        string propertyName,
        string switchName,
        IEnumerable<string> values) =>
        TryCreate(className, propertyName, switchName, values.Select(value => (value, (string?) null)));

    public static CliEnumDefinition? TryCreate(
        string className,
        string propertyName,
        string switchName,
        IEnumerable<(string Value, string? Description)> values)
    {
        var distinctValues = values.GroupBy(value => value.Value, StringComparer.Ordinal).ToArray();
        if (distinctValues.Length is < 2 or > 20)
        {
            return null;
        }

        var members = distinctValues.Select(group => new CliEnumValue
        {
            CliValue = group.Key,
            MemberName = GeneratorUtils.ToEnumMemberName(group.Key),
            Description = MergeDocumentation(group.Select(value => value.Description)),
        }).ToArray();
        var prefix = className.EndsWith("Options", StringComparison.Ordinal) ? className[..^"Options".Length] : className;
        return new CliEnumDefinition
        {
            EnumName = $"{prefix}{propertyName}",
            Values = EnumGenerator.GetUniqueValues(members),
            Description = $"Allowed values for {switchName}.",
        };
    }

    private static string? MergeDocumentation(IEnumerable<string?> descriptions)
    {
        var distinct = descriptions.Where(description => !string.IsNullOrWhiteSpace(description))
            .Select(description => description!.Trim())
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .ToArray();
        return distinct.Length > 0 ? string.Join(' ', distinct) : null;
    }

    public static string? PreserveValueHint(CliEnumDefinition? enumDefinition, string? description, string? valueHint)
    {
        if (enumDefinition is not null || string.IsNullOrWhiteSpace(valueHint)
            || (!valueHint.Contains('|') && !valueHint.StartsWith('{') && UnwrapChoiceHint(valueHint).Length != 0))
        {
            return description;
        }

        return AppendMetadata(description, $"[value type: {valueHint}]");
    }

    public static string PreserveChoices(string? description, IEnumerable<string> values) =>
        AppendMetadata(description, $"[possible values: {string.Join(", ", values)}]");

    private static string AppendMetadata(string? description, string metadata) =>
        description?.Contains(metadata, StringComparison.Ordinal) == true
            ? description
            : $"{description} {metadata}".Trim();
}
