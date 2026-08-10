using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ModularPipelines.Exceptions;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal static class CommandLineOptionsValidator
{
    private static readonly ConditionalWeakTable<Type, ValidationMetadata> ValidationMetadataCache = [];

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Command options follow the generated-metadata-or-reflection contract used by CommandModelProvider. Unprocessed reflection fallback assemblies are not trim-safe.")]
    public static void Validate(CommandLineToolOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var optionsType = options.GetType();
        var metadata = ValidationMetadataCache.GetValue(
            optionsType,
            static type => new ValidationMetadata(RequiresValidation(type)));
        if (!metadata.RequiresValidation)
        {
            return;
        }

        var validationResults = new List<ValidationResult>();
        if (Validator.TryValidateObject(
                options,
                new ValidationContext(options),
                validationResults,
                validateAllProperties: true))
        {
            return;
        }

        var errors = validationResults
            .Select(result => FormatValidationResult(optionsType, result))
            .Order(StringComparer.Ordinal)
            .ToArray();
        var message = $"Invalid command-line options: {string.Join("; ", errors)}";
        throw new CommandOptionsValidationException(
            message,
            new ValidationException(message));
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "Generated command option types retain their public option properties. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static bool RequiresValidation(Type optionsType) =>
        typeof(IValidatableObject).IsAssignableFrom(optionsType)
        || Attribute.IsDefined(optionsType, typeof(ValidationAttribute), inherit: true)
        || optionsType.GetProperties().Any(property =>
            Attribute.IsDefined(property, typeof(ValidationAttribute), inherit: true));

    private static string FormatValidationResult(Type optionsType, ValidationResult result)
    {
        var memberNames = result.MemberNames
            .Select(memberName => $"{optionsType.Name}.{memberName}")
            .ToArray();
        var target = memberNames.Length == 0
            ? optionsType.Name
            : string.Join(", ", memberNames);
        return $"{target}: {result.ErrorMessage ?? "Validation failed."}";
    }

    private sealed record ValidationMetadata(bool RequiresValidation);
}
