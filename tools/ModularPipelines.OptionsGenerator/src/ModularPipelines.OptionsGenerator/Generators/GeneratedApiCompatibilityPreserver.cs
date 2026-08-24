using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

internal static class GeneratedApiCompatibilityPreserver
{
    private enum RequiredMemberRestoreResult
    {
        NotFound,
        Restored,
        Rejected,
    }

    public static CliToolDefinition Preserve(CliToolDefinition tool, string outputDirectory)
    {
        var optionsDirectory = Path.Combine(
            outputDirectory,
            tool.OutputDirectory,
            "Options");
        if (!Directory.Exists(optionsDirectory))
        {
            return tool;
        }

        var enumBaseline = ReadEnumBaseline(Path.Combine(
                outputDirectory,
                tool.OutputDirectory,
                "Enums"))
            .Where(pair => pair.Key.StartsWith(tool.NamespacePrefix, StringComparison.Ordinal))
            .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal);
        MergeCurrentAliasEnumValues(tool, enumBaseline);
        tool = tool with
        {
            CompatibilityEnums = tool.CompatibilityEnums
                .Concat(enumBaseline.Values)
                .DistinctBy(static definition => definition.EnumName)
                .ToArray(),
        };
        var baseline = ReadBaseline(optionsDirectory);
        var compatibleTool = baseline.TryGetValue($"{tool.NamespacePrefix}Options", out var globalBaseline)
            ? PreserveGlobalOptions(tool, globalBaseline.Properties)
            : tool;
        var facadeMethods = ReadFacadeMethods(
            Path.Combine(outputDirectory, tool.OutputDirectory, "Services"),
            $"{tool.TargetNamespace}.Services",
            tool.NamespacePrefix);
        var executeFacadeOptionTypes = facadeMethods
            .Where(static method => method.MethodName.Equals("ExecuteAsync", StringComparison.Ordinal))
            .Select(static method => method.OptionsType)
            .ToHashSet(StringComparer.Ordinal);
        var namedFacadeOptionTypes = facadeMethods
            .Where(static method => !method.MethodName.Equals("ExecuteAsync", StringComparison.Ordinal))
            .Select(static method => method.OptionsType)
            .ToHashSet(StringComparer.Ordinal);
        var optionalFacadeOptionTypes = facadeMethods
            .Where(static method => method.IsOptionsOptional)
            .Select(static method => method.OptionsType)
            .ToHashSet(StringComparer.Ordinal);
        var commands = compatibleTool.Commands
            .Select(command => PreserveIdentifierCasing(
                compatibleTool,
                command,
                baseline,
                facadeMethods))
            .Concat(RestoreRemovedCommands(compatibleTool, baseline, facadeMethods))
            .DistinctBy(static command => command.ClassName, StringComparer.Ordinal)
            .ToArray();
        var preservedTool = compatibleTool with
        {
            Commands = commands
                .Select(command => baseline.TryGetValue(command.ClassName, out var commandBaseline)
                    ? Preserve(command, commandBaseline.Properties, commandBaseline.Constructors)
                    : command)
                .Select(command => PreserveAliasCompatibility(
                    compatibleTool,
                    command,
                    baseline,
                    enumBaseline))
                .Select(command => executeFacadeOptionTypes.Contains(command.ClassName)
                    ? command with { PreserveExecuteFacade = true }
                    : command)
                .Select(command => namedFacadeOptionTypes.Contains(command.ClassName)
                    ? command with { PreserveNamedFacade = true }
                    : command)
                .Select(command => optionalFacadeOptionTypes.Contains(command.ClassName)
                    ? command with { PreserveOptionalOptionsParameter = true }
                    : command)
                .Select(command => PreserveFacadeMethodCasing(command, facadeMethods))
                .ToArray(),
        };
        RejectRemovedFacadeMethods(preservedTool, facadeMethods);
        return preservedTool;
    }

    private static IEnumerable<CliCommandDefinition> RestoreRemovedCommands(
        CliToolDefinition tool,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline,
        IReadOnlyList<GeneratedFacadeMethod> facadeMethods)
    {
        var currentOptionTypes = tool.Commands
            .Select(static command => command.ClassName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var methods in facadeMethods
                     .Where(method => !currentOptionTypes.Contains(method.OptionsType))
                     .GroupBy(static method => method.OptionsType, StringComparer.Ordinal))
        {
            if (!baseline.TryGetValue(methods.Key, out var commandBaseline)
                || commandBaseline.CommandParts is not { Length: > 0 })
            {
                continue;
            }

            yield return RestoreRemovedCommand(tool, commandBaseline, methods.ToArray());
        }
    }

    private static CliCommandDefinition RestoreRemovedCommand(
        CliToolDefinition tool,
        GeneratedApiBaseline baseline,
        IReadOnlyList<GeneratedFacadeMethod> facadeMethods)
    {
        var commandParts = baseline.CommandParts!;
        var groupIdentifier = GetRestoredCommandGroupIdentifier(tool, baseline, facadeMethods);
        var subDomainGroup = commandParts.Length > 1
            ? GetRestoredSubDomainGroup(tool, commandParts[0], groupIdentifier)
            : null;

        return new CliCommandDefinition
        {
            FullCommand = $"{tool.ToolName} {string.Join(' ', commandParts)}",
            CommandParts = commandParts,
            ClassName = baseline.ClassName,
            ParentClassName = baseline.ParentClassName ?? $"{tool.NamespacePrefix}Options",
            ToolNamespacePrefix = tool.NamespacePrefix,
            Options = RestoreRemovedOptions(baseline.Properties),
            PositionalArguments = RestoreRemovedPositionalArguments(baseline.Properties),
            CompatibilityProperties = RestoreRemovedCompatibilityProperties(baseline.Properties),
            CompatibilityConstructors = baseline.Constructors,
            SubDomainGroup = subDomainGroup,
            CommandGroupIdentifierOverride = commandParts.Length > 1 ? groupIdentifier : null,
            CommandPartIdentifierOverrides = GetRestoredCommandPartIdentifierOverrides(
                tool,
                commandParts,
                groupIdentifier,
                facadeMethods),
            PreserveExecuteFacade = facadeMethods.Any(static method =>
                method.MethodName.Equals("ExecuteAsync", StringComparison.Ordinal)),
            PreserveNamedFacade = facadeMethods.Any(static method =>
                !method.MethodName.Equals("ExecuteAsync", StringComparison.Ordinal)),
            PreserveOptionalOptionsParameter = facadeMethods.Any(static method => method.IsOptionsOptional),
        };
    }

    private static string? GetRestoredSubDomainGroup(
        CliToolDefinition tool,
        string rootCommand,
        string? fallbackIdentifier)
    {
        var currentGroups = tool.Commands
            .Where(command => command.CommandParts.Length > 0
                              && command.CommandParts[0].Equals(
                                  rootCommand,
                                  StringComparison.OrdinalIgnoreCase))
            .Select(command => command.SubDomainGroup)
            .OfType<string>()
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var matchingGroups = currentGroups
            .Where(group => GeneratorUtils.GetSubDomainIdentifier(tool, group)
                .Equals(fallbackIdentifier, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        return matchingGroups.Length == 1 ? matchingGroups[0] : fallbackIdentifier;
    }

    private static string? GetRestoredCommandGroupIdentifier(
        CliToolDefinition tool,
        GeneratedApiBaseline baseline,
        IReadOnlyList<GeneratedFacadeMethod> facadeMethods)
    {
        var rootCommand = baseline.CommandParts?[0];
        if (rootCommand is null)
        {
            return null;
        }

        var defaultIdentifier = GeneratorUtils.ToPascalCase(rootCommand);
        var facadeIdentifiers = facadeMethods
            .Select(method => GetRootIdentifierFromFacade(tool, baseline.CommandParts!, method))
            .Where(static identifier => identifier is not null)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (facadeIdentifiers.Length == 1)
        {
            return facadeIdentifiers[0];
        }

        var currentIdentifiers = tool.Commands
            .Where(command => command.CommandParts.Length > 0
                              && command.CommandParts[0].Equals(
                                  rootCommand,
                                  StringComparison.OrdinalIgnoreCase))
            .Select(command => command.SubDomainGroup is { } group
                ? GeneratorUtils.GetSubDomainIdentifier(tool, group)
                : command.CommandGroupIdentifierOverride ?? defaultIdentifier)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (currentIdentifiers.Length == 1)
        {
            return currentIdentifiers[0];
        }

        if (baseline.ClassName.StartsWith(tool.NamespacePrefix, StringComparison.Ordinal)
            && baseline.ClassName.EndsWith("Options", StringComparison.Ordinal))
        {
            var recoveredIdentifiers = SplitRecoveredIdentifiers(
                baseline.ClassName[tool.NamespacePrefix.Length..^"Options".Length],
                baseline.CommandParts!
                    .Select(GeneratorUtils.ToPascalCase)
                    .ToArray());
            if (recoveredIdentifiers is not null)
            {
                return recoveredIdentifiers[0];
            }
        }

        return defaultIdentifier;
    }

    private static string? GetRootIdentifierFromFacade(
        CliToolDefinition tool,
        IReadOnlyList<string> commandParts,
        GeneratedFacadeMethod facadeMethod)
    {
        var implementationType = GetFacadeImplementationType(tool, facadeMethod);
        var commandTail = commandParts.Skip(1).ToArray();
        var isParentExecuteFacade = IsParentExecuteFacade(implementationType, facadeMethod);
        var facadeCommandParts = isParentExecuteFacade ? commandTail : commandTail.SkipLast(1);
        if (!implementationType.StartsWith(tool.NamespacePrefix, StringComparison.Ordinal))
        {
            return null;
        }

        var defaultIdentifiers = commandParts
            .Take(1)
            .Concat(facadeCommandParts)
            .Select(GeneratorUtils.ToPascalCase)
            .ToArray();
        return SplitRecoveredIdentifiers(
            implementationType[tool.NamespacePrefix.Length..],
            defaultIdentifiers)?[0];
    }

    private static IReadOnlyDictionary<int, string> GetRestoredCommandPartIdentifierOverrides(
        CliToolDefinition tool,
        IReadOnlyList<string> commandParts,
        string? groupIdentifier,
        IReadOnlyList<GeneratedFacadeMethod> facadeMethods)
    {
        if (groupIdentifier is null)
        {
            return new Dictionary<int, string>();
        }

        Dictionary<int, string>? recoveredOverrides = null;
        foreach (var facadeMethod in facadeMethods)
        {
            var candidate = GetRecoveredCommandPartIdentifierOverrides(
                tool,
                commandParts,
                groupIdentifier,
                facadeMethod);
            if (candidate is null)
            {
                continue;
            }

            recoveredOverrides ??= new Dictionary<int, string>();
            foreach (var (partIndex, identifier) in candidate)
            {
                if (recoveredOverrides.TryGetValue(partIndex, out var recoveredIdentifier)
                    && !recoveredIdentifier.Equals(identifier, StringComparison.Ordinal))
                {
                    return new Dictionary<int, string>();
                }

                recoveredOverrides[partIndex] = identifier;
            }
        }

        return recoveredOverrides ?? new Dictionary<int, string>();
    }

    private static Dictionary<int, string>? GetRecoveredCommandPartIdentifierOverrides(
        CliToolDefinition tool,
        IReadOnlyList<string> commandParts,
        string groupIdentifier,
        GeneratedFacadeMethod facadeMethod)
    {
        var implementationType = GetFacadeImplementationType(tool, facadeMethod);
        var isParentExecuteFacade = IsParentExecuteFacade(implementationType, facadeMethod);
        var facadePartCount = commandParts.Count - (isParentExecuteFacade ? 1 : 2);
        if (facadePartCount <= 0)
        {
            return null;
        }

        var identifiers = commandParts
            .Skip(1)
            .Take(facadePartCount)
            .Select(GeneratorUtils.ToPascalCase)
            .ToArray();
        if (!implementationType.StartsWith(tool.NamespacePrefix, StringComparison.Ordinal))
        {
            return null;
        }

        var implementationSuffix = implementationType[tool.NamespacePrefix.Length..];
        if (!implementationSuffix.StartsWith(groupIdentifier, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var recoveredIdentifiers = SplitRecoveredIdentifiers(
            implementationSuffix[groupIdentifier.Length..],
            identifiers);
        if (recoveredIdentifiers is null)
        {
            return null;
        }

        var recoveredOverrides = new Dictionary<int, string>();
        for (var index = 0; index < recoveredIdentifiers.Length; index++)
        {
            recoveredOverrides[index + 1] = recoveredIdentifiers[index];
        }

        return recoveredOverrides;
    }

    private static string[]? SplitRecoveredIdentifiers(
        string recoveredSuffix,
        IReadOnlyList<string> defaultIdentifiers)
    {
        if (recoveredSuffix.Length < defaultIdentifiers.Count)
        {
            return null;
        }

        var candidates = new Dictionary<int, (int Cost, string[] Identifiers, bool IsUnique)>
        {
            [0] = (0, [], true),
        };
        for (var partIndex = 0; partIndex < defaultIdentifiers.Count; partIndex++)
        {
            var nextCandidates = new Dictionary<int, (int Cost, string[] Identifiers, bool IsUnique)>();
            var remainingPartCount = defaultIdentifiers.Count - partIndex - 1;
            foreach (var (offset, candidate) in candidates)
            {
                for (var end = offset + 1; end <= recoveredSuffix.Length - remainingPartCount; end++)
                {
                    var identifier = recoveredSuffix[offset..end];
                    var cost = candidate.Cost
                               + (identifier.Equals(
                                   defaultIdentifiers[partIndex],
                                   StringComparison.OrdinalIgnoreCase)
                                   ? 0
                                   : 1);
                    var identifiers = candidate.Identifiers.Append(identifier).ToArray();
                    if (!nextCandidates.TryGetValue(end, out var current) || cost < current.Cost)
                    {
                        nextCandidates[end] = (cost, identifiers, candidate.IsUnique);
                    }
                    else if (cost == current.Cost)
                    {
                        nextCandidates[end] = (cost, current.Identifiers, false);
                    }
                }
            }

            candidates = nextCandidates;
        }

        return candidates.TryGetValue(recoveredSuffix.Length, out var result) && result.IsUnique
            ? result.Identifiers
            : null;
    }

    private static string GetFacadeImplementationType(
        CliToolDefinition tool,
        GeneratedFacadeMethod facadeMethod) =>
        facadeMethod.DeclaringType.StartsWith($"I{tool.NamespacePrefix}", StringComparison.Ordinal)
            ? facadeMethod.DeclaringType[1..]
            : facadeMethod.DeclaringType;

    private static bool IsParentExecuteFacade(
        string implementationType,
        GeneratedFacadeMethod facadeMethod)
    {
        var optionsImplementationType = facadeMethod.OptionsType.EndsWith("Options", StringComparison.Ordinal)
            ? facadeMethod.OptionsType[..^"Options".Length]
            : facadeMethod.OptionsType;
        return facadeMethod.MethodName.Equals("ExecuteAsync", StringComparison.Ordinal)
               && implementationType.Equals(optionsImplementationType, StringComparison.OrdinalIgnoreCase);
    }

    private static CliOptionDefinition[] RestoreRemovedOptions(
        IEnumerable<GeneratedApiProperty> properties) =>
        properties
            .Where(static property => !property.IsCompatibility && property.SwitchName is not null)
            .Select(static property => new CliOptionDefinition
            {
                SwitchName = property.SwitchName!,
                PropertyName = property.PropertyName,
                CSharpType = property.CSharpType,
                IsRequired = property.IsRequired,
                IsFlag = property.CSharpType is "bool" or "bool?",
            })
            .ToArray();

    private static CliPositionalArgument[] RestoreRemovedPositionalArguments(
        IEnumerable<GeneratedApiProperty> properties) =>
        properties
            .Where(static property => !property.IsCompatibility && property.ArgumentPosition is not null)
            .Select(static property => new CliPositionalArgument
            {
                PropertyName = property.PropertyName,
                CSharpType = property.CSharpType,
                PositionIndex = property.ArgumentPosition!.Value,
                IsRequired = property.IsRequired,
            })
            .ToArray();

    private static CliCompatibilityProperty[] RestoreRemovedCompatibilityProperties(
        IEnumerable<GeneratedApiProperty> properties) =>
        properties
            .Where(static property => property.IsCompatibility)
            .Select(static property => new CliCompatibilityProperty
            {
                PropertyName = property.PropertyName,
                CSharpType = property.CSharpType,
                ForwardToPropertyName = property.ForwardToPropertyName,
                UseInitAccessor = property.UseInitAccessor,
                ForwardingKind = property.ForwardingKind,
                ObsoleteMessage = property.ObsoleteMessage
                    ?? $"{property.PropertyName} is retained for compatibility.",
            })
            .ToArray();

    private static CliCommandDefinition PreserveIdentifierCasing(
        CliToolDefinition tool,
        CliCommandDefinition command,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline,
        IReadOnlyList<GeneratedFacadeMethod> facadeMethods)
    {
        var preserved = command with
        {
            ClassName = FindBaselineIdentifier(command.ClassName, baseline.Keys),
            ParentClassName = FindBaselineIdentifier(command.ParentClassName, baseline.Keys),
        };
        if (!baseline.TryGetValue(preserved.ClassName, out var commandBaseline)
            || commandBaseline.CommandParts is not { Length: > 0 })
        {
            return preserved;
        }

        var commandFacadeMethods = facadeMethods
            .Where(method => method.OptionsType.Equals(preserved.ClassName, StringComparison.Ordinal))
            .ToArray();
        var groupIdentifier = GetRestoredCommandGroupIdentifier(
            tool,
            commandBaseline,
            commandFacadeMethods);
        var recoveredOverrides = GetRestoredCommandPartIdentifierOverrides(
            tool,
            commandBaseline.CommandParts,
            groupIdentifier,
            commandFacadeMethods);
        var mergedOverrides = recoveredOverrides
            .Concat(preserved.CommandPartIdentifierOverrides)
            .DistinctBy(static pair => pair.Key)
            .ToDictionary(static pair => pair.Key, static pair => pair.Value);

        return preserved with
        {
            CommandGroupIdentifierOverride = preserved.CommandGroupIdentifierOverride
                                             ?? (preserved.CommandParts.Length > 1
                                                 ? groupIdentifier
                                                 : null),
            CommandPartIdentifierOverrides = mergedOverrides,
        };
    }

    private static string FindBaselineIdentifier(
        string identifier,
        IEnumerable<string> baselineIdentifiers)
    {
        if (baselineIdentifiers.Contains(identifier, StringComparer.Ordinal))
        {
            return identifier;
        }

        var matches = baselineIdentifiers
            .Where(candidate => candidate.Equals(identifier, StringComparison.OrdinalIgnoreCase))
            .Take(2)
            .ToArray();
        return matches.Length switch
        {
            0 => identifier,
            1 => matches[0],
            _ => throw new InvalidOperationException(
                $"Generated API compatibility validation found ambiguous casing for {identifier}."),
        };
    }

    private static CliCommandDefinition PreserveFacadeMethodCasing(
        CliCommandDefinition command,
        IReadOnlyList<GeneratedFacadeMethod> baselineFacadeMethods)
    {
        var currentMethodName = GeneratorUtils.EnsureAsyncSuffix(
            GeneratorUtils.GenerateMethodNameFromLastCommandPart(command));
        var compatibilityMethods = baselineFacadeMethods
            .Where(method => method.OptionsType.Equals(command.ClassName, StringComparison.Ordinal)
                             && method.MethodName.Equals(currentMethodName, StringComparison.OrdinalIgnoreCase)
                             && !method.MethodName.Equals(currentMethodName, StringComparison.Ordinal))
            .Select(method => new CliCompatibilityMethod
            {
                MethodName = method.MethodName,
                ObsoleteMessage = $"Use {currentMethodName} instead.",
            });

        return command with
        {
            CompatibilityMethods = command.CompatibilityMethods
                .Concat(compatibilityMethods)
                .DistinctBy(static method => method.MethodName, StringComparer.Ordinal)
                .ToArray(),
        };
    }

    private static CliCommandDefinition PreserveAliasCompatibility(
        CliToolDefinition tool,
        CliCommandDefinition command,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline,
        IReadOnlyDictionary<string, CliEnumDefinition> enumBaseline)
    {
        if (command.CommandParts.Length == 0)
        {
            return command;
        }

        var constructorsByAlias = command.AliasCompatibilityConstructors
            .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal);
        var propertiesByAlias = command.AliasCompatibilityProperties
            .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal);
        foreach (var alias in tool.CommandGroupAliases.Where(alias =>
                     command.CommandParts[0].Equals(
                         alias.CanonicalCommand,
                         StringComparison.OrdinalIgnoreCase)))
        {
            var aliasClassName = GeneratorUtils.GetAliasedClassName(tool, alias, command.ClassName);
            if (!baseline.TryGetValue(aliasClassName, out var aliasBaseline))
            {
                continue;
            }

            PreserveAliasConstructors(
                tool,
                command,
                alias,
                aliasClassName,
                aliasBaseline,
                constructorsByAlias);
            PreserveAliasProperties(
                tool,
                command,
                alias,
                aliasClassName,
                aliasBaseline,
                baseline,
                enumBaseline,
                propertiesByAlias);
        }

        return command with
        {
            AliasCompatibilityConstructors = constructorsByAlias,
            AliasCompatibilityProperties = propertiesByAlias,
        };
    }

    private static void PreserveAliasConstructors(
        CliToolDefinition tool,
        CliCommandDefinition command,
        CliCommandGroupAlias alias,
        string aliasClassName,
        GeneratedApiBaseline aliasBaseline,
        Dictionary<string, IReadOnlyList<CliCompatibilityConstructor>> constructorsByAlias)
    {
        var compatibilityConstructors = constructorsByAlias
            .GetValueOrDefault(aliasClassName, [])
            .ToList();
        var currentRequired = GeneratorUtils.GetRequiredConstructorParameters(command)
            .Select(parameter => new GeneratedApiProperty(
                parameter.PropertyName,
                GeneratorUtils.GetAliasedRequiredConstructorParameterType(parameter, tool, alias),
                null,
                null,
                true,
                false,
                null,
                null))
            .ToArray();
        PreserveCompatibilityConstructors(
            aliasBaseline.Properties,
            aliasBaseline.Constructors,
            currentRequired,
            compatibilityConstructors,
            allowNewRequiredMembers: false);
        SetCompatibilityEntries(constructorsByAlias, aliasClassName, compatibilityConstructors);
    }

    private static void PreserveAliasProperties(
        CliToolDefinition tool,
        CliCommandDefinition command,
        CliCommandGroupAlias alias,
        string aliasClassName,
        GeneratedApiBaseline aliasBaseline,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline,
        IReadOnlyDictionary<string, CliEnumDefinition> enumBaseline,
        Dictionary<string, IReadOnlyList<CliAliasCompatibilityProperty>> propertiesByAlias)
    {
        var currentAliasProperties = command.Options
            .Where(static option => option.EnumDefinition is not null
                                    && option.ValueArity != CliOptionValueArity.Optional)
            .Select(option => (
                option.PropertyName,
                option.CSharpType.Replace(
                    option.EnumDefinition!.EnumName,
                    GeneratorUtils.GetAliasedClassName(
                        tool,
                        alias,
                        option.EnumDefinition.EnumName),
                    StringComparison.Ordinal)))
            .ToHashSet();
        var compatibilityProperties = propertiesByAlias
            .GetValueOrDefault(aliasClassName, [])
            .ToList();
        var canonicalProperties = baseline.GetValueOrDefault(command.ClassName)?.Properties ?? [];

        foreach (var baselineProperty in aliasBaseline.Properties)
        {
            var compatibilityProperty = CreateAliasCompatibilityProperty(
                baselineProperty,
                canonicalProperties,
                enumBaseline,
                currentAliasProperties,
                compatibilityProperties);
            if (compatibilityProperty is not null)
            {
                compatibilityProperties.Add(compatibilityProperty);
            }
        }

        SetCompatibilityEntries(propertiesByAlias, aliasClassName, compatibilityProperties);
    }

    private static CliAliasCompatibilityProperty? CreateAliasCompatibilityProperty(
        GeneratedApiProperty baselineProperty,
        IReadOnlyList<GeneratedApiProperty> canonicalProperties,
        IReadOnlyDictionary<string, CliEnumDefinition> enumBaseline,
        IReadOnlySet<(string PropertyName, string CSharpType)> currentAliasProperties,
        IReadOnlyCollection<CliAliasCompatibilityProperty> compatibilityProperties)
    {
        if (currentAliasProperties.Contains((baselineProperty.PropertyName, baselineProperty.CSharpType)))
        {
            return null;
        }

        var canonicalProperty = canonicalProperties.FirstOrDefault(property =>
            property.PropertyName.Equals(baselineProperty.PropertyName, StringComparison.Ordinal));
        if (canonicalProperty is null)
        {
            throw new InvalidOperationException(
                $"Cannot retain alias property {baselineProperty.PropertyName} because the canonical property is missing.");
        }

        EnsureAliasPropertyCanForward(baselineProperty, canonicalProperty, enumBaseline);
        var supplied = compatibilityProperties.FirstOrDefault(existing =>
            existing.PropertyName.Equals(baselineProperty.PropertyName, StringComparison.Ordinal));
        if (supplied is not null)
        {
            EnsureSuppliedAliasContractMatches(baselineProperty, canonicalProperty, supplied);
            return null;
        }

        return new CliAliasCompatibilityProperty
        {
            PropertyName = baselineProperty.PropertyName,
            AliasCSharpType = baselineProperty.CSharpType,
            CanonicalCSharpType = canonicalProperty.CSharpType,
            UseInitAccessor = baselineProperty.UseInitAccessor || canonicalProperty.UseInitAccessor,
            ObsoleteMessage = baselineProperty.ObsoleteMessage
                ?? $"{baselineProperty.PropertyName} is retained for compatibility.",
        };
    }

    private static void EnsureAliasPropertyCanForward(
        GeneratedApiProperty baselineProperty,
        GeneratedApiProperty canonicalProperty,
        IReadOnlyDictionary<string, CliEnumDefinition> enumBaseline)
    {
        if (baselineProperty.CSharpType.Equals(canonicalProperty.CSharpType, StringComparison.Ordinal))
        {
            return;
        }

        var aliasEnumName = GeneratorUtils.GetEnumTypeName(baselineProperty.CSharpType);
        var canonicalEnumName = GeneratorUtils.GetEnumTypeName(canonicalProperty.CSharpType);
        if (!enumBaseline.ContainsKey(aliasEnumName)
            || !enumBaseline.ContainsKey(canonicalEnumName))
        {
            throw new InvalidOperationException(
                $"Cannot retain alias property {baselineProperty.PropertyName} because type "
                + $"{baselineProperty.CSharpType} cannot forward to {canonicalProperty.CSharpType}.");
        }
    }

    private static void EnsureSuppliedAliasContractMatches(
        GeneratedApiProperty baselineProperty,
        GeneratedApiProperty canonicalProperty,
        CliAliasCompatibilityProperty supplied)
    {
        if (!supplied.AliasCSharpType.Equals(baselineProperty.CSharpType, StringComparison.Ordinal)
            || !supplied.CanonicalCSharpType.Equals(canonicalProperty.CSharpType, StringComparison.Ordinal)
            || supplied.UseInitAccessor
            != (baselineProperty.UseInitAccessor || canonicalProperty.UseInitAccessor))
        {
            throw new InvalidOperationException(
                $"Cannot retain alias property {baselineProperty.PropertyName} because its supplied compatibility contract changed.");
        }
    }

    private static void SetCompatibilityEntries<T>(
        Dictionary<string, IReadOnlyList<T>> entriesByAlias,
        string aliasClassName,
        IReadOnlyList<T> entries)
    {
        if (entries.Count == 0)
        {
            entriesByAlias.Remove(aliasClassName);
            return;
        }

        entriesByAlias[aliasClassName] = entries;
    }

    internal static CliToolDefinition PreserveGlobalOptions(
        CliToolDefinition tool,
        string outputDirectory)
    {
        var optionsDirectory = Path.Combine(
            outputDirectory,
            tool.OutputDirectory,
            "Options");
        if (!Directory.Exists(optionsDirectory))
        {
            return tool;
        }

        var globalBaseline = ReadBaseline(
            optionsDirectory,
            $"{tool.NamespacePrefix}Options");
        return globalBaseline is null
            ? tool
            : PreserveGlobalOptions(tool, globalBaseline.Properties);
    }

    internal static CliToolDefinition PreserveGlobalOptions(
        CliToolDefinition tool,
        IReadOnlyList<GeneratedApiProperty> baselineProperties)
    {
        var globalClassName = $"{tool.NamespacePrefix}Options";
        var preserved = Preserve(
            new CliCommandDefinition
            {
                FullCommand = tool.ToolName,
                CommandParts = [],
                ClassName = globalClassName,
                ParentClassName = "CommandLineToolOptions",
                ToolNamespacePrefix = tool.NamespacePrefix,
                Options = tool.GetGlobalOptions(),
                CompatibilityProperties = tool.GlobalCompatibilityProperties,
            },
            baselineProperties);

        return tool with
        {
            GlobalOptions = preserved.Options,
            SupplementalGlobalOptions = [],
            GlobalCompatibilityProperties = preserved.CompatibilityProperties,
        };
    }

    internal static CliCommandDefinition Preserve(
        CliCommandDefinition command,
        IReadOnlyList<GeneratedApiProperty> baselineProperties) =>
        Preserve(command, baselineProperties, []);

    private static CliCommandDefinition Preserve(
        CliCommandDefinition command,
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IReadOnlyList<CliCompatibilityConstructor> baselineConstructors)
    {
        var livePropertyNames = command.Options
            .Select(static option => option.PropertyName)
            .Concat(command.PositionalArguments.Select(static argument => argument.PropertyName))
            .ToHashSet(StringComparer.Ordinal);
        var compatibilityProperties = command.CompatibilityProperties
            .Where(property => !livePropertyNames.Contains(property.PropertyName))
            .ToList();
        var compatibilityConstructors = command.CompatibilityConstructors.ToList();
        var positionalArguments = command.PositionalArguments.ToArray();
        var options = command.Options.ToArray();
        var violations = new List<string>();
        var renamedProperties = new Dictionary<string, string>(StringComparer.Ordinal);
        var preservedTypeChanges = PreserveScalarToCollectionChanges(
            command,
            baselineProperties,
            positionalArguments,
            options,
            compatibilityProperties,
            renamedProperties,
            violations);
        RestoreBaselinePropertyShapes(
            baselineProperties,
            preservedTypeChanges,
            positionalArguments,
            options);

        RestoreRequiredMemberNames(
            baselineProperties,
            positionalArguments,
            options,
            compatibilityProperties,
            renamedProperties,
            violations);
        RestoreRequiredMemberOrder(baselineProperties, positionalArguments, options);

        var currentProperties = GetCurrentProperties(positionalArguments, options);
        foreach (var baseline in baselineProperties)
        {
            if (preservedTypeChanges.Contains(baseline.PropertyName))
            {
                continue;
            }

            PreserveBaselineProperty(
                command,
                baseline,
                currentProperties,
                compatibilityProperties,
                renamedProperties,
                violations);
        }

        RetargetCompatibilityProperties(compatibilityProperties, renamedProperties);

        if (violations.Count > 0)
        {
            throw new InvalidOperationException(
                $"Generated API compatibility validation failed for {command.FullCommand}:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, violations.Select(violation => $"- {violation}")));
        }

        PreserveCompatibilityConstructors(
            baselineProperties,
            baselineConstructors,
            positionalArguments,
            options,
            compatibilityConstructors);

        return command with
        {
            Options = options,
            PositionalArguments = positionalArguments,
            CompatibilityProperties = compatibilityProperties,
            CompatibilityConstructors = compatibilityConstructors,
            DocumentationExampleValues = RenameDocumentationExampleValues(
                command.DocumentationExampleValues,
                renamedProperties),
        };
    }

    private static HashSet<string> PreserveScalarToCollectionChanges(
        CliCommandDefinition command,
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        CliOptionDefinition[] options,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        IDictionary<string, string> renamedProperties,
        ICollection<string> violations)
    {
        var preserved = new HashSet<string>(StringComparer.Ordinal);
        var propertyNames = options.Select(static option => option.PropertyName)
            .Concat(positionalArguments.Select(static argument => argument.PropertyName))
            .Concat(compatibilityProperties.Select(static property => property.PropertyName))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var baseline in baselineProperties.Where(static property =>
                     !property.IsRequired
                     && property.CSharpType.Equals("string?", StringComparison.Ordinal)))
        {
            var optionIndex = Array.FindIndex(options, option =>
                option.PropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal)
                && option.PropertyType.Equals("IEnumerable<string>?", StringComparison.Ordinal)
                && HasSameCliIdentity(ToGeneratedProperty(option), baseline));
            if (optionIndex < 0)
            {
                continue;
            }

            var replacementName = GetUniqueReplacementName(
                $"{baseline.PropertyName}Values",
                propertyNames);
            propertyNames.Add(replacementName);
            options[optionIndex] = options[optionIndex] with { PropertyName = replacementName };
            PreserveCompatibilityProperty(
                command,
                new CliCompatibilityProperty
                {
                    PropertyName = baseline.PropertyName,
                    CSharpType = baseline.CSharpType,
                    ForwardToPropertyName = replacementName,
                    ForwardingKind = CliCompatibilityForwardingKind.ScalarToCollection,
                    ObsoleteMessage = $"Use {replacementName} instead.",
                },
                compatibilityProperties,
                violations);
            renamedProperties[baseline.PropertyName] = replacementName;
            preserved.Add(baseline.PropertyName);
        }

        return preserved;
    }

    private static void RestoreBaselinePropertyShapes(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IReadOnlySet<string> preservedTypeChanges,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options)
    {
        foreach (var baseline in baselineProperties.Where(property =>
                     !property.IsCompatibility
                     && !preservedTypeChanges.Contains(property.PropertyName)))
        {
            var optionIndex = Array.FindIndex(options, option =>
                option.PropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal)
                && HasSameCliIdentity(ToGeneratedProperty(option), baseline));
            if (optionIndex >= 0
                && HasBaselineShapeDrift(ToGeneratedProperty(options[optionIndex]), baseline))
            {
                var isCollection = CliOptionDefinition.TryGetCollectionShape(
                    baseline.CSharpType,
                    out var resolvedCollectionShape)
                    && resolvedCollectionShape;
                var isFlag = baseline.CSharpType.Equals("bool?", StringComparison.Ordinal)
                             || baseline.CSharpType.Equals("bool", StringComparison.Ordinal);
                options[optionIndex] = options[optionIndex] with
                {
                    CSharpType = baseline.CSharpType,
                    IsRequired = baseline.IsRequired,
                    IsFlag = isFlag,
                    ValueArity = CliOptionValueArity.Required,
                    AcceptsMultipleValues = isCollection,
                    IsCollection = isCollection,
                    EnumDefinition = null,
                };
                continue;
            }

            var positionalIndex = Array.FindIndex(positionalArguments, argument =>
                argument.PropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal)
                && HasSameCliIdentity(ToGeneratedProperty(argument), baseline));
            if (positionalIndex >= 0
                && HasBaselineShapeDrift(ToGeneratedProperty(positionalArguments[positionalIndex]), baseline))
            {
                positionalArguments[positionalIndex] = positionalArguments[positionalIndex] with
                {
                    CSharpType = baseline.CSharpType,
                    IsRequired = baseline.IsRequired,
                };
            }
        }
    }

    private static bool HasBaselineShapeDrift(
        GeneratedApiProperty current,
        GeneratedApiProperty baseline) =>
        current.IsRequired != baseline.IsRequired
        || !current.CSharpType.Equals(baseline.CSharpType, StringComparison.Ordinal);

    private static string GetUniqueReplacementName(
        string candidate,
        IReadOnlySet<string> propertyNames)
    {
        while (propertyNames.Contains(candidate))
        {
            candidate += "Values";
        }

        return candidate;
    }

    private static void PreserveBaselineProperty(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        IReadOnlyList<GeneratedApiProperty> currentProperties,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        IDictionary<string, string> renamedProperties,
        List<string> violations)
    {
        if (TryValidateSameNameProperty(
                command,
                baseline,
                currentProperties,
                violations))
        {
            return;
        }

        if (baseline.IsCompatibility)
        {
            PreserveCompatibilityProperty(
                command,
                baseline,
                compatibilityProperties,
                violations);
            return;
        }

        var replacement = currentProperties.FirstOrDefault(property =>
            HasSameCliIdentity(property, baseline));
        var forwardingKind = GetRenamedPropertyForwardingKind(baseline, replacement);
        if (TryRecordRemovedPropertyViolation(
                command,
                baseline,
                replacement,
                forwardingKind,
                violations))
        {
            return;
        }

        PreserveCompatibilityProperty(
            command,
            new CliCompatibilityProperty
            {
                PropertyName = baseline.PropertyName,
                CSharpType = baseline.CSharpType,
                ForwardToPropertyName = replacement?.PropertyName,
                UseInitAccessor = baseline.UseInitAccessor || replacement?.UseInitAccessor == true,
                ForwardingKind = forwardingKind ?? CliCompatibilityForwardingKind.Direct,
                ObsoleteMessage = replacement is null
                    ? $"{baseline.PropertyName} is no longer supported by the installed CLI and has no effect."
                    : $"Use {replacement.PropertyName} instead.",
            },
            compatibilityProperties,
            violations);
        if (replacement is not null)
        {
            renamedProperties[baseline.PropertyName] = replacement.PropertyName;
        }
    }

    private static bool TryValidateSameNameProperty(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        IReadOnlyList<GeneratedApiProperty> currentProperties,
        List<string> violations)
    {
        var candidates = currentProperties
            .Where(property => property.PropertyName.Equals(
                baseline.PropertyName,
                StringComparison.Ordinal))
            .ToArray();
        var match = candidates.FirstOrDefault(property => HasSameCliIdentity(property, baseline))
                    ?? candidates.FirstOrDefault();
        if (match is null)
        {
            return false;
        }

        if (baseline is { IsCompatibility: true, ForwardToPropertyName: not null })
        {
            ValidateMatchingPropertyShape(command, baseline, match, violations);
        }
        else
        {
            ValidateMatchingProperty(command, baseline, match, violations);
        }

        return true;
    }

    private static void RetargetCompatibilityProperties(
        IList<CliCompatibilityProperty> compatibilityProperties,
        IReadOnlyDictionary<string, string> renamedProperties)
    {
        for (var index = 0; index < compatibilityProperties.Count; index++)
        {
            var property = compatibilityProperties[index];
            if (property.ForwardToPropertyName is { } target
                && renamedProperties.TryGetValue(target, out var replacement))
            {
                compatibilityProperties[index] = property with
                {
                    ForwardToPropertyName = replacement,
                };
            }
        }
    }

    private static bool TryRecordRemovedPropertyViolation(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        GeneratedApiProperty? replacement,
        CliCompatibilityForwardingKind? forwardingKind,
        ICollection<string> violations)
    {
        if (replacement is not null
            && forwardingKind is null
            && !replacement.CSharpType.Equals(baseline.CSharpType, StringComparison.Ordinal))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed type from "
                + $"{baseline.CSharpType} to {replacement.CSharpType} "
                + $"while being renamed to {replacement.PropertyName}");
            return true;
        }

        if (baseline.ArgumentPosition is not null
            && baseline.IsRequired
            && replacement is null)
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} positional argument was removed");
            return true;
        }

        if (baseline.IsRequired && replacement is null)
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} was removed from the required constructor");
            return true;
        }

        return false;
    }

    private static CliCompatibilityForwardingKind? GetRenamedPropertyForwardingKind(
        GeneratedApiProperty baseline,
        GeneratedApiProperty? replacement)
    {
        if (replacement is null
            || replacement.CSharpType.Equals(baseline.CSharpType, StringComparison.Ordinal))
        {
            return CliCompatibilityForwardingKind.Direct;
        }

        return baseline.CSharpType.Equals("string?", StringComparison.Ordinal)
               && replacement.CSharpType.Equals("string", StringComparison.Ordinal)
            ? CliCompatibilityForwardingKind.NullableStringToRequiredString
            : null;
    }

    private static void PreserveCompatibilityProperty(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        ICollection<string> violations) =>
        PreserveCompatibilityProperty(
            command,
            new CliCompatibilityProperty
            {
                PropertyName = baseline.PropertyName,
                CSharpType = baseline.CSharpType,
                ForwardToPropertyName = baseline.ForwardToPropertyName,
                UseInitAccessor = baseline.UseInitAccessor,
                ForwardingKind = baseline.ForwardingKind,
                ObsoleteMessage = baseline.ObsoleteMessage
                    ?? $"{baseline.PropertyName} is retained for compatibility.",
            },
            compatibilityProperties,
            violations);

    private static void PreserveCompatibilityProperty(
        CliCommandDefinition command,
        CliCompatibilityProperty expected,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        ICollection<string> violations)
    {
        var supplied = compatibilityProperties.FirstOrDefault(property =>
            property.PropertyName.Equals(expected.PropertyName, StringComparison.Ordinal));
        if (supplied is null)
        {
            compatibilityProperties.Add(expected);
            return;
        }

        if (!supplied.CSharpType.Equals(expected.CSharpType, StringComparison.Ordinal))
        {
            violations.Add(
                $"{command.ClassName}.{expected.PropertyName} compatibility property changed type from "
                + $"{expected.CSharpType} to {supplied.CSharpType}");
        }
        else if (!string.Equals(
                     supplied.ForwardToPropertyName,
                     expected.ForwardToPropertyName,
                     StringComparison.Ordinal))
        {
            violations.Add(
                $"{command.ClassName}.{expected.PropertyName} compatibility property changed forwarding target from "
                + $"{expected.ForwardToPropertyName ?? "<none>"} to {supplied.ForwardToPropertyName ?? "<none>"}");
        }
        else if (supplied.UseInitAccessor != expected.UseInitAccessor)
        {
            violations.Add(
                $"{command.ClassName}.{expected.PropertyName} compatibility property changed accessor from "
                + $"{(expected.UseInitAccessor ? "init" : "set")} to {(supplied.UseInitAccessor ? "init" : "set")}");
        }
        else if (supplied.ForwardingKind != expected.ForwardingKind)
        {
            violations.Add(
                $"{command.ClassName}.{expected.PropertyName} compatibility property changed forwarding conversion from "
                + $"{expected.ForwardingKind} to {supplied.ForwardingKind}");
        }
    }

    private static void ValidateMatchingProperty(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        GeneratedApiProperty current,
        List<string> violations)
    {
        if (!ValidateMatchingPropertyShape(command, baseline, current, violations))
        {
            return;
        }

        if (!HasSameCliIdentity(current, baseline))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed CLI switch or argument position");
        }
    }

    private static bool ValidateMatchingPropertyShape(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        GeneratedApiProperty current,
        ICollection<string> violations)
    {
        if (!current.CSharpType.Equals(baseline.CSharpType, StringComparison.Ordinal))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed type from "
                + $"{baseline.CSharpType} to {current.CSharpType}");
            return false;
        }

        if (baseline.IsRequired && !current.IsRequired)
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed from required to optional");
            return false;
        }

        if (!baseline.IsRequired && current.IsRequired)
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed from optional to required "
                + "and would remove its public setter");
            return false;
        }

        return true;
    }

    private static void RestoreRequiredMemberNames(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        IDictionary<string, string> renamedProperties,
        ICollection<string> violations)
    {
        foreach (var baseline in baselineProperties.Where(property =>
                     property.IsRequired && !property.IsCompatibility))
        {
            var propertyNames = positionalArguments.Select(argument => argument.PropertyName)
                .Concat(options.Select(option => option.PropertyName));
            var positionalResult = TryRestoreRequiredMember(
                baseline,
                positionalArguments,
                propertyNames,
                ToGeneratedProperty,
                static argument => argument.CSharpType,
                argument => argument with
                {
                    PropertyName = baseline.PropertyName,
                    CSharpType = baseline.CSharpType,
                    IsRequired = true,
                },
                violations,
                out var currentName);
            if (positionalResult != RequiredMemberRestoreResult.NotFound)
            {
                if (positionalResult == RequiredMemberRestoreResult.Restored)
                {
                    RecordRequiredMemberRename(
                        baselineProperties,
                        compatibilityProperties,
                        renamedProperties,
                        currentName,
                        baseline);
                }

                continue;
            }

            var optionResult = TryRestoreRequiredMember(
                baseline,
                options,
                propertyNames,
                ToGeneratedProperty,
                static option => option.PropertyType,
                option => option with
                {
                    PropertyName = baseline.PropertyName,
                    CSharpType = baseline.CSharpType,
                    IsRequired = true,
                },
                violations,
                out currentName);
            if (optionResult == RequiredMemberRestoreResult.Restored)
            {
                RecordRequiredMemberRename(
                    baselineProperties,
                    compatibilityProperties,
                    renamedProperties,
                    currentName,
                    baseline);
            }
        }
    }

    private static RequiredMemberRestoreResult TryRestoreRequiredMember<T>(
        GeneratedApiProperty baseline,
        T[] members,
        IEnumerable<string> propertyNames,
        Func<T, GeneratedApiProperty> toGeneratedProperty,
        Func<T, string> getEmittedType,
        Func<T, T> restore,
        ICollection<string> violations,
        out string currentName)
    {
        var index = Array.FindIndex(members, current =>
            HasSameCliIdentity(toGeneratedProperty(current), baseline)
            && getEmittedType(current).TrimEnd('?').Equals(
                baseline.CSharpType.TrimEnd('?'),
                StringComparison.Ordinal));
        if (index < 0)
        {
            currentName = string.Empty;
            return RequiredMemberRestoreResult.NotFound;
        }

        currentName = toGeneratedProperty(members[index]).PropertyName;
        if (!CanRestoreName(
                baseline.PropertyName,
                currentName,
                propertyNames,
                violations))
        {
            return RequiredMemberRestoreResult.Rejected;
        }

        members[index] = restore(members[index]);
        return RequiredMemberRestoreResult.Restored;
    }

    private static bool CanRestoreName(
        string baselineName,
        string currentName,
        IEnumerable<string> propertyNames,
        ICollection<string> violations)
    {
        if (baselineName.Equals(currentName, StringComparison.Ordinal))
        {
            return true;
        }

        if (!propertyNames.Any(name => name.Equals(baselineName, StringComparison.Ordinal)))
        {
            return true;
        }

        violations.Add($"restoring required member {currentName} to {baselineName} would duplicate a member name");
        return false;
    }

    private static void RecordRequiredMemberRename(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        IDictionary<string, string> renamedProperties,
        string currentName,
        GeneratedApiProperty baseline)
    {
        if (currentName.Equals(baseline.PropertyName, StringComparison.Ordinal))
        {
            return;
        }

        renamedProperties[currentName] = baseline.PropertyName;
        if (baselineProperties.Any(property =>
                property.PropertyName.Equals(currentName, StringComparison.Ordinal)))
        {
            return;
        }

        AddRenamedCurrentProperty(
            compatibilityProperties,
            currentName,
            baseline.CSharpType,
            baseline.PropertyName);
    }

    private static void AddRenamedCurrentProperty(
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        string propertyName,
        string cSharpType,
        string forwardToPropertyName) =>
        AddCompatibilityProperty(
            compatibilityProperties,
            new CliCompatibilityProperty
            {
                PropertyName = propertyName,
                CSharpType = cSharpType,
                ForwardToPropertyName = forwardToPropertyName,
                UseInitAccessor = true,
                ObsoleteMessage = $"Use {forwardToPropertyName} instead.",
            });

    private static void RestoreRequiredMemberOrder(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options)
    {
        var baselineOrder = baselineProperties
            .Where(static property => property.IsRequired && !property.IsCompatibility)
            .Select((property, index) => (property.PropertyName, index))
            .ToDictionary(pair => pair.PropertyName, pair => pair.index, StringComparer.Ordinal);
        RestoreRequiredMemberOrder(
            positionalArguments,
            static argument => argument.IsRequired,
            static argument => argument.PropertyName,
            baselineOrder);
        RestoreRequiredMemberOrder(
            options,
            static option => option.IsRequired,
            static option => option.PropertyName,
            baselineOrder);
    }

    private static void RestoreRequiredMemberOrder<T>(
        T[] members,
        Func<T, bool> isRequired,
        Func<T, string> getPropertyName,
        IReadOnlyDictionary<string, int> baselineOrder)
    {
        var orderedRequired = members
            .Where(isRequired)
            .OrderBy(member => baselineOrder.GetValueOrDefault(getPropertyName(member), int.MaxValue))
            .ToArray();
        var requiredIndex = 0;
        for (var index = 0; index < members.Length; index++)
        {
            if (isRequired(members[index]))
            {
                members[index] = orderedRequired[requiredIndex++];
            }
        }
    }

    private static void AddCompatibilityProperty(
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        CliCompatibilityProperty property)
    {
        if (compatibilityProperties.Any(existing =>
                existing.PropertyName.Equals(property.PropertyName, StringComparison.Ordinal)))
        {
            return;
        }

        compatibilityProperties.Add(property);
    }

    private static void PreserveCompatibilityConstructors(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IReadOnlyList<CliCompatibilityConstructor> baselineConstructors,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        IReadOnlyList<CliOptionDefinition> options,
        List<CliCompatibilityConstructor> compatibilityConstructors) =>
        PreserveCompatibilityConstructors(
            baselineProperties,
            baselineConstructors,
            GetCurrentProperties(positionalArguments, options)
                .Where(static property => property.IsRequired)
                .ToArray(),
            compatibilityConstructors,
            allowNewRequiredMembers: true);

    private static void PreserveCompatibilityConstructors(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IReadOnlyList<CliCompatibilityConstructor> baselineConstructors,
        IReadOnlyList<GeneratedApiProperty> currentRequired,
        List<CliCompatibilityConstructor> compatibilityConstructors,
        bool allowNewRequiredMembers)
    {
        if (currentRequired.Count == 0)
        {
            compatibilityConstructors.Clear();
            return;
        }

        var baselineRequired = baselineProperties
            .Where(static property => property.IsRequired && !property.IsCompatibility)
            .ToArray();
        var addedRequired = currentRequired
            .Where(current => !baselineRequired.Any(baseline =>
                baseline.PropertyName.Equals(current.PropertyName, StringComparison.Ordinal)
                && baseline.CSharpType.Equals(current.CSharpType, StringComparison.Ordinal)))
            .Select(static property => property.PropertyName)
            .ToArray();
        if (addedRequired.Length > 0
            && (!allowNewRequiredMembers || baselineRequired.Length > 0))
        {
            throw new InvalidOperationException(
                "Cannot retain generated constructors because newly required member(s) "
                + $"{string.Join(", ", addedRequired)} have no baseline value.");
        }

        foreach (var constructor in baselineConstructors)
        {
            AddCompatibilityConstructor(compatibilityConstructors, constructor, currentRequired);
        }

        if (HasSameConstructorContract(baselineRequired, currentRequired))
        {
            return;
        }

        var baselineParameters = baselineRequired
            .Select(property => new CliCompatibilityConstructorParameter(
                property.PropertyName,
                property.CSharpType))
            .ToArray();
        var primaryArguments = currentRequired
            .Select(current => baselineRequired.Any(baseline =>
                baseline.PropertyName.Equals(current.PropertyName, StringComparison.Ordinal)
                && baseline.CSharpType.Equals(current.CSharpType, StringComparison.Ordinal))
                    ? current.PropertyName
                    : GetTypedDefault(current.CSharpType))
            .ToArray();
        AddCompatibilityConstructor(
            compatibilityConstructors,
            new CliCompatibilityConstructor
            {
                Parameters = baselineParameters,
                PrimaryConstructorArguments = primaryArguments,
                PreserveDeconstruct = baselineParameters.Length > 0,
            },
            currentRequired);
    }

    private static void AddCompatibilityConstructor(
        ICollection<CliCompatibilityConstructor> constructors,
        CliCompatibilityConstructor constructor,
        IReadOnlyList<GeneratedApiProperty> currentRequired)
    {
        if (HasSameConstructorSignature(constructor.Parameters, currentRequired))
        {
            return;
        }

        constructor = constructor with
        {
            PrimaryConstructorArguments = constructor.PrimaryConstructorArguments
                .Select((argument, index) => argument.Equals("default!", StringComparison.Ordinal)
                                             && index < currentRequired.Count
                    ? GetTypedDefault(currentRequired[index].CSharpType)
                    : argument)
                .ToArray(),
        };

        var existing = constructors.FirstOrDefault(candidate => HasSameConstructorSignature(
            candidate.Parameters,
            constructor.Parameters));
        if (existing is not null)
        {
            if (!HasSameConstructorContract(existing.Parameters, constructor.Parameters)
                || !existing.PrimaryConstructorArguments.SequenceEqual(
                    constructor.PrimaryConstructorArguments,
                    StringComparer.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Compatibility constructor ({string.Join(", ", constructor.Parameters.Select(GetConstructorParameterType))}) "
                    + "conflicts with the generated baseline contract.");
            }

            if (constructor.PreserveDeconstruct && !existing.PreserveDeconstruct)
            {
                constructors.Remove(existing);
                constructors.Add(existing with { PreserveDeconstruct = true });
            }

            return;
        }

        constructors.Add(constructor);
    }

    private static string GetTypedDefault(string cSharpType) => $"default({cSharpType})!";

    private static bool HasSameConstructorContract<TLeft, TRight>(
        IReadOnlyList<TLeft> left,
        IReadOnlyList<TRight> right)
        where TLeft : notnull
        where TRight : notnull
    {
        if (left.Count != right.Count)
        {
            return false;
        }

        return left.Select(GetConstructorParameterContract)
            .SequenceEqual(right.Select(GetConstructorParameterContract));
    }

    private static bool HasSameConstructorSignature<TLeft, TRight>(
        IReadOnlyList<TLeft> left,
        IReadOnlyList<TRight> right)
        where TLeft : notnull
        where TRight : notnull
    {
        if (left.Count != right.Count)
        {
            return false;
        }

        return left.Select(GetConstructorParameterSignatureType)
            .SequenceEqual(right.Select(GetConstructorParameterSignatureType), StringComparer.Ordinal);
    }

    private static string GetConstructorParameterSignatureType<T>(T parameter) =>
        GetConstructorParameterType(parameter).TrimEnd('?');

    private static string GetConstructorParameterType<T>(T parameter) => parameter switch
    {
        GeneratedApiProperty property => property.CSharpType,
        CliCompatibilityConstructorParameter compatibilityParameter => compatibilityParameter.CSharpType,
        _ => throw new ArgumentOutOfRangeException(nameof(parameter)),
    };

    private static (string PropertyName, string CSharpType) GetConstructorParameterContract<T>(T parameter) =>
        parameter switch
        {
            GeneratedApiProperty property => (property.PropertyName, property.CSharpType),
            CliCompatibilityConstructorParameter compatibilityParameter =>
                (compatibilityParameter.PropertyName, compatibilityParameter.CSharpType),
            _ => throw new ArgumentOutOfRangeException(nameof(parameter)),
        };

    private static IReadOnlyDictionary<string, string> RenameDocumentationExampleValues(
        IReadOnlyDictionary<string, string> values,
        Dictionary<string, string> renamedProperties)
    {
        if (renamedProperties.Count == 0)
        {
            return values;
        }

        return values.ToDictionary(
            pair => renamedProperties.GetValueOrDefault(pair.Key, pair.Key),
            pair => pair.Value,
            StringComparer.Ordinal);
    }

    private static GeneratedApiProperty[] GetCurrentProperties(
        IEnumerable<CliPositionalArgument> positionalArguments,
        IEnumerable<CliOptionDefinition> options) =>
        options.Select(ToGeneratedProperty)
            .Concat(positionalArguments.Select(ToGeneratedProperty))
            .ToArray();

    private static GeneratedApiProperty ToGeneratedProperty(CliPositionalArgument argument) =>
        new(
            argument.PropertyName,
            argument.IsRequired ? argument.CSharpType.TrimEnd('?') : argument.CSharpType,
            null,
            argument.PositionIndex,
            argument.IsRequired,
            false,
            null,
            null,
            argument.IsRequired);

    private static GeneratedApiProperty ToGeneratedProperty(CliOptionDefinition option) =>
        new(
            option.PropertyName,
            option.IsRequired ? option.PropertyType.TrimEnd('?') : option.PropertyType,
            option.SwitchName,
            null,
            option.IsRequired,
            false,
            null,
            null,
            option.IsRequired);

    private static bool HasSameCliIdentity(
        GeneratedApiProperty left,
        GeneratedApiProperty right)
    {
        if (left.ArgumentPosition is not null || right.ArgumentPosition is not null)
        {
            return left.ArgumentPosition == right.ArgumentPosition;
        }

        if (left.SwitchName is not null || right.SwitchName is not null)
        {
            return left.SwitchName?.Equals(right.SwitchName, StringComparison.Ordinal) == true;
        }

        return true;
    }

    private static Dictionary<string, GeneratedApiBaseline> ReadBaseline(
        string optionsDirectory)
    {
        var baseline = new Dictionary<string, GeneratedApiBaseline>(StringComparer.Ordinal);
        foreach (var path in Directory.EnumerateFiles(
                     optionsDirectory,
                     "*.Generated.cs",
                     SearchOption.TopDirectoryOnly))
        {
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
            foreach (var declaration in root.DescendantNodes().OfType<RecordDeclarationSyntax>())
            {
                baseline[declaration.Identifier.ValueText] = ReadBaseline(declaration);
            }
        }

        return baseline;
    }

    private static GeneratedApiBaseline? ReadBaseline(
        string optionsDirectory,
        string className)
    {
        var path = Path.Combine(optionsDirectory, $"{className}.Generated.cs");
        if (!File.Exists(path))
        {
            return null;
        }

        var declaration = CSharpSyntaxTree.ParseText(File.ReadAllText(path))
            .GetRoot()
            .DescendantNodes()
            .OfType<RecordDeclarationSyntax>()
            .FirstOrDefault(record => record.Identifier.ValueText.Equals(
                className,
                StringComparison.Ordinal));
        return declaration is null
            ? null
            : ReadBaseline(declaration);
    }

    private static GeneratedApiBaseline ReadBaseline(RecordDeclarationSyntax declaration)
    {
        var attributes = declaration.AttributeLists.SelectMany(static list => list.Attributes);
        var subCommand = FindAttribute(attributes, "CliSubCommand");
        var commandParts = subCommand?.ArgumentList?.Arguments
            .Select(static argument => argument.Expression)
            .OfType<LiteralExpressionSyntax>()
            .Where(static literal => literal.IsKind(SyntaxKind.StringLiteralExpression))
            .Select(static literal => literal.Token.ValueText)
            .ToArray();
        var parentClassName = declaration.BaseList?.Types.FirstOrDefault()?.Type.ToString();
        return new GeneratedApiBaseline(
            declaration.Identifier.ValueText,
            commandParts,
            parentClassName,
            ReadProperties(declaration),
            ReadCompatibilityConstructors(declaration));
    }

    private static Dictionary<string, CliEnumDefinition> ReadEnumBaseline(string enumsDirectory)
    {
        var baseline = new Dictionary<string, CliEnumDefinition>(StringComparer.Ordinal);
        if (!Directory.Exists(enumsDirectory))
        {
            return baseline;
        }

        foreach (var path in Directory.EnumerateFiles(
                     enumsDirectory,
                     "*.Generated.cs",
                     SearchOption.TopDirectoryOnly))
        {
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
            foreach (var declaration in root.DescendantNodes().OfType<EnumDeclarationSyntax>())
            {
                var nextNumericValue = 0;
                var values = new List<CliEnumValue>(declaration.Members.Count);
                foreach (var member in declaration.Members)
                {
                    var attributes = member.AttributeLists.SelectMany(static list => list.Attributes);
                    var cliValueAttribute = FindAttribute(attributes, "EnumValue")
                                            ?? FindAttribute(attributes, "Description");
                    var numericValue = GetEnumNumericValue(member.EqualsValue?.Value) ?? nextNumericValue;
                    values.Add(new CliEnumValue
                    {
                        MemberName = member.Identifier.ValueText,
                        CliValue = GetStringArgument(cliValueAttribute) ?? member.Identifier.ValueText,
                        NumericValue = numericValue,
                    });
                    nextNumericValue = checked(numericValue + 1);
                }

                baseline[declaration.Identifier.ValueText] = new CliEnumDefinition
                {
                    EnumName = declaration.Identifier.ValueText,
                    Values = values,
                };
            }
        }

        return baseline;
    }

    private static void MergeCurrentAliasEnumValues(
        CliToolDefinition tool,
        IDictionary<string, CliEnumDefinition> enumBaseline)
    {
        var currentAliasEnums = tool.CommandGroupAliases
            .SelectMany(alias => tool.Commands
                .Where(command => command.CommandParts.Length > 0
                                  && command.CommandParts[0].Equals(
                                      alias.CanonicalCommand,
                                      StringComparison.OrdinalIgnoreCase))
                .SelectMany(command => command.Options
                    .Where(static option => option.EnumDefinition is not null)
                    .Select(option => option.EnumDefinition! with
                    {
                        EnumName = GeneratorUtils.GetAliasedClassName(
                            tool,
                            alias,
                            option.EnumDefinition!.EnumName),
                    })))
            .GroupBy(static definition => definition.EnumName, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => group.SelectMany(static definition => definition.Values)
                    .DistinctBy(static value => value.CliValue, StringComparer.Ordinal)
                    .ToArray(),
                StringComparer.Ordinal);

        foreach (var pair in enumBaseline.ToArray())
        {
            if (!currentAliasEnums.TryGetValue(pair.Key, out var currentValues))
            {
                continue;
            }

            enumBaseline[pair.Key] = pair.Value with
            {
                Values = pair.Value.Values
                    .Concat(currentValues)
                    .DistinctBy(static value => value.CliValue, StringComparer.Ordinal)
                    .ToArray(),
            };
        }
    }

    private static int? GetEnumNumericValue(ExpressionSyntax? expression) => expression switch
    {
        LiteralExpressionSyntax literal when literal.Token.Value is int value => value,
        PrefixUnaryExpressionSyntax prefix when prefix.IsKind(SyntaxKind.UnaryMinusExpression)
                                                 && prefix.Operand is LiteralExpressionSyntax literal
                                                 && literal.Token.Value is int value => -value,
        _ => null,
    };

    private static CliCompatibilityConstructor[] ReadCompatibilityConstructors(
        RecordDeclarationSyntax declaration) =>
        declaration.Members
            .OfType<ConstructorDeclarationSyntax>()
            .Where(constructor => constructor.Modifiers.Any(SyntaxKind.PublicKeyword))
            .Where(constructor => constructor.Initializer?.IsKind(
                SyntaxKind.ThisConstructorInitializer) == true)
            .Select(constructor => ReadCompatibilityConstructor(declaration, constructor))
            .ToArray();

    private static CliCompatibilityConstructor ReadCompatibilityConstructor(
        RecordDeclarationSyntax declaration,
        ConstructorDeclarationSyntax constructor)
    {
        var parameters = constructor.ParameterList.Parameters
            .Select(parameter => new CliCompatibilityConstructorParameter(
                parameter.Identifier.ValueText,
                parameter.Type?.ToString() ?? string.Empty))
            .ToArray();
        return new CliCompatibilityConstructor
        {
            Parameters = parameters,
            PrimaryConstructorArguments = constructor.Initializer!.ArgumentList.Arguments
                .Select(argument => argument.Expression.ToString())
                .ToArray(),
            PreserveDeconstruct = HasMatchingDeconstruct(declaration, parameters),
        };
    }

    private static bool HasMatchingDeconstruct(
        RecordDeclarationSyntax declaration,
        IReadOnlyList<CliCompatibilityConstructorParameter> parameters) =>
        declaration.Members
            .OfType<MethodDeclarationSyntax>()
            .Where(method => method.Identifier.ValueText.Equals("Deconstruct", StringComparison.Ordinal))
            .Where(method => method.Modifiers.Any(SyntaxKind.PublicKeyword))
            .Any(method => method.ParameterList.Parameters.Count == parameters.Count
                           && method.ParameterList.Parameters
                               .Zip(parameters)
                               .All(pair => pair.First.Modifiers.Any(SyntaxKind.OutKeyword)
                                            && pair.First.Identifier.ValueText.Equals(
                                                pair.Second.PropertyName,
                                                StringComparison.Ordinal)
                                            && (pair.First.Type?.ToString() ?? string.Empty).Equals(
                                                pair.Second.CSharpType,
                                                StringComparison.Ordinal)));

    private static void RejectRemovedFacadeMethods(
        CliToolDefinition tool,
        IReadOnlyList<GeneratedFacadeMethod> baselineFacadeMethods)
    {
        var currentFacadeMethods = GenerateFacadeMethods(tool).ToHashSet();
        var removedMethods = baselineFacadeMethods
            .Where(method => !currentFacadeMethods.Contains(method))
            .Distinct()
            .OrderBy(static method => method.DeclaringType, StringComparer.Ordinal)
            .ThenBy(static method => method.MethodName, StringComparer.Ordinal)
            .ThenBy(static method => method.OptionsType, StringComparer.Ordinal)
            .ToArray();
        if (removedMethods.Length == 0)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Generated API compatibility validation failed for {tool.ToolName}:"
            + Environment.NewLine
            + string.Join(
                Environment.NewLine,
                removedMethods.Select(method =>
                    $"- {method.DeclaringType}.{method.MethodName}({method.OptionsType}): "
                    + $"{method.OptionsType} command disappeared from generated facade")));
    }

    private static IReadOnlyList<GeneratedFacadeMethod> GenerateFacadeMethods(CliToolDefinition tool)
    {
        var generatedFiles = new List<GeneratedFile>();
        generatedFiles.AddRange(
            new ServiceInterfaceGenerator().GenerateAsync(tool).GetAwaiter().GetResult());
        generatedFiles.AddRange(
            new ServiceImplementationGenerator().GenerateAsync(tool).GetAwaiter().GetResult());
        generatedFiles.AddRange(
            new SubDomainClassGenerator().GenerateAsync(tool).GetAwaiter().GetResult());
        return ReadFacadeMethods(
            generatedFiles.Select(static file => file.Content),
            $"{tool.TargetNamespace}.Services");
    }

    private static IReadOnlyList<GeneratedFacadeMethod> ReadFacadeMethods(
        string servicesDirectory,
        string targetNamespace,
        string namespacePrefix)
    {
        if (!Directory.Exists(servicesDirectory))
        {
            return [];
        }

        var sources = Directory.EnumerateFiles(
                servicesDirectory,
                $"{namespacePrefix}*.Generated.cs",
                SearchOption.TopDirectoryOnly)
            .Select(File.ReadAllText);
        return ReadFacadeMethods(sources, targetNamespace);
    }

    private static IReadOnlyList<GeneratedFacadeMethod> ReadFacadeMethods(
        IEnumerable<string> sources,
        string targetNamespace)
    {
        var methods = new List<GeneratedFacadeMethod>();
        foreach (var source in sources)
        {
            var root = CSharpSyntaxTree.ParseText(source).GetRoot();
            foreach (var method in root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                         .Where(method => method.Ancestors()
                             .OfType<BaseNamespaceDeclarationSyntax>()
                             .FirstOrDefault()?.Name.ToString().Equals(
                                 targetNamespace,
                                 StringComparison.Ordinal) == true)
                         .Where(method => method.Modifiers.Any(SyntaxKind.PublicKeyword)))
            {
                var optionsParameter = method.ParameterList.Parameters.FirstOrDefault();
                var optionsType = optionsParameter?.Type?.ToString();
                var declaringType = method.Ancestors()
                    .OfType<TypeDeclarationSyntax>()
                    .FirstOrDefault()?.Identifier.ValueText;
                if (!string.IsNullOrWhiteSpace(declaringType)
                    && !string.IsNullOrWhiteSpace(optionsType)
                    && optionsType.TrimEnd('?').EndsWith("Options", StringComparison.Ordinal))
                {
                    methods.Add(new GeneratedFacadeMethod(
                        declaringType,
                        method.Identifier.ValueText,
                        optionsType.TrimEnd('?'),
                        optionsParameter?.Default is not null));
                }
            }
        }

        return methods;
    }

    private static List<GeneratedApiProperty> ReadProperties(
        RecordDeclarationSyntax declaration)
    {
        var properties = new List<GeneratedApiProperty>();
        if (declaration.ParameterList is not null)
        {
            properties.AddRange(declaration.ParameterList.Parameters.Select(parameter =>
                ReadProperty(
                    parameter.Identifier.ValueText,
                    parameter.Type?.ToString() ?? string.Empty,
                    parameter.AttributeLists,
                    isRequired: true,
                    accessorList: null)));
        }
        else
        {
            var baseConstructor = declaration.Members
                .OfType<ConstructorDeclarationSyntax>()
                .FirstOrDefault(constructor =>
                    constructor.Modifiers.Any(SyntaxKind.PublicKeyword)
                    && constructor.Initializer?.IsKind(SyntaxKind.BaseConstructorInitializer) == true);
            if (baseConstructor is not null)
            {
                properties.AddRange(baseConstructor.ParameterList.Parameters.Select(parameter =>
                    new GeneratedApiProperty(
                        parameter.Identifier.ValueText,
                        parameter.Type?.ToString() ?? string.Empty,
                        null,
                        null,
                        true,
                        false,
                        null,
                        null)));
            }
        }

        properties.AddRange(declaration.Members
            .OfType<PropertyDeclarationSyntax>()
            .Where(property => property.Modifiers.Any(SyntaxKind.PublicKeyword))
            .Select(property => ReadProperty(
                property.Identifier.ValueText,
                property.Type.ToString(),
                property.AttributeLists,
                isRequired: false,
                property.AccessorList)));
        return properties;
    }

    private static GeneratedApiProperty ReadProperty(
        string propertyName,
        string cSharpType,
        SyntaxList<AttributeListSyntax> attributeLists,
        bool isRequired,
        AccessorListSyntax? accessorList)
    {
        var attributes = attributeLists.SelectMany(list => list.Attributes).ToArray();
        var cliArgument = FindAttribute(attributes, "CliArgument");
        var cliOption = FindAttribute(attributes, "CliOption")
                        ?? FindAttribute(attributes, "CliFlag");
        var obsolete = FindAttribute(attributes, "Obsolete");
        var forwarding = GetForwarding(accessorList);

        return new GeneratedApiProperty(
            propertyName,
            cSharpType,
            GetStringArgument(cliOption),
            GetIntegerArgument(cliArgument),
            isRequired,
            obsolete is not null,
            forwarding.TargetPropertyName,
            GetStringArgument(obsolete),
            isRequired
            || accessorList?.Accessors.Any(static accessor =>
                accessor.IsKind(SyntaxKind.InitAccessorDeclaration)) == true,
            forwarding.Kind);
    }

    private static AttributeSyntax? FindAttribute(
        IEnumerable<AttributeSyntax> attributes,
        string name) =>
        attributes.FirstOrDefault(attribute =>
            attribute.Name.ToString().Equals(name, StringComparison.Ordinal)
            || attribute.Name.ToString().Equals($"{name}Attribute", StringComparison.Ordinal));

    private static string? GetStringArgument(AttributeSyntax? attribute) =>
        attribute?.ArgumentList?.Arguments.FirstOrDefault()?.Expression is LiteralExpressionSyntax literal
            && literal.IsKind(SyntaxKind.StringLiteralExpression)
                ? literal.Token.ValueText
                : null;

    private static int? GetIntegerArgument(AttributeSyntax? attribute) =>
        attribute?.ArgumentList?.Arguments.FirstOrDefault()?.Expression is LiteralExpressionSyntax literal
            && literal.IsKind(SyntaxKind.NumericLiteralExpression)
            && literal.Token.Value is int value
                ? value
                : null;

    private static (string? TargetPropertyName, CliCompatibilityForwardingKind Kind) GetForwarding(
        AccessorListSyntax? accessorList)
    {
        var expression = accessorList?.Accessors
            .FirstOrDefault(accessor => accessor.IsKind(SyntaxKind.GetAccessorDeclaration))?
            .ExpressionBody?.Expression;
        if (expression is IdentifierNameSyntax identifier)
        {
            var setterExpression = accessorList?.Accessors
                .FirstOrDefault(accessor =>
                    accessor.IsKind(SyntaxKind.SetAccessorDeclaration)
                    || accessor.IsKind(SyntaxKind.InitAccessorDeclaration))?
                .ExpressionBody?.Expression;
            if (setterExpression is AssignmentExpressionSyntax
                {
                    Left: IdentifierNameSyntax setterTarget,
                    Right: BinaryExpressionSyntax
                    {
                        RawKind: (int) SyntaxKind.CoalesceExpression,
                        Left: IdentifierNameSyntax { Identifier.ValueText: "value" },
                        Right: MemberAccessExpressionSyntax
                        {
                            Expression: PredefinedTypeSyntax
                            {
                                Keyword.RawKind: (int) SyntaxKind.StringKeyword,
                            },
                            Name.Identifier.ValueText: "Empty",
                        },
                    },
                }
                && setterTarget.Identifier.ValueText.Equals(identifier.Identifier.ValueText, StringComparison.Ordinal))
            {
                return (
                    identifier.Identifier.ValueText,
                    CliCompatibilityForwardingKind.NullableStringToRequiredString);
            }

            return (identifier.Identifier.ValueText, CliCompatibilityForwardingKind.Direct);
        }

        if (expression is ConditionalAccessExpressionSyntax
            {
                Expression: IdentifierNameSyntax collection,
                WhenNotNull: InvocationExpressionSyntax
                {
                    Expression: MemberBindingExpressionSyntax member,
                },
            }
            && member.Name.Identifier.ValueText.Equals("FirstOrDefault", StringComparison.Ordinal))
        {
            return (
                collection.Identifier.ValueText,
                CliCompatibilityForwardingKind.ScalarToCollection);
        }

        if (expression is ConditionalExpressionSyntax
            {
                Condition: InvocationExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax
                    {
                        Name.Identifier.ValueText: "TryParse",
                    },
                    ArgumentList.Arguments: { Count: > 0 } arguments,
                },
            }
            && arguments[0].Expression is IdentifierNameSyntax stringValue)
        {
            return (
                stringValue.Identifier.ValueText,
                CliCompatibilityForwardingKind.NullableInt32ToString);
        }

        return (null, CliCompatibilityForwardingKind.Direct);
    }
}

internal sealed record GeneratedApiProperty(
    string PropertyName,
    string CSharpType,
    string? SwitchName,
    int? ArgumentPosition,
    bool IsRequired,
    bool IsCompatibility,
    string? ForwardToPropertyName,
    string? ObsoleteMessage,
    bool UseInitAccessor = false,
    CliCompatibilityForwardingKind ForwardingKind = CliCompatibilityForwardingKind.Direct);

internal sealed record GeneratedApiBaseline(
    string ClassName,
    string[]? CommandParts,
    string? ParentClassName,
    IReadOnlyList<GeneratedApiProperty> Properties,
    IReadOnlyList<CliCompatibilityConstructor> Constructors);

internal sealed record GeneratedFacadeMethod(
    string DeclaringType,
    string MethodName,
    string OptionsType,
    bool IsOptionsOptional);
