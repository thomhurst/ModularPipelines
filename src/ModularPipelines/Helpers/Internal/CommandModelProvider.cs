using System.Collections.Concurrent;
using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
public sealed class CommandModelProvider : ICommandModelProvider
{
    private readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyCommandLinePart>> _cache = new();

    /// <inheritdoc/>
    public IReadOnlyList<PropertyCommandLinePart> GetCommandModel(Type optionsType)
    {
        return _cache.GetOrAdd(optionsType, BuildModel);
    }

    private static IReadOnlyList<PropertyCommandLinePart> BuildModel(Type type)
    {
        var parts = new List<PropertyCommandLinePart>();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<CliArgumentAttribute>() is { } arg)
            {
                parts.Add(new ArgumentPart(property, arg));
            }
            else if (property.GetCustomAttribute<CliFlagAttribute>() is { } flag)
            {
                parts.Add(new FlagPart(property, flag));
            }
            else if (property.GetCustomAttribute<CliOptionAttribute>() is { } option)
            {
                parts.Add(new OptionPart(property, option));
            }
        }

        return parts;
    }
}
