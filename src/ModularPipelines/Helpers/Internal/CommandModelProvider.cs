using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
internal sealed class CommandModelProvider : ICommandModelProvider
{
    private readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyCommandLinePart>> _cache = new();

    /// <inheritdoc/>
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Generated command metadata handles statically known options; BuildModel is the documented reflection fallback for dynamic options.")]
    public IReadOnlyList<PropertyCommandLinePart> GetCommandModel(Type optionsType)
    {
        return _cache.GetOrAdd(optionsType, static type =>
        {
            var model = GeneratedCommandMetadata.TryGet(type, out var generatedModel)
                ? generatedModel
                : BuildModel(type);

            ValidateUniqueSwitches(type, model);
            return model;
        });
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "This explicit reflection-only path is used to validate the documented fallback against preserved test assemblies.")]
    internal static IReadOnlyList<PropertyCommandLinePart> GetReflectionCommandModel(Type optionsType)
    {
        var model = BuildModel(optionsType);
        ValidateUniqueSwitches(optionsType, model);
        return model;
    }

    [RequiresUnreferencedCode("Reflection fallback requires CLI-attributed properties. Ensure ModularPipelines.SourceGenerator runs for trim-safe command models.")]
    private static IReadOnlyList<PropertyCommandLinePart> BuildModel(Type type)
    {
        var parts = new List<PropertyCommandLinePart>();
        foreach (var property in GetCommandProperties(type))
        {
            if (property.GetCustomAttribute<CliArgumentAttribute>() is { } arg)
            {
                parts.Add(new ArgumentPart(property.Name, property.GetValue, arg)
                {
                    IsGlobalOption = IsGlobalOption(property),
                });
            }
            else if (property.GetCustomAttribute<CliFlagAttribute>() is { } flag)
            {
                parts.Add(new FlagPart(property.Name, property.GetValue, flag)
                {
                    IsGlobalOption = IsGlobalOption(property),
                });
            }
            else if (property.GetCustomAttribute<CliOptionAttribute>() is { } option)
            {
                parts.Add(new OptionPart(property.Name, property.GetValue, option)
                {
                    IsGlobalOption = IsGlobalOption(property),
                });
            }
        }

        return parts;
    }

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
}
