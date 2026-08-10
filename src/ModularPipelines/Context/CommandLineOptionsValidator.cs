using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
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

        var validationResults = new List<ValidationResult>();
        try
        {
            ValidateProperties(options, metadata.NonPublicProperties, serviceProvider, validationResults);
            ValidateTypeDescriptorMetadata(options, serviceProvider, validationResults);
            if (validationResults.Count == 0)
            {
                ValidateObject(options, serviceProvider, validationResults);
            }
        }
        catch (Exception exception)
        {
            validationResults.Add(new ValidationResult(exception.Message));
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
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Generated command option types retain their public and non-public option properties. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static ValidationMetadata CreateValidationMetadata(Type optionsType)
    {
        var validatedProperties = CommandModelProvider.GetOptionProperties(optionsType)
            .Where(static property => property.GetIndexParameters().Length == 0)
            .Select(static property => new ValidatedProperty(
                property,
                property.GetCustomAttributes<ValidationAttribute>(inherit: true).ToArray()))
            .Where(static property => property.Attributes.Count > 0)
            .ToArray();
        var nonPublicProperties = validatedProperties
            .Where(static property => property.Property.GetMethod is not { IsPublic: true })
            .ToArray();
        return new ValidationMetadata(nonPublicProperties);
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Generated command option types retain their public properties and validation attributes. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static void ValidateTypeDescriptorMetadata(
        object options,
        IServiceProvider serviceProvider,
        ICollection<ValidationResult> validationResults)
    {
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(options))
        {
            var attributes = property.Attributes
                .OfType<ValidationAttribute>()
                .ToArray();
            if (attributes.Length == 0)
            {
                continue;
            }

            var context = new ValidationContext(options, serviceProvider, items: null)
            {
                DisplayName = property.DisplayName,
                MemberName = property.Name,
            };
            ValidateAttributes(
                attributes,
                property.GetValue(options),
                context,
                property.Name,
                validationResults);
        }

        if (validationResults.Count > 0)
        {
            return;
        }

        var objectContext = new ValidationContext(options, serviceProvider, items: null);
        foreach (var attribute in TypeDescriptor.GetAttributes(options)
                     .OfType<ValidationAttribute>())
        {
            var result = attribute.GetValidationResult(options, objectContext);
            if (result != ValidationResult.Success)
            {
                validationResults.Add(result!);
            }
        }
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Generated command option types retain the metadata required by IValidatableObject. Unprocessed reflection fallback assemblies are not trim-safe.")]
    private static void ValidateObject(
        object options,
        IServiceProvider serviceProvider,
        ICollection<ValidationResult> validationResults)
    {
        if (options is not IValidatableObject validatableObject)
        {
            return;
        }

        var context = new ValidationContext(options, serviceProvider, items: null);
        foreach (var result in validatableObject.Validate(context))
        {
            if (result is not null)
            {
                validationResults.Add(result);
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
            ValidateAttributes(
                property.Attributes,
                value,
                context,
                property.Property.Name,
                validationResults);
        }
    }

    private static void ValidateAttributes(
        IReadOnlyList<ValidationAttribute> attributes,
        object? value,
        ValidationContext context,
        string propertyName,
        ICollection<ValidationResult> validationResults)
    {
        var requiredAttribute = attributes.FirstOrDefault(static attribute => attribute is RequiredAttribute);
        if (requiredAttribute is not null
            && !ValidateAttribute(requiredAttribute, value, context, propertyName, validationResults))
        {
            return;
        }

        foreach (var attribute in attributes)
        {
            if (!ReferenceEquals(attribute, requiredAttribute))
            {
                ValidateAttribute(attribute, value, context, propertyName, validationResults);
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
        IReadOnlyList<ValidatedProperty> NonPublicProperties);

    private sealed record ValidatedProperty(
        PropertyInfo Property,
        IReadOnlyList<ValidationAttribute> Attributes);
}
