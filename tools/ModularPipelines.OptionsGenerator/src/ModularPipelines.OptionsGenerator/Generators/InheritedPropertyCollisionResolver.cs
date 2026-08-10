using ModularPipelines.Options;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

internal static class InheritedPropertyCollisionResolver
{
    private static readonly HashSet<string> InheritedPropertyNames =
        typeof(CommandLineToolOptions)
            .GetProperties()
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);

    public static bool IsInheritedPropertyName(string propertyName) =>
        InheritedPropertyNames.Contains(propertyName);

    public static CliToolDefinition Resolve(CliToolDefinition tool)
    {
        ArgumentNullException.ThrowIfNull(tool);

        var globalNames = tool.GlobalOptions
            .Concat(tool.SupplementalGlobalOptions)
            .Select(option => option.PropertyName)
            .Concat(tool.GlobalCompatibilityProperties.Select(property => property.PropertyName))
            .ToHashSet(StringComparer.Ordinal);
        var globalRenamedProperties = new Dictionary<string, string>(StringComparer.Ordinal);
        var globalOptions = ResolveOptions(
            tool.GlobalOptions,
            [],
            globalNames,
            globalRenamedProperties);
        var supplementalGlobalOptions = ResolveOptions(
            tool.SupplementalGlobalOptions,
            [],
            globalNames,
            globalRenamedProperties);
        var resolvedGlobalNames = globalOptions
            .Concat(supplementalGlobalOptions)
            .Select(option => option.PropertyName)
            .Concat(tool.GlobalCompatibilityProperties.Select(property => property.PropertyName))
            .ToHashSet(StringComparer.Ordinal);

        return tool with
        {
            Commands = tool.Commands
                .Select(command => ResolveCommand(
                    command,
                    globalRenamedProperties,
                    resolvedGlobalNames))
                .ToArray(),
            GlobalOptions = globalOptions,
            SupplementalGlobalOptions = supplementalGlobalOptions,
            GlobalCompatibilityProperties = tool.GlobalCompatibilityProperties
                .Select(property => property.ForwardToPropertyName is { } target
                    && globalRenamedProperties.TryGetValue(target, out var renamedTarget)
                        ? property with { ForwardToPropertyName = renamedTarget }
                        : property)
                .ToArray(),
        };
    }

    private static CliCommandDefinition ResolveCommand(
        CliCommandDefinition command,
        IReadOnlyDictionary<string, string> globalRenamedProperties,
        IReadOnlySet<string> globalPropertyNames)
    {
        var occupiedNames = command.Options
            .Select(option => option.PropertyName)
            .Concat(command.PositionalArguments.Select(argument => argument.PropertyName))
            .Concat(command.CompatibilityProperties.Select(property => property.PropertyName))
            .Concat(globalPropertyNames)
            .ToHashSet(StringComparer.Ordinal);
        var renamedProperties = new Dictionary<string, string>(StringComparer.Ordinal);
        var options = ResolveOptions(
            command.Options,
            command.CommandParts,
            occupiedNames,
            renamedProperties);
        var positionalArguments = command.PositionalArguments
            .Select(argument => argument with
            {
                PropertyName = ResolveName(
                    argument.PropertyName,
                    command.CommandParts,
                    occupiedNames,
                    renamedProperties),
            })
            .ToArray();

        return command with
        {
            Options = options,
            PositionalArguments = positionalArguments,
            CompatibilityProperties = command.CompatibilityProperties
                .Select(property => property.ForwardToPropertyName is { } target
                    && TryGetRename(
                        target,
                        renamedProperties,
                        globalRenamedProperties,
                        out var renamedTarget)
                        ? property with { ForwardToPropertyName = renamedTarget }
                        : property)
                .ToArray(),
            DocumentationExampleValues = command.DocumentationExampleValues
                .ToDictionary(
                    pair => TryGetRename(
                        pair.Key,
                        renamedProperties,
                        globalRenamedProperties,
                        out var renamedProperty)
                            ? renamedProperty
                            : pair.Key,
                    pair => pair.Value,
                    StringComparer.Ordinal),
        };
    }

    private static bool TryGetRename(
        string propertyName,
        IReadOnlyDictionary<string, string> commandRenames,
        IReadOnlyDictionary<string, string> globalRenames,
        out string renamedProperty) =>
        commandRenames.TryGetValue(propertyName, out renamedProperty!)
        || globalRenames.TryGetValue(propertyName, out renamedProperty!);

    private static IReadOnlyList<CliOptionDefinition> ResolveOptions(
        IReadOnlyList<CliOptionDefinition> options,
        IReadOnlyList<string> commandParts,
        HashSet<string> occupiedNames,
        Dictionary<string, string>? renamedProperties) =>
        options
            .Select(option => option with
            {
                PropertyName = ResolveName(
                    option.PropertyName,
                    commandParts,
                    occupiedNames,
                    renamedProperties),
            })
            .ToArray();

    private static string ResolveName(
        string propertyName,
        IReadOnlyList<string> commandParts,
        HashSet<string> occupiedNames,
        Dictionary<string, string>? renamedProperties)
    {
        if (renamedProperties?.TryGetValue(propertyName, out var existingRename) == true)
        {
            return existingRename;
        }

        if (!IsInheritedPropertyName(propertyName))
        {
            return propertyName;
        }

        for (var index = commandParts.Count - 2; index >= 0; index--)
        {
            var candidate = GeneratorUtils.ToPascalCase(commandParts[index]) + propertyName;
            if (occupiedNames.Add(candidate))
            {
                return RecordRename(propertyName, candidate, renamedProperties);
            }
        }

        var cliCandidate = $"Cli{propertyName}";
        if (occupiedNames.Add(cliCandidate))
        {
            return RecordRename(propertyName, cliCandidate, renamedProperties);
        }

        for (var suffix = 2; ; suffix++)
        {
            var candidate = $"{cliCandidate}{suffix}";
            if (occupiedNames.Add(candidate))
            {
                return RecordRename(propertyName, candidate, renamedProperties);
            }
        }
    }

    private static string RecordRename(
        string propertyName,
        string renamedPropertyName,
        Dictionary<string, string>? renamedProperties)
    {
        if (renamedProperties is not null)
        {
            renamedProperties[propertyName] = renamedPropertyName;
        }

        return renamedPropertyName;
    }
}
