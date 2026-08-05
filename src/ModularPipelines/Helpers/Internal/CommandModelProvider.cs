using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
internal sealed class CommandModelProvider : ICommandModelProvider
{
    private readonly ConditionalWeakTable<Type, CommandModel> _cache = [];

    /// <inheritdoc/>
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Processed C# assemblies require generated metadata. Unprocessed assemblies use a reflection fallback and are not trim-safe.")]
    public IReadOnlyList<PropertyCommandLinePart> GetCommandModel(Type optionsType)
    {
        return _cache.GetValue(optionsType, static type =>
        {
            if (!GeneratedCommandMetadata.TryGet(type, out var model))
            {
                if (GeneratedCommandMetadata.IsGeneratedMetadataRequired(type.Assembly)
                    || !RuntimeFeature.IsDynamicCodeSupported
                    || (GeneratedCommandMetadata.IsAssemblyProcessed(type.Assembly)
                        && GeneratedCommandMetadata.IsTypeCovered(type)))
                {
                    throw new MissingCommandMetadataException(type);
                }

                model = BuildModel(type);
            }

            ValidateModel(type, model);
            return new CommandModel(model);
        }).Value;
    }

    [RequiresUnreferencedCode("Assemblies without generated command metadata require reflection and are not trim-safe.")]
    private static IReadOnlyList<PropertyCommandLinePart> BuildModel(Type type)
    {
        var parts = new List<PropertyCommandLinePart>();
        foreach (var property in GetCommandProperties(type))
        {
            if (property.GetCustomAttribute<CliArgumentAttribute>() is { } argument)
            {
                parts.Add(new ArgumentPart(property.Name, property.GetValue, argument)
                {
                    IsGlobalOption = IsGlobalOption(property),
                    HasExplicitPosition = property.CustomAttributes.Any(static attribute =>
                        attribute.AttributeType == typeof(CliArgumentAttribute)
                        && attribute.ConstructorArguments.Count > 0),
                });
            }
            else if (property.GetCustomAttribute<CliFlagAttribute>() is { } flag)
            {
                parts.Add(new FlagPart(property.Name, property.GetValue, flag)
                {
                    IsGlobalOption = IsGlobalOption(property),
                    IsSupportedPropertyType = IsSupportedFlagType(property.PropertyType),
                });
            }
            else if (property.GetCustomAttribute<CliOptionAttribute>() is { } option)
            {
                var allowsLegacyOptionalValues = IsLegacyGeneratedOption(property);
                parts.Add(new OptionPart(property.Name, property.GetValue, option)
                {
                    IsGlobalOption = IsGlobalOption(property),
                    ManualOperandCount = GetManualOperandCount(property.PropertyType),
                    AllowsLegacyOptionalValues = allowsLegacyOptionalValues,
                    IsSupportedPropertyType = IsSupportedOptionalValueType(
                        property.PropertyType,
                        allowsLegacyOptionalValues),
                });
            }
        }

        return parts;
    }

    [RequiresUnreferencedCode("Reflection fallback requires option value type metadata.")]
    internal static int GetManualOperandCount(Type propertyType)
    {
        propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        return typeof(ModularPipelines.Models.CliValuePair).IsAssignableFrom(propertyType)
               || propertyType.GetInterfaces()
                   .Append(propertyType)
                   .Any(type => type.IsGenericType
                                && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                                && typeof(ModularPipelines.Models.CliValuePair)
                                    .IsAssignableFrom(type.GetGenericArguments()[0]))
            ? 2
            : 1;
    }

    [RequiresUnreferencedCode("Legacy generated metadata requires option property metadata.")]
    internal static int GetManualOperandCount(Type optionsType, string propertyName) =>
        GetCommandProperties(optionsType)
            .FirstOrDefault(property => property.Name == propertyName) is { } property
            ? GetManualOperandCount(property.PropertyType)
            : 1;

    [RequiresUnreferencedCode("Reflection fallback requires CLI-attributed properties.")]
    private static IEnumerable<PropertyInfo> GetCommandProperties(Type optionsType)
    {
        var seenPropertyNames = new HashSet<string>(StringComparer.Ordinal);
        for (var currentType = optionsType; currentType is not null; currentType = currentType.BaseType)
        {
            foreach (var property in currentType.GetProperties(
                         BindingFlags.Public
                         | BindingFlags.NonPublic
                         | BindingFlags.Instance
                         | BindingFlags.DeclaredOnly))
            {
                if (property.GetMethod is not null && seenPropertyNames.Add(property.Name))
                {
                    yield return property;
                }
            }
        }
    }

    [RequiresUnreferencedCode("Reflection fallback requires CLI-attributed properties.")]
    private static bool IsGlobalOption(PropertyInfo property)
    {
        if (property.DeclaringType?.IsDefined(typeof(CliGlobalOptionsAttribute), inherit: false) == true)
        {
            return true;
        }

        var baseAccessor = property.GetMethod?.GetBaseDefinition();
        for (var currentType = property.DeclaringType?.BaseType; currentType is not null; currentType = currentType.BaseType)
        {
            var declaredProperty = currentType.GetProperty(
                property.Name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (currentType.IsDefined(typeof(CliGlobalOptionsAttribute), inherit: false)
                && declaredProperty?.GetMethod?.GetBaseDefinition().Equals(baseAccessor) == true)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsSupportedFlagType(Type propertyType)
    {
        propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        return propertyType == typeof(bool) || propertyType == typeof(int);
    }

    private static bool IsSupportedOptionalValueType(
        Type propertyType,
        bool allowsLegacyOptionalValues)
    {
        propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        return propertyType == typeof(ModularPipelines.Models.CliOptionValue)
               || propertyType.IsAssignableTo(typeof(IEnumerable<ModularPipelines.Models.CliOptionValue>))
               || (allowsLegacyOptionalValues
                   && (propertyType == typeof(string)
                       || propertyType.IsAssignableTo(typeof(IEnumerable<string>))));
    }

    private static bool IsLegacyGeneratedOption(PropertyInfo property) =>
        property.DeclaringType?.GetCustomAttribute<GeneratedCodeAttribute>(inherit: false)?.Tool
        == "ModularPipelines.OptionsGenerator";

    private static void ValidateModel(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> parts)
    {
        ValidateUniqueSwitches(optionsType, parts);
        ValidateUniqueArgumentPositions(optionsType, parts);

        foreach (var part in parts)
        {
            ValidateProperty(optionsType, part);
        }
    }

    private static void ValidateUniqueArgumentPositions(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> parts)
    {
        var positions = new Dictionary<(bool IsGlobalOption, CommandLinePhase Phase, int Position), string>();
        foreach (var argument in parts.OfType<ArgumentPart>().Where(static argument => argument.HasExplicitPosition))
        {
            var key = (argument.IsGlobalOption, argument.Phase, argument.Attribute.Position);
            if (positions.TryGetValue(key, out var existingProperty))
            {
                throw new InvalidOperationException(
                    $"{optionsType.Name} defines CLI argument position {argument.Attribute.Position} "
                    + $"more than once in phase {argument.Phase} on properties "
                    + $"'{existingProperty}' and '{argument.PropertyName}'.");
            }

            positions.Add(key, argument.PropertyName);
        }
    }

    private static void ValidateProperty(Type optionsType, PropertyCommandLinePart part)
    {
        var propertyName = $"{optionsType.FullName ?? optionsType.Name}.{part.PropertyName}";
        switch (part)
        {
            case FlagPart { IsSupportedPropertyType: false }:
                throw new InvalidOperationException(
                    $"CLI flag property '{propertyName}' must use bool, bool?, int, or int?.");
            case OptionPart { Attribute.GroupValues: true } groupedOption
                when groupedOption.Attribute.Format != OptionFormat.SpaceSeparated:
                throw new InvalidOperationException(
                    $"Grouped CLI option property '{propertyName}' must use OptionFormat.SpaceSeparated.");
            case OptionPart { ManualOperandCount: 2 } valuePairOption
                when valuePairOption.Attribute.Format != OptionFormat.SpaceSeparated:
                throw new InvalidOperationException(
                    $"CliValuePair CLI option property '{propertyName}' must use OptionFormat.SpaceSeparated.");
            case OptionPart
            {
                Attribute.ValueArity: CliOptionValueArity.Optional,
                IsSupportedPropertyType: false,
            }:
                throw new InvalidOperationException(
                    $"Optional-value CLI option property '{propertyName}' must use "
                    + "CliOptionValue or IEnumerable<CliOptionValue>.");
        }
    }

    private static void ValidateUniqueSwitches(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> parts)
    {
        var switches = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var part in parts)
        {
            foreach (var switchName in GetSwitchNames(part))
            {
                if (switches.TryGetValue(switchName, out var existingProperty))
                {
                    throw new InvalidOperationException(
                        $"{optionsType.Name} defines CLI switch '{switchName}' more than once " +
                        $"on properties '{existingProperty}' and '{part.PropertyName}'. " +
                        "Model aliases with ShortForm on a single property.");
                }

                switches.Add(switchName, part.PropertyName);
            }
        }
    }

    private static IEnumerable<string> GetSwitchNames(PropertyCommandLinePart part)
    {
        return part switch
        {
            FlagPart flag => GetNames(flag.Attribute.Name, flag.Attribute.ShortForm),
            OptionPart option => GetNames(option.Attribute.Name, option.Attribute.ShortForm),
            _ => [],
        };
    }

    private static IEnumerable<string> GetNames(string name, string? shortForm)
    {
        yield return name;

        if (!string.IsNullOrWhiteSpace(shortForm) &&
            !string.Equals(name, shortForm, StringComparison.Ordinal))
        {
            yield return shortForm;
        }
    }

    private sealed record CommandModel(IReadOnlyList<PropertyCommandLinePart> Value);
}
