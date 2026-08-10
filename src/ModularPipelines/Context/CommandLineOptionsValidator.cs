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
            ValidateProperties(options, metadata.NonPublicProperties, validationResults);
            if (validationResults.Count == 0)
            {
                Validator.TryValidateObject(
                    options,
                    new ValidationContext(options),
                    validationResults,
                    validateAllProperties: true);
            }
            else
            {
                ValidateProperties(options, metadata.PublicProperties, validationResults);
            }
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
        var validatedProperties = properties
            .Where(static property => property.GetIndexParameters().Length == 0)
            .Select(static property => new ValidatedProperty(
                property,
                property.GetCustomAttributes<ValidationAttribute>(inherit: true).ToArray()))
            .Where(static property => property.Attributes.Count > 0)
            .ToArray();
        var publicProperties = validatedProperties
            .Where(static property => property.Property.GetMethod is { IsPublic: true })
            .ToArray();
        var nonPublicProperties = validatedProperties
            .Where(static property => property.Property.GetMethod is not { IsPublic: true })
            .ToArray();
        var requiresValidation = validatedProperties.Length > 0
                                 || typeof(IValidatableObject).IsAssignableFrom(optionsType)
                                 || Attribute.IsDefined(
                                     optionsType,
                                     typeof(ValidationAttribute),
                                     inherit: true);
        return new ValidationMetadata(requiresValidation, publicProperties, nonPublicProperties);
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Schema-3 generated metadata preserves non-public properties and their validation attributes. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static void ValidateProperties(
        object options,
        IReadOnlyList<ValidatedProperty> properties,
        ICollection<ValidationResult> validationResults)
    {
        foreach (var property in properties)
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
        IReadOnlyList<ValidatedProperty> PublicProperties,
        IReadOnlyList<ValidatedProperty> NonPublicProperties);

    private sealed record ValidatedProperty(
        PropertyInfo Property,
        IReadOnlyList<ValidationAttribute> Attributes);
}
