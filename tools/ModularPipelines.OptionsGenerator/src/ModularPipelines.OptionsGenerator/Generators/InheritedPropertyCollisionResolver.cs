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

    private static readonly HashSet<string> RecordReservedPropertyNames =
    [
        "Clone",
    ];

    public static bool IsInheritedPropertyName(string propertyName) =>
        InheritedPropertyNames.Contains(propertyName);

    public static CliToolDefinition Resolve(CliToolDefinition tool)
    {
        ArgumentNullException.ThrowIfNull(tool);

        var globalNames = tool.GlobalOptions
            .Concat(tool.SupplementalGlobalOptions)
            .Select(option => option.PropertyName)
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
            .Concat(globalPropertyNames)
            .ToHashSet(StringComparer.Ordinal);
        var renamedProperties = new Dictionary<string, string>(StringComparer.Ordinal);
        var options = ResolveOptions(
            command.Options,
            command.CommandParts,
            occupiedNames,
            renamedProperties);
        var usedLocalNames = new HashSet<string>(StringComparer.Ordinal);
        options = ResolveDuplicateOptionNames(options, usedLocalNames);
        var renamedArgumentNames = new Dictionary<string, string>(StringComparer.Ordinal);
        var positionalArguments = command.PositionalArguments
            .Select(argument => argument with
            {
                PropertyName = ResolveName(
                    argument.PropertyName,
                    command.CommandParts,
                    occupiedNames,
                    renamedProperties),
            })
            .Select(argument => !usedLocalNames.Contains(argument.PropertyName)
                ? argument
                : argument with
                {
                    PropertyName = GetOrCreateArgumentName(
                        argument.PropertyName,
                        renamedArgumentNames,
                        usedLocalNames),
                })
            .ToArray();
        var resolvedPropertyNames = command.Options
            .Select((option, index) => (Original: option.PropertyName, Resolved: options[index].PropertyName))
            .Concat(command.PositionalArguments.Select((argument, index) =>
                (Original: argument.PropertyName, Resolved: positionalArguments[index].PropertyName)))
            .GroupBy(static pair => pair.Original, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => group.First().Resolved,
                StringComparer.Ordinal);

        return command with
        {
            Options = options,
            PositionalArguments = positionalArguments,
            RequiredAlternativeGroups = command.RequiredAlternativeGroups
                .Select(group => group with
                {
                    PropertyNames = group.PropertyNames
                        .Select(propertyName => resolvedPropertyNames.GetValueOrDefault(propertyName, propertyName))
                        .ToArray(),
                })
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

    private static IReadOnlyList<CliOptionDefinition> ResolveDuplicateOptionNames(
        IReadOnlyList<CliOptionDefinition> options,
        HashSet<string> usedLocalNames) =>
        options
            .Select(option => usedLocalNames.Add(option.PropertyName)
                ? option
                : option with
                {
                    PropertyName = CreateUniqueLocalName(
                        option.PropertyName,
                        "Option",
                        usedLocalNames),
                })
            .ToArray();

    private static string GetOrCreateArgumentName(
        string propertyName,
        IDictionary<string, string> renamedArgumentNames,
        HashSet<string> usedLocalNames)
    {
        if (renamedArgumentNames.TryGetValue(propertyName, out var renamedPropertyName))
        {
            return renamedPropertyName;
        }

        renamedPropertyName = CreateUniqueLocalName(
            propertyName,
            "Argument",
            usedLocalNames);
        renamedArgumentNames.Add(propertyName, renamedPropertyName);
        return renamedPropertyName;
    }

    private static string CreateUniqueLocalName(
        string propertyName,
        string suffix,
        HashSet<string> usedLocalNames)
    {
        var candidate = propertyName + suffix;
        if (usedLocalNames.Add(candidate))
        {
            return candidate;
        }

        for (var index = 2; ; index++)
        {
            candidate = propertyName + suffix + index;
            if (usedLocalNames.Add(candidate))
            {
                return candidate;
            }
        }
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

        if (!IsInheritedPropertyName(propertyName)
            && !RecordReservedPropertyNames.Contains(propertyName))
        {
            return propertyName;
        }

        for (var index = commandParts.Count - 2; index >= 0; index--)
        {
            var candidate = GeneratorUtils.ToPascalCase(commandParts[index]) + propertyName;
            if (TryOccupyResolvedName(candidate, occupiedNames))
            {
                return RecordRename(propertyName, candidate, renamedProperties);
            }
        }

        var cliCandidate = $"Cli{propertyName}";
        if (TryOccupyResolvedName(cliCandidate, occupiedNames))
        {
            return RecordRename(propertyName, cliCandidate, renamedProperties);
        }

        for (var suffix = 2; ; suffix++)
        {
            var candidate = $"{cliCandidate}{suffix}";
            if (TryOccupyResolvedName(candidate, occupiedNames))
            {
                return RecordRename(propertyName, candidate, renamedProperties);
            }
        }
    }

    private static bool TryOccupyResolvedName(string candidate, HashSet<string> occupiedNames) =>
        !IsInheritedPropertyName(candidate)
        && !RecordReservedPropertyNames.Contains(candidate)
        && occupiedNames.Add(candidate);

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
