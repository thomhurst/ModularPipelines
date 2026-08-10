using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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
            CreateValidationMetadata);
        if (!metadata.RequiresValidation)
        {
            return;
        }

        var validationResults = new List<ValidationResult>();
        try
        {
            Validator.TryValidateObject(
                options,
                new ValidationContext(options),
                validationResults,
                validateAllProperties: true);
            ValidateNonPublicProperties(options, metadata, validationResults);
        }
        catch (ValidationException exception)
        {
            throw new CommandOptionsValidationException(
                $"Invalid command-line options: {optionsType.Name}: {exception.Message}",
                exception);
        }

        if (validationResults.Count == 0)
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
        Justification = "Generated command option types retain their public and non-public option properties. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static ValidationMetadata CreateValidationMetadata(Type optionsType)
    {
        var properties = optionsType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var nonPublicProperties = properties
            .Where(static property =>
                property.GetMethod is { IsPublic: false }
                && property.GetIndexParameters().Length == 0)
            .Select(static property => new ValidatedProperty(
                property,
                property.GetCustomAttributes<ValidationAttribute>(inherit: true).ToArray()))
            .Where(static property => property.Attributes.Count > 0)
            .ToArray();
        var requiresValidation = nonPublicProperties.Length > 0
                                 || typeof(IValidatableObject).IsAssignableFrom(optionsType)
                                 || Attribute.IsDefined(
                                     optionsType,
                                     typeof(ValidationAttribute),
                                     inherit: true)
                                 || properties.Any(static property =>
                                     property.GetMethod is { IsPublic: true }
                                     && Attribute.IsDefined(
                                         property,
                                         typeof(ValidationAttribute),
                                         inherit: true));
        return new ValidationMetadata(requiresValidation, nonPublicProperties);
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Schema-3 generated metadata preserves non-public properties and their validation attributes. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static void ValidateNonPublicProperties(
        object options,
        ValidationMetadata metadata,
        ICollection<ValidationResult> validationResults)
    {
        foreach (var property in metadata.NonPublicProperties)
        {
            var context = new ValidationContext(options)
            {
                DisplayName = property.Property.Name,
                MemberName = property.Property.Name,
            };
            var value = property.Property.GetValue(options);
            foreach (var attribute in property.Attributes)
            {
                var result = attribute.GetValidationResult(value, context);
                if (result == ValidationResult.Success)
                {
                    continue;
                }

                validationResults.Add(result!.MemberNames.Any()
                    ? result
                    : new ValidationResult(result.ErrorMessage, [property.Property.Name]));
            }
        }
    }

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

    private sealed record ValidationMetadata(
        bool RequiresValidation,
        IReadOnlyList<ValidatedProperty> NonPublicProperties);

    private sealed record ValidatedProperty(
        PropertyInfo Property,
        IReadOnlyList<ValidationAttribute> Attributes);
}
