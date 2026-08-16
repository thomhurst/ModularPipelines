using System.Text.RegularExpressions;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Preserves generated enum members, CLI values, and numeric values across regeneration.
/// </summary>
internal static partial class EnumDefinitionStabilizer
{
    private static readonly HashSet<string> SuspiciousProseValues = new(StringComparer.OrdinalIgnoreCase)
    {
        "accepts",
        "them",
    };

    public static CliToolDefinition Stabilize(
        CliToolDefinition tool,
        string outputDirectory,
        IReadOnlyDictionary<string, string>? fallbackExistingFiles = null)
    {
        var stabilizedEnums = tool.AllEnums.ToDictionary(
            definition => definition.EnumName,
            definition => Stabilize(
                definition,
                Path.Combine(
                    outputDirectory,
                    tool.OutputDirectory,
                    "Enums",
                    $"{definition.EnumName}.Generated.cs"),
                fallbackExistingFiles?.GetValueOrDefault(definition.EnumName)),
            StringComparer.Ordinal);

        var commands = tool.Commands
            .Select(command => command with
            {
                Enums = command.Enums.Select(definition => stabilizedEnums[definition.EnumName]).ToList(),
                Options = StabilizeOptions(command.Options, stabilizedEnums),
            })
            .ToList();

        return tool with
        {
            Commands = commands,
            GlobalOptions = StabilizeOptions(tool.GlobalOptions, stabilizedEnums),
            CompatibilityEnums = tool.CompatibilityEnums
                .Select(definition => stabilizedEnums[definition.EnumName])
                .ToList(),
        };
    }

    private static IReadOnlyList<CliOptionDefinition> StabilizeOptions(
        IReadOnlyList<CliOptionDefinition> options,
        IReadOnlyDictionary<string, CliEnumDefinition> stabilizedEnums) =>
        options.Select(option => option.EnumDefinition is null
                ? option
                : option with { EnumDefinition = stabilizedEnums[option.EnumDefinition.EnumName] })
            .ToList();

    private static CliEnumDefinition Stabilize(
        CliEnumDefinition definition,
        string existingFile,
        string? fallbackExistingFile)
    {
        ValidateValues(definition);

        var baselineFile = File.Exists(existingFile)
            ? existingFile
            : fallbackExistingFile;
        var existingValues = baselineFile is not null && File.Exists(baselineFile)
            ? ParseExistingValues(File.ReadAllText(baselineFile))
            : [];

        var existingByCliValue = existingValues.ToDictionary(value => value.CliValue, StringComparer.Ordinal);
        var incomingByCliValue = definition.Values.ToDictionary(value => value.CliValue, StringComparer.Ordinal);
        var stabilizedValues = new List<CliEnumValue>(definition.Values.Count);

        foreach (var existingValue in existingValues)
        {
            if (incomingByCliValue.TryGetValue(existingValue.CliValue, out var incomingValue))
            {
                stabilizedValues.Add(incomingValue with
                {
                    MemberName = existingValue.MemberName,
                    NumericValue = existingValue.NumericValue,
                });
                continue;
            }

            var reusedMember = definition.Values.FirstOrDefault(value => value.MemberName.Equals(
                existingValue.MemberName,
                StringComparison.Ordinal));
            if (reusedMember is not null)
            {
                throw new InvalidOperationException(
                    $"Enum '{definition.EnumName}' member '{existingValue.MemberName}' changed CLI value from "
                    + $"'{existingValue.CliValue}' to '{reusedMember.CliValue}'.");
            }

            stabilizedValues.Add(new CliEnumValue
            {
                MemberName = existingValue.MemberName,
                CliValue = existingValue.CliValue,
                NumericValue = existingValue.NumericValue,
            });
        }

        var usedNumericValues = existingValues
            .Select(value => value.NumericValue)
            .ToHashSet();
        var newValues = definition.Values
            .Where(value => !existingByCliValue.ContainsKey(value.CliValue))
            .ToList();
        var nextNumericValue = newValues.Count > 0 && usedNumericValues.Count > 0
            ? checked(usedNumericValues.Max() + 1)
            : 0;

        for (var index = 0; index < newValues.Count; index++)
        {
            var incomingValue = newValues[index];
            while (usedNumericValues.Contains(nextNumericValue))
            {
                nextNumericValue = checked(nextNumericValue + 1);
            }

            stabilizedValues.Add(incomingValue with { NumericValue = nextNumericValue });
            usedNumericValues.Add(nextNumericValue);
            if (index < newValues.Count - 1)
            {
                nextNumericValue = checked(nextNumericValue + 1);
            }
        }

        ValidateUniqueMemberNames(definition.EnumName, stabilizedValues);
        return definition with { Values = stabilizedValues };
    }

    private static void ValidateValues(CliEnumDefinition definition)
    {
        var duplicateCliValue = definition.Values
            .GroupBy(value => value.CliValue, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicateCliValue is not null)
        {
            throw new InvalidOperationException(
                $"Enum '{definition.EnumName}' contains duplicate CLI value '{duplicateCliValue.Key}'.");
        }

        var duplicateMember = definition.Values
            .GroupBy(value => value.MemberName, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicateMember is not null)
        {
            throw new InvalidOperationException(
                $"Enum '{definition.EnumName}' contains duplicate member '{duplicateMember.Key}'.");
        }

        var suspiciousValue = definition.Values
            .FirstOrDefault(value => SuspiciousProseValues.Contains(value.CliValue));
        if (suspiciousValue is not null)
        {
            throw new InvalidOperationException(
                $"Enum '{definition.EnumName}' contains suspicious prose value '{suspiciousValue.CliValue}'.");
        }
    }

    private static void ValidateUniqueMemberNames(
        string enumName,
        IReadOnlyList<CliEnumValue> values)
    {
        var duplicateMemberName = values
            .GroupBy(value => value.MemberName, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicateMemberName is not null)
        {
            throw new InvalidOperationException(
                $"Enum '{enumName}' contains duplicate member name '{duplicateMemberName.Key}' after stabilization.");
        }
    }

    private static IReadOnlyList<ExistingEnumValue> ParseExistingValues(string content)
    {
        var values = new List<ExistingEnumValue>();
        var nextNumericValue = 0;

        foreach (Match match in ExistingEnumValuePattern().Matches(content))
        {
            var numericValue = match.Groups["number"].Success
                ? int.Parse(match.Groups["number"].Value, System.Globalization.CultureInfo.InvariantCulture)
                : nextNumericValue;
            var cliValue = Regex.Unescape(match.Groups["cliValue"].Value);
            var memberName = match.Groups["member"].Value;

            values.Add(new ExistingEnumValue(cliValue, memberName, numericValue));
            nextNumericValue = checked(numericValue + 1);
        }

        var duplicateCliValue = values
            .GroupBy(value => value.CliValue, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicateCliValue is not null)
        {
            throw new InvalidOperationException(
                $"Existing generated enum contains duplicate CLI value '{duplicateCliValue.Key}'.");
        }

        var duplicateNumericValue = values
            .GroupBy(value => value.NumericValue)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicateNumericValue is not null)
        {
            throw new InvalidOperationException(
                $"Existing generated enum contains duplicate numeric value '{duplicateNumericValue.Key}'.");
        }

        return values;
    }

    // Description was emitted by generators before EnumValue existed. Retaining this migration
    // shape preserves established ordinals on the first regeneration of older enum files.
    [GeneratedRegex(
        """\[(?:EnumValue|Description)\("(?<cliValue>(?:\\.|[^"\\])*)"\)\]\s*(?<member>[\p{L}_][\p{L}\p{Nd}_]*)(?:\s*=\s*(?<number>-?\d+))?\s*(?:,|})""")]
    private static partial Regex ExistingEnumValuePattern();

    private sealed record ExistingEnumValue(string CliValue, string MemberName, int NumericValue);
}
