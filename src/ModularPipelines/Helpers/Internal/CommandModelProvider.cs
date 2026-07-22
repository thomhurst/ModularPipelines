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
    [RequiresUnreferencedCode("Calls ModularPipelines.Helpers.Internal.CommandModelProvider.BuildModel(Type)")]
    public IReadOnlyList<PropertyCommandLinePart> GetCommandModel(Type optionsType)
    {
        if (GeneratedCommandMetadata.TryGet(optionsType, out var generatedModel))
        {
            return generatedModel;
        }

        return _cache.GetOrAdd(optionsType, BuildModel);
    }

    [RequiresUnreferencedCode("Reflection fallback requires CLI-attributed properties. Ensure ModularPipelines.SourceGenerator runs for trim-safe command models.")]
    private static IReadOnlyList<PropertyCommandLinePart> BuildModel(Type type)
    {
        var parts = new List<PropertyCommandLinePart>();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        foreach (var property in properties)
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

    private static bool IsGlobalOption(PropertyInfo property) =>
        property.DeclaringType?.IsDefined(typeof(CliGlobalOptionsAttribute), inherit: false) == true;
}
