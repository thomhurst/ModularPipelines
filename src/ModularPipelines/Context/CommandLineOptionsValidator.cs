using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using ModularPipelines.Engine;
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
    public static void Validate(
        CommandLineToolOptions options,
        IServiceProvider serviceProvider,
        ISecretObfuscator secretObfuscator)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(secretObfuscator);

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
            ValidateProperties(options, metadata.NonPublicProperties, serviceProvider, validationResults);
            if (validationResults.Count == 0)
            {
                Validator.TryValidateObject(
                    options,
                    new ValidationContext(options, serviceProvider, items: null),
                    validationResults,
                    validateAllProperties: true);
            }
            else
            {
                ValidateProperties(options, metadata.PublicProperties, serviceProvider, validationResults);
            }
        }
        catch (Exception exception)
        {
            var safeFailureMessage = ObfuscateValidationMessage(
                $"Invalid command-line options: {optionsType.Name}: {exception.Message}",
                options,
                secretObfuscator);
            throw new CommandOptionsValidationException(
                safeFailureMessage,
                new ValidationException(safeFailureMessage));
        }

        if (validationResults.Count == 0)
        {
            return;
        }

        var errors = validationResults
            .Select(result => FormatValidationResult(optionsType, result))
            .Order(StringComparer.Ordinal)
            .ToArray();
        var message = ObfuscateValidationMessage(
            $"Invalid command-line options: {string.Join("; ", errors)}",
            options,
            secretObfuscator);
        throw new CommandOptionsValidationException(
            message,
            new ValidationException(message));
    }

    private static string ObfuscateValidationMessage(
        string message,
        CommandLineToolOptions options,
        ISecretObfuscator secretObfuscator)
    {
        try
        {
            return secretObfuscator.Obfuscate(message, options);
        }
        catch (Exception)
        {
            return $"Invalid command-line options: {options.GetType().Name}: Validation failed.";
        }
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "Generated command option types retain their public and non-public option properties. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static ValidationMetadata CreateValidationMetadata(Type optionsType)
    {
        var validatedProperties = GetValidationProperties(optionsType)
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
        "IL2070",
        Justification = "Generated command option types retain their public and non-public option properties. Unprocessed reflection fallback assemblies are not trim-safe.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "Generated command option type hierarchies retain their public and non-public option properties. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static IEnumerable<PropertyInfo> GetValidationProperties(Type optionsType)
    {
        var seenPropertyNames = new HashSet<string>(StringComparer.Ordinal);
        for (var currentType = optionsType; currentType is not null; currentType = currentType.BaseType)
        {
            foreach (var property in currentType.GetProperties(
                         BindingFlags.Instance
                         | BindingFlags.Public
                         | BindingFlags.NonPublic
                         | BindingFlags.DeclaredOnly))
            {
                if (property.GetMethod is not null && seenPropertyNames.Add(property.Name))
                {
                    yield return property;
                }
            }
        }
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Schema-3 generated metadata preserves non-public properties and their validation attributes. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static void ValidateProperties(
        object options,
        IReadOnlyList<ValidatedProperty> properties,
        IServiceProvider serviceProvider,
        ICollection<ValidationResult> validationResults)
    {
        foreach (var property in properties)
        {
            var context = new ValidationContext(options, serviceProvider, items: null)
            {
                DisplayName = property.Property.Name,
                MemberName = property.Property.Name,
            };
            var value = property.Property.GetValue(options);
            var requiredAttribute = property.Attributes.FirstOrDefault(static attribute => attribute is RequiredAttribute);
            if (requiredAttribute is not null
                && !ValidateAttribute(requiredAttribute, value, context, property.Property.Name, validationResults))
            {
                continue;
            }

            foreach (var attribute in property.Attributes)
            {
                if (ReferenceEquals(attribute, requiredAttribute))
                {
                    continue;
                }

                ValidateAttribute(attribute, value, context, property.Property.Name, validationResults);
            }
        }
    }

    private static bool ValidateAttribute(
        ValidationAttribute attribute,
        object? value,
        ValidationContext context,
        string propertyName,
        ICollection<ValidationResult> validationResults)
    {
        var result = attribute.GetValidationResult(value, context);
        if (result == ValidationResult.Success)
        {
            return true;
        }

        validationResults.Add(result!.MemberNames.Any()
            ? result
            : new ValidationResult(result.ErrorMessage, [propertyName]));
        return false;
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
