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
        var reservedNames = globalPropertyNames;
        var usedLocalNames = new HashSet<string>(reservedNames, StringComparer.Ordinal);
        options = ResolveDuplicateOptionNames(
            options,
            reservedNames,
            usedLocalNames,
            renamedProperties);
        var renamedArgumentNames = new Dictionary<string, string>(StringComparer.Ordinal);
        var retainedArgumentNames = new HashSet<string>(StringComparer.Ordinal);
        var positionalArguments = command.PositionalArguments
            .Select(argument => argument with
            {
                PropertyName = ResolveName(
                    argument.PropertyName,
                    command.CommandParts,
                    occupiedNames,
                    renamedProperties),
            })
            .Select(argument => ResolveArgumentName(
                argument,
                renamedArgumentNames,
                retainedArgumentNames,
                usedLocalNames))
            .ToArray();
        var optionAndGlobalPropertyNames = options
            .Select(option => option.PropertyName)
            .Concat(globalPropertyNames)
            .ToHashSet(StringComparer.Ordinal);

        return command with
        {
            Options = options,
            PositionalArguments = positionalArguments,
            DocumentationExampleValues = command.DocumentationExampleValues
                .ToDictionary(
                    pair => ResolveRenamedPropertyName(
                        pair.Key,
                        renamedProperties,
                        globalRenamedProperties,
                        optionAndGlobalPropertyNames,
                        renamedArgumentNames),
                    pair => pair.Value,
                    StringComparer.Ordinal),
        };
    }

    private static IReadOnlyList<CliOptionDefinition> ResolveDuplicateOptionNames(
        IReadOnlyList<CliOptionDefinition> options,
        IReadOnlySet<string> reservedNames,
        HashSet<string> usedLocalNames,
        IDictionary<string, string> renamedProperties) =>
        options
            .Select(option => usedLocalNames.Add(option.PropertyName)
                ? option
                : option with
                {
                    PropertyName = RecordReservedRename(
                        option.PropertyName,
                        CreateUniqueLocalName(
                            option.PropertyName,
                            "Option",
                            usedLocalNames),
                        reservedNames,
                        renamedProperties),
                })
            .ToArray();

    private static CliPositionalArgument ResolveArgumentName(
        CliPositionalArgument argument,
        IDictionary<string, string> renamedArgumentNames,
        ISet<string> retainedArgumentNames,
        HashSet<string> usedLocalNames)
    {
        if (retainedArgumentNames.Contains(argument.PropertyName))
        {
            return argument;
        }

        if (renamedArgumentNames.TryGetValue(argument.PropertyName, out var existingRename))
        {
            return argument with { PropertyName = existingRename };
        }

        if (usedLocalNames.Add(argument.PropertyName))
        {
            retainedArgumentNames.Add(argument.PropertyName);
            return argument;
        }

        var renamedPropertyName = CreateUniqueLocalName(
            argument.PropertyName,
            "Argument",
            usedLocalNames);
        renamedArgumentNames.Add(argument.PropertyName, renamedPropertyName);
        return argument with { PropertyName = renamedPropertyName };
    }

    private static string RecordReservedRename(
        string propertyName,
        string renamedPropertyName,
        IReadOnlySet<string> reservedNames,
        IDictionary<string, string> renamedProperties)
    {
        if (reservedNames.Contains(propertyName))
        {
            renamedProperties[propertyName] = renamedPropertyName;
        }

        return renamedPropertyName;
    }

    private static string ResolveRenamedPropertyName(
        string propertyName,
        IReadOnlyDictionary<string, string> commandRenames,
        IReadOnlyDictionary<string, string> globalRenames,
        IReadOnlySet<string> optionAndGlobalPropertyNames,
        IReadOnlyDictionary<string, string> argumentRenames)
    {
        var resolvedName = TryGetRename(
            propertyName,
            commandRenames,
            globalRenames,
            out var renamedProperty)
                ? renamedProperty
                : propertyName;
        if (optionAndGlobalPropertyNames.Contains(resolvedName))
        {
            return resolvedName;
        }

        return argumentRenames.TryGetValue(resolvedName, out var renamedArgument)
            ? renamedArgument
            : resolvedName;
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
