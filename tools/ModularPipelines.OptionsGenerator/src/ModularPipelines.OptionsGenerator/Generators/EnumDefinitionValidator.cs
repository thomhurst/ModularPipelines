using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

internal static class EnumDefinitionValidator
{
    private static readonly HashSet<string> SuspiciousProseValues = new(StringComparer.OrdinalIgnoreCase)
    {
        "accepts",
        "them",
    };

    public static void Validate(CliToolDefinition tool)
    {
        foreach (var definition in tool.AllEnums)
        {
            Validate(definition);
        }
    }

    private static void Validate(CliEnumDefinition definition)
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

        ValidateNumericValues(definition);

        var suspiciousValue = definition.Values
            .FirstOrDefault(value => SuspiciousProseValues.Contains(value.CliValue));
        if (suspiciousValue is not null)
        {
            throw new InvalidOperationException(
                $"Enum '{definition.EnumName}' contains suspicious prose value '{suspiciousValue.CliValue}'.");
        }
    }

    private static void ValidateNumericValues(CliEnumDefinition definition)
    {
        var membersByNumericValue = new Dictionary<int, string>();
        int? nextNumericValue = 0;

        foreach (var value in definition.Values)
        {
            var numericValue = value.NumericValue
                ?? nextNumericValue
                ?? throw new InvalidOperationException(
                    $"Enum '{definition.EnumName}' member '{value.MemberName}' has an implicit "
                    + $"numeric value after '{int.MaxValue}'.");
            if (!membersByNumericValue.TryAdd(numericValue, value.MemberName))
            {
                throw new InvalidOperationException(
                    $"Enum '{definition.EnumName}' contains duplicate effective numeric value "
                    + $"'{numericValue}' for members '{membersByNumericValue[numericValue]}' "
                    + $"and '{value.MemberName}'.");
            }

            nextNumericValue = numericValue == int.MaxValue ? null : numericValue + 1;
        }
    }
}
