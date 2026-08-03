using System.Collections.Concurrent;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
internal sealed class CommandModelProvider : ICommandModelProvider
{
    private readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyCommandLinePart>> _cache = new();

    /// <inheritdoc/>
    public IReadOnlyList<PropertyCommandLinePart> GetCommandModel(Type optionsType)
    {
        return _cache.GetOrAdd(optionsType, static type =>
        {
            if (!GeneratedCommandMetadata.TryGet(type, out var model))
            {
                throw new MissingCommandMetadataException(type);
            }

            ValidateUniqueSwitches(type, model);
            return model;
        });
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
