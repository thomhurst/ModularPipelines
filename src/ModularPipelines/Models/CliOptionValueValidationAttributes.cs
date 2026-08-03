using System.ComponentModel.DataAnnotations;

namespace ModularPipelines.Models;

/// <summary>
/// Validates the explicit values carried by a <see cref="CliOptionValue"/> property.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CliOptionValueRangeAttribute(int minimum, int maximum)
    : RangeAttribute(minimum, maximum)
{
    /// <inheritdoc />
    public override bool IsValid(object? value) =>
        CliOptionValueValidation.IsValid(value, base.IsValid);
}

/// <summary>
/// Validates the explicit values carried by a <see cref="CliOptionValue"/> property.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CliOptionValueRegularExpressionAttribute(string pattern)
    : RegularExpressionAttribute(pattern)
{
    /// <inheritdoc />
    public override bool IsValid(object? value) =>
        CliOptionValueValidation.IsValid(value, base.IsValid);
}

internal static class CliOptionValueValidation
{
    public static bool IsValid(object? value, Func<object?, bool> validate) => value switch
    {
        null => true,
        CliOptionValue optionValue => validate(optionValue.Value),
        IEnumerable<CliOptionValue?> optionValues => optionValues.All(
            optionValue => optionValue is null || validate(optionValue.Value)),
        _ => false,
    };
}
