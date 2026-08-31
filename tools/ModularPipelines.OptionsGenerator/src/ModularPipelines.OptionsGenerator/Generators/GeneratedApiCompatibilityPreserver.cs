using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Generators;

internal static class GeneratedApiCompatibilityPreserver
{
    private enum RequiredMemberRestoreResult
    {
        NotFound,
        Restored,
        Rejected,
    }

    private readonly record struct LocalMemberLocation(
        bool IsOption,
        int Index,
        string PropertyName);

    private readonly record struct ResolvedForwardingTarget(
        string? PropertyName,
        CliCompatibilityForwardingKind ForwardingKind,
        bool UseInitAccessor);

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

        var baseline = FilterToShippedTypes(
            ReadBaseline(optionsDirectory),
            outputDirectory,
            tool);
        var enumBaseline = ReadEnumBaseline(Path.Combine(
                outputDirectory,
                tool.OutputDirectory,
                "Enums"))
            .Where(pair => IsEnumOwnedByTool(pair.Key, tool.NamespacePrefix, baseline))
            .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal);
        MergeCurrentAliasEnumValues(tool, enumBaseline);
        tool = tool with
        {
            CompatibilityEnums = [.. tool.CompatibilityEnums
                .Concat(enumBaseline.Values)
                .DistinctBy(static definition => definition.EnumName)],
        };
        var compatibleTool = baseline.TryGetValue($"{tool.NamespacePrefix}Options", out var globalBaseline)
            ? PreserveGlobalOptions(tool, globalBaseline.Properties)
            : tool;
        var facadeMethods = ReadFacadeMethods(
            Path.Combine(outputDirectory, tool.OutputDirectory, "Services"),
            $"{tool.TargetNamespace}.Services",
            tool.NamespacePrefix,
            baseline);
        var executeFacadeOptionTypes = facadeMethods
            .Where(method => IsParentExecuteFacade(
                GetFacadeImplementationType(compatibleTool, method),
                method))
            .Select(static method => method.OptionsType)
            .ToHashSet(StringComparer.Ordinal);
        var namedFacadeOptionTypes = facadeMethods
            .Where(method => IsNamedFacadeMethod(compatibleTool, method))
            .Select(static method => method.OptionsType)
            .ToHashSet(StringComparer.Ordinal);
        var rootNamedFacadeOptionTypes = facadeMethods
            .Where(method => IsRootFacadeMethod(compatibleTool, method)
                             && IsNamedFacadeMethod(compatibleTool, method))
            .Select(static method => method.OptionsType)
            .ToHashSet(StringComparer.Ordinal);
        var optionalFacadeOptionTypes = facadeMethods
            .Where(static method => method.IsOptionsOptional)
            .Select(static method => method.OptionsType)
            .ToHashSet(StringComparer.Ordinal);
        var liveCommands = compatibleTool.Commands
            .Select(command => PreserveIdentifierCasing(
                compatibleTool,
                command,
                baseline,
                facadeMethods))
            .ToArray();
        RejectLiveCommandClassNameCollisions(liveCommands);
        var commands = liveCommands
            .Concat(RestoreRemovedCommands(
                compatibleTool with { Commands = liveCommands },
                baseline,
                facadeMethods))
            .DistinctBy(static command => command.ClassName, StringComparer.Ordinal)
            .ToArray();
        var commandGlobalOptions = compatibleTool.GetGlobalOptions();
        var commandGlobalCompatibilityProperties = compatibleTool.GlobalCompatibilityProperties;
        var preservedTool = compatibleTool with
        {
            Commands = [.. commands
                .Select(command => baseline.TryGetValue(command.ClassName, out var commandBaseline)
                    ? Preserve(
                        command,
                        commandBaseline.Properties,
                        commandBaseline.Constructors,
                        commandGlobalOptions,
                        commandGlobalCompatibilityProperties)
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
                .Select(command => rootNamedFacadeOptionTypes.Contains(command.ClassName)
                    ? command with { PreserveRootNamedFacade = true }
                    : command)
                .Select(command => optionalFacadeOptionTypes.Contains(command.ClassName)
                    ? command with { PreserveOptionalOptionsParameter = true }
                    : command)],
        };
        var currentFacadeMethods = GenerateFacadeMethods(preservedTool);
        preservedTool = preservedTool with
        {
            Commands = [.. preservedTool.Commands.Select(command =>
                PreserveFacadeMethodCompatibility(
                    command,
                    facadeMethods,
                    currentFacadeMethods))],
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
        var rootCommands = GeneratorUtils.GetNonCollidingRootCommands(tool).ToHashSet();
        var facadeMethodsByOptionsType = facadeMethods
            .GroupBy(static method => method.OptionsType, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => (IReadOnlyList<GeneratedFacadeMethod>) [.. group],
                StringComparer.Ordinal);
        foreach (var commandBaseline in baseline.Values
                     .Where(command => command.CommandParts is { Length: > 0 }
                                       && HasToolOptionsAncestor(command, tool.NamespacePrefix, baseline)
                                       && !tool.Commands.Any(current => current.CommandParts.SequenceEqual(
                                           command.CommandParts,
                                           StringComparer.OrdinalIgnoreCase))
                                       && !currentOptionTypes.Contains(command.ClassName))
                     .OrderBy(static command => command.ClassName, StringComparer.Ordinal))
        {
            yield return RestoreRemovedCommand(
                tool,
                commandBaseline,
                facadeMethodsByOptionsType.GetValueOrDefault(commandBaseline.ClassName) ?? [],
                rootCommands);
        }
    }

    private static bool HasToolOptionsAncestor(
        GeneratedApiBaseline command,
        string namespacePrefix,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline)
    {
        var expectedRoot = $"{namespacePrefix}Options";
        var parentClassName = command.ParentClassName;
        var visited = new HashSet<string>(StringComparer.Ordinal);
        while (parentClassName is not null && visited.Add(parentClassName))
        {
            if (parentClassName.Equals(expectedRoot, StringComparison.Ordinal))
            {
                return true;
            }

            parentClassName = baseline.GetValueOrDefault(parentClassName)?.ParentClassName;
        }

        return false;
    }

    private static bool IsEnumOwnedByTool(
        string enumName,
        string namespacePrefix,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline) =>
        baseline.Values
            .Where(candidate => candidate.ClassName.Equals(
                                    $"{namespacePrefix}Options",
                                    StringComparison.Ordinal)
                                || HasToolOptionsAncestor(candidate, namespacePrefix, baseline))
            .SelectMany(static candidate => candidate.Properties)
            .Any(property => TypeReferencesIdentifier(property.CSharpType, enumName));

    private static bool TypeReferencesIdentifier(string typeName, string identifier)
    {
        for (var startIndex = 0;
             (startIndex = typeName.IndexOf(identifier, startIndex, StringComparison.Ordinal)) >= 0;
             startIndex += identifier.Length)
        {
            var beforeIsIdentifier = startIndex > 0
                                     && SyntaxFacts.IsIdentifierPartCharacter(typeName[startIndex - 1]);
            var endIndex = startIndex + identifier.Length;
            var afterIsIdentifier = endIndex < typeName.Length
                                    && SyntaxFacts.IsIdentifierPartCharacter(typeName[endIndex]);
            if (!beforeIsIdentifier && !afterIsIdentifier)
            {
                return true;
            }
        }

        return false;
    }

    private static CliCommandDefinition RestoreRemovedCommand(
        CliToolDefinition tool,
        GeneratedApiBaseline baseline,
        IReadOnlyList<GeneratedFacadeMethod> facadeMethods,
        HashSet<CliCommandDefinition> rootCommands)
    {
        var commandParts = baseline.CommandParts!;
        var groupIdentifier = GetRestoredCommandGroupIdentifier(tool, baseline, facadeMethods);
        var preserveRootNamedFacade = facadeMethods.Any(method =>
            IsRootFacadeMethod(tool, method)
            && IsNamedFacadeMethod(tool, method));
        var replacementRootMethodName = FindLiveOperandReplacementRootMethodName(
            tool,
            commandParts,
            rootCommands);
        var subDomainGroup = commandParts.Length > 1 && !preserveRootNamedFacade
            ? GetRestoredSubDomainGroup(tool, commandParts[0], groupIdentifier)
            : null;
        var isConditionallyAvailable = IsConditionallyAvailableCommand(tool, commandParts);

        return new CliCommandDefinition
        {
            FullCommand = $"{tool.ToolName} {string.Join(' ', commandParts)}",
            CommandParts = commandParts,
            ClassName = baseline.ClassName,
            ParentClassName = baseline.ParentClassName ?? $"{tool.NamespacePrefix}Options",
            ToolNamespacePrefix = tool.NamespacePrefix,
            IsCompatibilityOnly = !isConditionallyAvailable,
            Options = RestoreRemovedOptions(baseline.Properties),
            PositionalArguments = RestoreRemovedPositionalArguments(baseline.Properties),
            CompatibilityProperties = RestoreRemovedCompatibilityProperties(baseline.Properties),
            CompatibilityConstructors = baseline.Constructors,
            CompatibilityMethods = [.. facadeMethods
                .Where(method => IsNamedFacadeMethod(tool, method))
                .Select(method => new CliCompatibilityMethod
                {
                    MethodName = method.MethodName,
                    ObsoleteMessage = !IsRootFacadeMethod(tool, method)
                        ? "Use the current command facade instead."
                        : replacementRootMethodName is null
                            ? GeneratorUtils.CompatibilityOnlyObsoleteMessage
                            : $"Use {replacementRootMethodName} instead.",
                })
                .DistinctBy(static method => method.MethodName, StringComparer.Ordinal)],
            SubDomainGroup = subDomainGroup,
            CommandGroupIdentifierOverride = commandParts.Length > 1 && !preserveRootNamedFacade
                ? groupIdentifier
                : null,
            CommandPartIdentifierOverrides = GetRestoredCommandPartIdentifierOverrides(
                tool,
                commandParts,
                groupIdentifier,
                facadeMethods),
            PreserveExecuteFacade = facadeMethods.Any(method => IsParentExecuteFacade(
                GetFacadeImplementationType(tool, method),
                method)),
            PreserveNamedFacade = facadeMethods.Any(method => IsNamedFacadeMethod(tool, method)),
            PreserveRootNamedFacade = preserveRootNamedFacade,
            PreserveOptionalOptionsParameter = facadeMethods.Any(static method => method.IsOptionsOptional),
        };
    }

    private static bool IsConditionallyAvailableCommand(
        CliToolDefinition tool,
        IReadOnlyList<string> commandParts) =>
        tool.CommandCoverage.ConditionallyAvailableCommands.Any(command =>
            command.Command.Split((char[]?) null, StringSplitOptions.RemoveEmptyEntries)
                .SequenceEqual(
                    [tool.ToolName, .. commandParts],
                    StringComparer.OrdinalIgnoreCase));

    private static string? FindLiveOperandReplacementRootMethodName(
        CliToolDefinition tool,
        string[] removedCommandParts,
        HashSet<CliCommandDefinition> rootCommands)
    {
        var candidates = tool.Commands
            .Where(static command => !command.IsCompatibilityOnly)
            .Where(command => command.CommandParts.Length < removedCommandParts.Length)
            .Where(command => command.CommandParts.SequenceEqual(
                removedCommandParts.Take(command.CommandParts.Length),
                StringComparer.OrdinalIgnoreCase))
            .Where(command => command.PositionalArguments.Any(argument =>
                argument is { IsRequired: true, PositionIndex: 0 }
                && argument.PropertyName.Equals(
                    GeneratorUtils.GenerateMethodNameFromCommandParts(
                        [.. removedCommandParts.Skip(command.CommandParts.Length)]),
                    StringComparison.Ordinal)))
            .Where(rootCommands.Contains)
            .OrderByDescending(static command => command.CommandParts.Length)
            .ToArray();
        if (candidates.Length == 0)
        {
            return null;
        }

        var longestPathLength = candidates[0].CommandParts.Length;
        var longestMatches = candidates
            .TakeWhile(command => command.CommandParts.Length == longestPathLength)
            .Take(2)
            .ToArray();
        return longestMatches.Length == 1
            ? GeneratorUtils.EnsureAsyncSuffix(
                GeneratorUtils.GenerateMethodNameFromCommandParts(longestMatches[0].CommandParts))
            : null;
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
                [.. baseline.CommandParts!.Select(GeneratorUtils.ToPascalCase)]);
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

            recoveredOverrides ??= [];
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

        return recoveredOverrides ?? [];
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

    private static bool IsRootFacadeMethod(
        CliToolDefinition tool,
        GeneratedFacadeMethod facadeMethod) =>
        facadeMethod.DeclaringType.Equals(tool.NamespacePrefix, StringComparison.Ordinal)
        || facadeMethod.DeclaringType.Equals($"I{tool.NamespacePrefix}", StringComparison.Ordinal);

    private static void RejectLiveCommandClassNameCollisions(
        IReadOnlyList<CliCommandDefinition> commands)
    {
        var collisions = commands
            .GroupBy(static command => command.ClassName, StringComparer.Ordinal)
            .Where(static group => group.Skip(1).Any())
            .ToArray();
        if (collisions.Length == 0)
        {
            return;
        }

        throw new InvalidOperationException(
            "Generated API compatibility restoration produced duplicate command class name(s): "
            + string.Join(
                "; ",
                collisions.Select(group =>
                    $"{group.Key} ({string.Join(", ", group.Select(static command => command.FullCommand))})")));
    }

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

    private static bool IsNamedFacadeMethod(
        CliToolDefinition tool,
        GeneratedFacadeMethod method) =>
        !method.MethodName.Equals("ExecuteAsync", StringComparison.Ordinal)
        || !IsParentExecuteFacade(GetFacadeImplementationType(tool, method), method);

    private static CliOptionDefinition[] RestoreRemovedOptions(
        IEnumerable<GeneratedApiProperty> properties) =>
        [.. properties
            .Where(static property => !property.IsCompatibility && property.SwitchName is not null)
            .Select(static property => new CliOptionDefinition
            {
                SwitchName = property.SwitchName!,
                ShortForm = property.ShortForm,
                PreferShortForm = property.PreferShortForm,
                PropertyName = property.PropertyName,
                CSharpType = property.CSharpType,
                IsRequired = property.IsRequired,
                IsFlag = property.IsFlag ?? property.CSharpType is "bool" or "bool?",
                ValueArity = property.ValueArity,
                Phase = property.Phase ?? CommandLinePhase.Normal,
                GroupValues = property.GroupValues,
                ValueSeparator = property.ValueSeparator,
                IsSecret = property.IsSecret,
                SecretValueKeys = property.SecretValueKeys ?? [],
                ValidationConstraints = property.ValidationConstraints,
            })];

    private static CliPositionalArgument[] RestoreRemovedPositionalArguments(
        IEnumerable<GeneratedApiProperty> properties) =>
        [.. properties
            .Where(static property => !property.IsCompatibility && property.ArgumentPosition is not null)
            .Select(static property => new CliPositionalArgument
            {
                PropertyName = property.PropertyName,
                CSharpType = property.CSharpType,
                PositionIndex = property.ArgumentPosition!.Value,
                IsRequired = property.IsRequired,
                Phase = property.Phase ?? CommandLinePhase.Passthrough,
                PrependOptionTerminator = property.PrependOptionTerminator,
                RepeatOptionTerminator = property.RepeatOptionTerminator,
                PrependOptionTerminatorIfValueStartsWithDash =
                    property.PrependOptionTerminatorIfValueStartsWithDash,
                IsSecret = property.IsSecret,
            })];

    private static CliCompatibilityProperty[] RestoreRemovedCompatibilityProperties(
        IEnumerable<GeneratedApiProperty> properties) =>
        [.. properties
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
            })];

    private static CliCommandDefinition PreserveIdentifierCasing(
        CliToolDefinition tool,
        CliCommandDefinition command,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline,
        IReadOnlyList<GeneratedFacadeMethod> facadeMethods)
    {
        var matchingBaseline = FindCommandBaseline(tool, command, baseline);
        var preserved = command with
        {
            ClassName = matchingBaseline?.ClassName
                        ?? FindBaselineIdentifier(command.ClassName, baseline.Keys),
            ParentClassName = matchingBaseline?.ParentClassName
                              ?? FindBaselineIdentifier(command.ParentClassName, baseline.Keys),
        };
        preserved = PreserveCommandScopedEnumCasing(command, preserved);
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
        if (commandFacadeMethods.Length == 0)
        {
            groupIdentifier = GetHistoricalSiblingRootIdentifier(
                                  tool,
                                  commandBaseline,
                                  baseline,
                                  facadeMethods)
                              ?? groupIdentifier;
        }

        var recoveredOverrides = GetRestoredCommandPartIdentifierOverrides(
            tool,
            commandBaseline.CommandParts,
            groupIdentifier,
            commandFacadeMethods);
        var mergedOverrides = preserved.CommandPartIdentifierOverrides
            .Concat(recoveredOverrides)
            .DistinctBy(static pair => pair.Key)
            .ToDictionary(static pair => pair.Key, static pair => pair.Value);
        var commandGroupIdentifierOverride = preserved.CommandGroupIdentifierOverride;
        if (preserved.CommandParts.Length > 1)
        {
            commandGroupIdentifierOverride = commandFacadeMethods.Length > 0
                ? groupIdentifier
                : commandGroupIdentifierOverride ?? groupIdentifier;
        }

        return preserved with
        {
            CommandGroupIdentifierOverride = commandGroupIdentifierOverride,
            CommandPartIdentifierOverrides = mergedOverrides,
        };
    }

    private static string? GetHistoricalSiblingRootIdentifier(
        CliToolDefinition tool,
        GeneratedApiBaseline commandBaseline,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline,
        IReadOnlyList<GeneratedFacadeMethod> facadeMethods)
    {
        if (commandBaseline.CommandParts is not { Length: > 1 } targetCommandParts)
        {
            return null;
        }

        var rootCommand = targetCommandParts[0];
        var branchCommand = targetCommandParts[1];

        var identifiers = new HashSet<string>(StringComparer.Ordinal);
        foreach (var method in facadeMethods)
        {
            if (!baseline.TryGetValue(method.OptionsType, out var methodBaseline)
                || methodBaseline.CommandParts is not { Length: > 1 } commandParts
                || !commandParts[0].Equals(rootCommand, StringComparison.OrdinalIgnoreCase)
                || !commandParts[1].Equals(branchCommand, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (GetRootIdentifierFromFacade(tool, commandParts, method) is { } identifier)
            {
                identifiers.Add(identifier);
            }
        }

        return identifiers.Count == 1 ? identifiers.Single() : null;
    }

    private static CliCommandDefinition PreserveCommandScopedEnumCasing(
        CliCommandDefinition original,
        CliCommandDefinition preserved)
    {
        if (original.ClassName.Equals(preserved.ClassName, StringComparison.Ordinal))
        {
            return preserved;
        }

        var originalPrefix = GetOptionsClassPrefix(original.ClassName);
        var preservedPrefix = GetOptionsClassPrefix(preserved.ClassName);
        CliEnumDefinition Rename(CliEnumDefinition definition) =>
            definition.EnumName.StartsWith(originalPrefix, StringComparison.Ordinal)
                ? definition with
                {
                    EnumName = preservedPrefix
                               + definition.EnumName[originalPrefix.Length..],
                }
                : definition;
        var enumRenames = original.Enums
            .Concat(original.Options
                .Where(static option => option.EnumDefinition is not null)
                .Select(static option => option.EnumDefinition!))
            .DistinctBy(static definition => definition.EnumName, StringComparer.Ordinal)
            .Select(definition => (Original: definition.EnumName, Renamed: Rename(definition).EnumName))
            .Where(static rename => !rename.Original.Equals(rename.Renamed, StringComparison.Ordinal))
            .ToDictionary(static rename => rename.Original, static rename => rename.Renamed, StringComparer.Ordinal);

        return preserved with
        {
            Enums = [.. preserved.Enums.Select(Rename)],
            Options = [.. preserved.Options.Select(option => RenameOptionEnum(
                    option,
                    enumRenames,
                    option.EnumDefinition is null ? null : Rename(option.EnumDefinition)))],
            CompatibilityProperties = [.. preserved.CompatibilityProperties
                .Select(property => property with
                {
                    CSharpType = RenameEnumType(property.CSharpType, enumRenames),
                })],
        };
    }

    private static string RenameEnumType(
        string cSharpType,
        IReadOnlyDictionary<string, string> enumRenames)
    {
        var enumTypeName = GeneratorUtils.GetEnumTypeName(cSharpType);
        return enumRenames.TryGetValue(enumTypeName, out var renamedType)
            ? cSharpType.Replace(enumTypeName, renamedType, StringComparison.Ordinal)
            : cSharpType;
    }

    private static CliOptionDefinition RenameOptionEnum(
        CliOptionDefinition option,
        IReadOnlyDictionary<string, string> enumRenames,
        CliEnumDefinition? renamedDefinition) =>
        option with
        {
            CSharpType = RenameEnumType(option.CSharpType, enumRenames),
            EnumDefinition = renamedDefinition,
        };

    private static string GetOptionsClassPrefix(string className) =>
        className.EndsWith("Options", StringComparison.Ordinal)
            ? className[..^"Options".Length]
            : className;

    private static GeneratedApiBaseline? FindCommandBaseline(
        CliToolDefinition tool,
        CliCommandDefinition command,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline)
    {
        if (baseline.TryGetValue(command.ClassName, out var exact))
        {
            return exact;
        }

        var pathMatches = baseline.Values
            .Where(candidate => candidate.CommandParts is not null
                                && HasToolOptionsAncestor(candidate, tool.NamespacePrefix, baseline)
                                && candidate.CommandParts.SequenceEqual(
                                    command.CommandParts,
                                    StringComparer.OrdinalIgnoreCase))
            .ToArray();
        if (pathMatches.Length == 1)
        {
            return pathMatches[0];
        }

        var parentMatches = pathMatches
            .Where(candidate => string.Equals(
                candidate.ParentClassName,
                command.ParentClassName,
                StringComparison.OrdinalIgnoreCase))
            .Take(2)
            .ToArray();
        return parentMatches.Length == 1 ? parentMatches[0] : null;
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

    private static CliCommandDefinition PreserveFacadeMethodCompatibility(
        CliCommandDefinition command,
        IReadOnlyList<GeneratedFacadeMethod> baselineFacadeMethods,
        IReadOnlyList<GeneratedFacadeMethod> currentFacadeMethods)
    {
        var currentMethods = currentFacadeMethods
            .Where(method => method.OptionsType.Equals(command.ClassName, StringComparison.Ordinal))
            .ToArray();
        var compatibilityMethods = baselineFacadeMethods
            .Where(method => method.OptionsType.Equals(command.ClassName, StringComparison.Ordinal)
                             && !currentFacadeMethods.Contains(method))
            .Select(method => (Baseline: method, Replacements: currentMethods
                .Where(current => current.DeclaringType.Equals(method.DeclaringType, StringComparison.Ordinal))
                .Select(static current => current.MethodName)
                .Distinct(StringComparer.Ordinal)
                .ToArray()))
            .Where(static match => match.Replacements.Length == 1)
            .Select(match => new CliCompatibilityMethod
            {
                MethodName = match.Baseline.MethodName,
                ObsoleteMessage = $"Use {match.Replacements[0]} instead.",
            });

        return command with
        {
            CompatibilityMethods = [.. command.CompatibilityMethods
                .Concat(compatibilityMethods)
                .DistinctBy(static method => method.MethodName, StringComparer.Ordinal)],
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
            compatibilityConstructors);
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
            property.PropertyName.Equals(baselineProperty.PropertyName, StringComparison.Ordinal)) ?? throw new InvalidOperationException(
                $"Cannot retain alias property {baselineProperty.PropertyName} because the canonical property is missing.");
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
        IReadOnlyList<CliCompatibilityConstructor> baselineConstructors,
        IReadOnlyList<CliOptionDefinition>? globalOptions = null,
        IReadOnlyList<CliCompatibilityProperty>? globalCompatibilityProperties = null)
    {
        baselineProperties = RemoveUnsafeBooleanStringAliases(baselineProperties, command.Options);
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
        var renamedProperties = new Dictionary<string, string>(StringComparer.Ordinal);
        var documentationRenames = new Dictionary<string, string>(StringComparer.Ordinal);
        var documentationCopies = new Dictionary<string, string>(StringComparer.Ordinal);
        RestoreNamesChangedByLocalCollisions(
            baselineProperties,
            positionalArguments,
            options,
            compatibilityProperties);
        RestoreForwardedAliasCollisions(
            baselineProperties,
            positionalArguments,
            options,
            compatibilityProperties);
        var violations = new List<string>();
        var preservedTypeChanges = PreserveScalarToCollectionChanges(
            command,
            baselineProperties,
            positionalArguments,
            options,
            compatibilityProperties,
            renamedProperties,
            violations);
        preservedTypeChanges.UnionWith(PreserveFlagToValueChanges(
            command,
            baselineProperties,
            positionalArguments,
            options,
            compatibilityProperties,
            renamedProperties,
            violations));
        preservedTypeChanges.UnionWith(PreserveOptionalValueArityChanges(
            command,
            baselineProperties,
            positionalArguments,
            options,
            compatibilityProperties,
            renamedProperties,
            violations));
        RestoreBaselinePropertyShapes(
            command,
            baselineProperties,
            preservedTypeChanges,
            positionalArguments,
            options,
            violations);

        positionalArguments = RestoreRemovedRequiredPositionalArguments(
            baselineProperties,
            positionalArguments,
            options,
            compatibilityProperties,
            documentationRenames,
            documentationCopies);

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
                baselineProperties,
                currentProperties,
                options,
                compatibilityProperties,
                renamedProperties,
                violations);
        }

        RetargetCompatibilityProperties(compatibilityProperties, renamedProperties);
        ResolveCompatibilityForwardingTargets(
            compatibilityProperties,
            [.. GetCurrentProperties(positionalArguments, options)
, .. (globalOptions ?? []).Select(ToGeneratedProperty)],
            globalCompatibilityProperties ?? []);

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
                MergeDocumentationRenames(renamedProperties, documentationRenames),
                documentationCopies),
        };
    }

    private static Dictionary<string, string> MergeDocumentationRenames(
        IReadOnlyDictionary<string, string> memberRenames,
        IReadOnlyDictionary<string, string> documentationRenames)
    {
        var merged = memberRenames.ToDictionary(
            static rename => rename.Key,
            static rename => rename.Value,
            StringComparer.Ordinal);
        foreach (var rename in documentationRenames)
        {
            merged[rename.Key] = rename.Value;
        }

        return merged;
    }

    private static void RestoreNamesChangedByLocalCollisions(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options,
        IList<CliCompatibilityProperty> compatibilityProperties)
    {
        var propertyNames = options.Select(static option => option.PropertyName)
            .Concat(positionalArguments.Select(static argument => argument.PropertyName))
            .Concat(compatibilityProperties.Select(static property => property.PropertyName))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var baseline in baselineProperties.Where(static property => !property.IsCompatibility))
        {
            var historicalMember = FindLocalMember(
                positionalArguments,
                options,
                property => HasSameCliIdentity(property, baseline));
            if (historicalMember is null
                || historicalMember.Value.PropertyName.Equals(
                    baseline.PropertyName,
                    StringComparison.Ordinal))
            {
                continue;
            }

            var conflictingMember = FindLocalMember(
                positionalArguments,
                options,
                property => property.PropertyName.Equals(
                    baseline.PropertyName,
                    StringComparison.Ordinal));
            if (conflictingMember is null)
            {
                continue;
            }

            propertyNames.Remove(historicalMember.Value.PropertyName);
            propertyNames.Remove(conflictingMember.Value.PropertyName);
            var conflictingName = GetUniquePropertyName(
                baseline.PropertyName + (conflictingMember.Value.IsOption ? "Option" : "Argument"),
                propertyNames);
            RenameLocalMember(conflictingMember.Value, conflictingName, positionalArguments, options);
            RenameLocalMember(historicalMember.Value, baseline.PropertyName, positionalArguments, options);
            var historicalForwarders = baselineProperties
                .Where(property => property is { IsCompatibility: true, ForwardToPropertyName: not null }
                    && property.ForwardToPropertyName.Equals(
                        baseline.PropertyName,
                        StringComparison.Ordinal))
                .Select(static property => property.PropertyName)
                .ToHashSet(StringComparer.Ordinal);
            RetargetCompatibilityProperties(
                compatibilityProperties,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    [baseline.PropertyName] = conflictingName,
                },
                historicalForwarders);
            propertyNames.Add(baseline.PropertyName);
        }
    }

    private static void RestoreForwardedAliasCollisions(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options,
        IList<CliCompatibilityProperty> compatibilityProperties)
    {
        var propertyNames = options.Select(static option => option.PropertyName)
            .Concat(positionalArguments.Select(static argument => argument.PropertyName))
            .Concat(compatibilityProperties.Select(static property => property.PropertyName))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var alias in baselineProperties.Where(static property =>
                     property is { IsCompatibility: true, ForwardToPropertyName: not null }))
        {
            var aliasMember = FindLocalMember(
                positionalArguments,
                options,
                property => property.PropertyName.Equals(alias.PropertyName, StringComparison.Ordinal));
            var targetBaseline = FindForwardingTargetBaseline(baselineProperties, alias);
            if (aliasMember is null || targetBaseline is null)
            {
                continue;
            }

            if (HasSameCliIdentity(
                    GetLocalMember(aliasMember.Value, positionalArguments, options),
                    targetBaseline)
                && TryRestoreForwardedAliasTarget(
                    alias,
                    aliasMember.Value,
                    targetBaseline,
                    positionalArguments,
                    options,
                    compatibilityProperties,
                    propertyNames))
            {
                continue;
            }

            var targetMember = FindLocalMember(
                positionalArguments,
                options,
                property => !property.PropertyName.Equals(alias.PropertyName, StringComparison.Ordinal)
                            && HasSameCliIdentity(property, targetBaseline));
            if (targetMember is null)
            {
                continue;
            }

            propertyNames.Remove(alias.PropertyName);
            var replacementName = GetUniquePropertyName(
                alias.PropertyName + (aliasMember.Value.IsOption ? "Option" : "Argument"),
                propertyNames);
            RenameLocalMember(aliasMember.Value, replacementName, positionalArguments, options);
            RetargetCompatibilityProperties(
                compatibilityProperties,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    [alias.PropertyName] = replacementName,
                });
            compatibilityProperties.Add(ToCompatibilityProperty(alias));
            propertyNames.Add(alias.PropertyName);
        }
    }

    private static bool TryRestoreForwardedAliasTarget(
        GeneratedApiProperty alias,
        LocalMemberLocation aliasMember,
        GeneratedApiProperty targetBaseline,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        HashSet<string> propertyNames)
    {
        if (propertyNames.Contains(targetBaseline.PropertyName)
            || (alias.ForwardingKind == CliCompatibilityForwardingKind.Direct
                && alias.PropertyName.Equals(
                    targetBaseline.PropertyName,
                    StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        propertyNames.Remove(alias.PropertyName);
        RenameLocalMember(
            aliasMember,
            targetBaseline.PropertyName,
            positionalArguments,
            options);
        compatibilityProperties.Add(ToCompatibilityProperty(alias));
        propertyNames.Add(targetBaseline.PropertyName);
        propertyNames.Add(alias.PropertyName);
        return true;
    }

    private static GeneratedApiProperty? FindForwardingTargetBaseline(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        GeneratedApiProperty alias)
    {
        var targetName = alias.ForwardToPropertyName;
        var visited = new HashSet<string>(StringComparer.Ordinal);
        while (targetName is not null && visited.Add(targetName))
        {
            var target = baselineProperties.FirstOrDefault(property =>
                property.PropertyName.Equals(targetName, StringComparison.Ordinal));
            if (target is null || !target.IsCompatibility)
            {
                return target;
            }

            targetName = target.ForwardToPropertyName;
        }

        return null;
    }

    private static GeneratedApiProperty GetLocalMember(
        LocalMemberLocation location,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        IReadOnlyList<CliOptionDefinition> options) =>
        location.IsOption
            ? ToGeneratedProperty(options[location.Index])
            : ToGeneratedProperty(positionalArguments[location.Index]);

    private static LocalMemberLocation? FindLocalMember(
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        IReadOnlyList<CliOptionDefinition> options,
        Func<GeneratedApiProperty, bool> predicate)
    {
        for (var index = 0; index < options.Count; index++)
        {
            if (predicate(ToGeneratedProperty(options[index])))
            {
                return new LocalMemberLocation(true, index, options[index].PropertyName);
            }
        }

        for (var index = 0; index < positionalArguments.Count; index++)
        {
            if (predicate(ToGeneratedProperty(positionalArguments[index])))
            {
                return new LocalMemberLocation(false, index, positionalArguments[index].PropertyName);
            }
        }

        return null;
    }

    private static void RenameLocalMember(
        LocalMemberLocation location,
        string propertyName,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options)
    {
        if (location.IsOption)
        {
            options[location.Index] = options[location.Index] with { PropertyName = propertyName };
            return;
        }

        positionalArguments[location.Index] = positionalArguments[location.Index] with
        {
            PropertyName = propertyName,
        };
    }

    private static string GetUniquePropertyName(
        string candidate,
        HashSet<string> propertyNames)
    {
        var root = candidate;
        for (var suffix = 2; !propertyNames.Add(candidate); suffix++)
        {
            candidate = root + suffix;
        }

        return candidate;
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

    private static HashSet<string> PreserveOptionalValueArityChanges(
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
                     && property.CSharpType is "string?" or "int?"
                     && property.SwitchName?.Contains('[', StringComparison.Ordinal) == true))
        {
            var optionIndex = Array.FindIndex(options, option =>
                option.PropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal)
                && option.ValueArity == CliOptionValueArity.Optional
                && NormalizeCliSwitchIdentity(baseline.SwitchName!)
                    .Equals(option.SwitchName, StringComparison.Ordinal));
            if (optionIndex < 0)
            {
                continue;
            }

            var replacementName = GetUniqueReplacementName(
                $"{baseline.PropertyName}Option",
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
                    ForwardingKind = baseline.CSharpType.Equals("int?", StringComparison.Ordinal)
                        ? CliCompatibilityForwardingKind.NullableInt32ToCliOptionValue
                        : CliCompatibilityForwardingKind.NullableStringToCliOptionValue,
                    ObsoleteMessage = $"Use {replacementName} instead.",
                },
                compatibilityProperties,
                violations);
            renamedProperties[baseline.PropertyName] = replacementName;
            preserved.Add(baseline.PropertyName);
        }

        return preserved;
    }

    private static HashSet<string> PreserveFlagToValueChanges(
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
                     !property.IsCompatibility
                     && !property.IsRequired
                     && property.CSharpType.Equals("bool?", StringComparison.Ordinal)))
        {
            var optionIndex = Array.FindIndex(options, option =>
                option.PropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal)
                && (option.PropertyType.Equals("string?", StringComparison.Ordinal)
                    || option.PropertyType.Equals("IEnumerable<string>?", StringComparison.Ordinal))
                && HasSameCliIdentity(ToGeneratedProperty(option), baseline));
            if (optionIndex < 0)
            {
                continue;
            }

            preserved.Add(baseline.PropertyName);
            var isPulumiLocalLogout = IsPulumiLocalLogoutOption(
                command,
                baseline,
                options[optionIndex]);
            if (!isPulumiLocalLogout
                && baseline.IsFlag != false
                && !ExplicitlyAcceptsBooleanText(options[optionIndex]))
            {
                continue;
            }

            var forwardsToCollection = options[optionIndex].PropertyType.Equals(
                "IEnumerable<string>?",
                StringComparison.Ordinal);
            var replacementName = GetUniqueReplacementName(
                forwardsToCollection
                    ? $"{baseline.PropertyName}Values"
                    : $"{baseline.PropertyName}Value",
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
                    ForwardingKind = forwardsToCollection
                        ? CliCompatibilityForwardingKind.NullableBooleanToStringCollection
                        : isPulumiLocalLogout
                            ? CliCompatibilityForwardingKind.NullableBooleanToLocalBackendString
                            : CliCompatibilityForwardingKind.NullableBooleanToString,
                    ObsoleteMessage = $"Use {replacementName} instead.",
                },
                compatibilityProperties,
                violations);
            renamedProperties[baseline.PropertyName] = replacementName;
        }

        return preserved;
    }

    private static bool IsPulumiLocalLogoutOption(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        CliOptionDefinition option) =>
        command.FullCommand.Equals("pulumi logout", StringComparison.OrdinalIgnoreCase)
        && baseline.PropertyName.Equals("Local", StringComparison.Ordinal)
        && option.SwitchName.Equals("--local", StringComparison.OrdinalIgnoreCase);

    private static string NormalizeCliSwitchIdentity(string switchName)
    {
        var optionalValueStart = switchName.IndexOf('[', StringComparison.Ordinal);
        if (optionalValueStart >= 0)
        {
            return switchName[..optionalValueStart];
        }

        var placeholderStart = switchName.IndexOf('<', StringComparison.Ordinal);
        if (placeholderStart < 0)
        {
            return switchName;
        }

        var switchEnd = placeholderStart;
        while (switchEnd > 0 && (switchName[switchEnd - 1] == '='
                                 || char.IsWhiteSpace(switchName[switchEnd - 1])))
        {
            switchEnd--;
        }

        return switchName[..switchEnd];
    }

    private static void RestoreBaselinePropertyShapes(
        CliCommandDefinition command,
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        ISet<string> preservedTypeChanges,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options,
        ICollection<string> violations)
    {
        foreach (var baseline in baselineProperties.Where(property =>
                     !property.IsCompatibility
                     && !preservedTypeChanges.Contains(property.PropertyName)))
        {
            if (TryRestoreBaselineOptionShape(
                    command,
                    baseline,
                    preservedTypeChanges,
                    options,
                    violations))
            {
                continue;
            }

            RestoreBaselinePositionalShape(baseline, positionalArguments);
        }
    }

    private static bool TryRestoreBaselineOptionShape(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        ISet<string> preservedTypeChanges,
        CliOptionDefinition[] options,
        ICollection<string> violations)
    {
        var index = Array.FindIndex(options, option =>
            option.PropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal)
            && HasSameCliIdentity(ToGeneratedProperty(option), baseline));
        if (index < 0 || !HasBaselineShapeDrift(ToGeneratedProperty(options[index]), baseline))
        {
            return false;
        }

        var current = ToGeneratedProperty(options[index]);
        if (IsUnsafeBooleanStringChange(baseline, current, options))
        {
            preservedTypeChanges.Add(baseline.PropertyName);
            if (baseline.IsRequired)
            {
                violations.Add(
                    $"{command.ClassName}.{baseline.PropertyName} was removed from the required constructor "
                    + $"because replacement {current.PropertyName} does not explicitly accept Boolean text");
            }

            return true;
        }

        var isCollection = CliOptionDefinition.TryGetCollectionShape(
            baseline.CSharpType,
            out var resolvedCollectionShape)
            && resolvedCollectionShape;
        var isFlag = baseline.CSharpType.Equals("bool?", StringComparison.Ordinal)
                     || baseline.CSharpType.Equals("bool", StringComparison.Ordinal);
        options[index] = options[index] with
        {
            CSharpType = baseline.CSharpType,
            IsRequired = baseline.IsRequired,
            IsFlag = isFlag,
            ValueArity = CliOptionValueArity.Required,
            AcceptsMultipleValues = isCollection,
            IsCollection = isCollection,
            EnumDefinition = null,
        };
        return true;
    }

    private static void RestoreBaselinePositionalShape(
        GeneratedApiProperty baseline,
        CliPositionalArgument[] positionalArguments)
    {
        var index = Array.FindIndex(positionalArguments, argument =>
            argument.PropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal)
            && HasSameCliIdentity(ToGeneratedProperty(argument), baseline));
        if (index < 0)
        {
            return;
        }

        var argument = positionalArguments[index];
        var restoresImplicitPhase = baseline.Phase is null
                                    && argument.Phase != CommandLinePhase.Passthrough;
        if (!restoresImplicitPhase
            && !HasBaselineShapeDrift(ToGeneratedProperty(argument), baseline))
        {
            return;
        }

        positionalArguments[index] = argument with
        {
            CSharpType = baseline.CSharpType,
            IsRequired = baseline.IsRequired,
            Phase = baseline.Phase ?? CommandLinePhase.Passthrough,
        };
    }

    private static bool HasBaselineShapeDrift(
        GeneratedApiProperty current,
        GeneratedApiProperty baseline) =>
        current.IsRequired != baseline.IsRequired
        || !current.CSharpType.Equals(baseline.CSharpType, StringComparison.Ordinal);

    private static CliPositionalArgument[] RestoreRemovedRequiredPositionalArguments(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options,
        IList<CliCompatibilityProperty> compatibilityProperties,
        IDictionary<string, string> documentationRenames,
        IDictionary<string, string> documentationCopies)
    {
        var restored = positionalArguments.ToList();
        var livePropertyNames = positionalArguments.Select(static argument => argument.PropertyName)
            .Concat(options.Select(static option => option.PropertyName))
            .Concat(compatibilityProperties.Select(static property => property.PropertyName))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var baseline in baselineProperties.Where(static property =>
                     property is
                     {
                         IsCompatibility: false,
                         IsRequired: true,
                         ArgumentPosition: not null,
                     }))
        {
            var normalizedBaseline = baseline with
            {
                Phase = baseline.Phase ?? CommandLinePhase.Passthrough,
            };
            if (restored.Any(argument => HasSameCliIdentity(
                    ToGeneratedProperty(argument),
                    normalizedBaseline)))
            {
                continue;
            }

            var occupiedIndex = restored.FindIndex(argument =>
                OccupiesSamePositionalSlot(argument, baseline));
            RenameMemberCollidingWithRestoredOperand(
                baseline,
                baselineProperties,
                restored,
                options,
                compatibilityProperties,
                livePropertyNames,
                documentationCopies,
                occupiedIndex >= 0 ? occupiedIndex : null);
            if (occupiedIndex >= 0)
            {
                var occupied = restored[occupiedIndex];
                var currentName = occupied.PropertyName;
                restored[occupiedIndex] = RestoreRequiredPositionalArgument(baseline) with
                {
                    PrependOptionTerminator = baseline.PrependOptionTerminator
                                              || occupied.PrependOptionTerminator,
                    RepeatOptionTerminator = baseline.RepeatOptionTerminator
                                             || occupied.RepeatOptionTerminator,
                    PrependOptionTerminatorIfValueStartsWithDash =
                        baseline.PrependOptionTerminatorIfValueStartsWithDash
                        || occupied.PrependOptionTerminatorIfValueStartsWithDash,
                    IsSecret = baseline.IsSecret || occupied.IsSecret,
                };
                if (!currentName.Equals(baseline.PropertyName, StringComparison.Ordinal))
                {
                    documentationRenames[currentName] = baseline.PropertyName;
                }

                continue;
            }

            restored.Add(RestoreRequiredPositionalArgument(baseline));
            livePropertyNames.Add(baseline.PropertyName);
        }

        return [.. restored];
    }

    private static bool OccupiesSamePositionalSlot(
        CliPositionalArgument argument,
        GeneratedApiProperty baseline) =>
        argument.PositionIndex == baseline.ArgumentPosition
        && argument.Phase == (baseline.Phase ?? CommandLinePhase.Passthrough);

    private static CliPositionalArgument RestoreRequiredPositionalArgument(
        GeneratedApiProperty baseline) =>
        new()
        {
            PropertyName = baseline.PropertyName,
            CSharpType = baseline.CSharpType,
            PositionIndex = baseline.ArgumentPosition.GetValueOrDefault(),
            IsRequired = true,
            Phase = baseline.Phase ?? CommandLinePhase.Passthrough,
            PrependOptionTerminator = baseline.PrependOptionTerminator,
            RepeatOptionTerminator = baseline.RepeatOptionTerminator,
            PrependOptionTerminatorIfValueStartsWithDash =
                baseline.PrependOptionTerminatorIfValueStartsWithDash,
            IsSecret = baseline.IsSecret,
        };

    private static void RenameMemberCollidingWithRestoredOperand(
        GeneratedApiProperty baseline,
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        List<CliPositionalArgument> positionalArguments,
        CliOptionDefinition[] options,
        IList<CliCompatibilityProperty> compatibilityProperties,
        HashSet<string> livePropertyNames,
        IDictionary<string, string> documentationCopies,
        int? ignoredPositionalIndex)
    {
        var collision = FindMemberCollidingWithRestoredOperand(
            baseline.PropertyName,
            positionalArguments,
            options,
            ignoredPositionalIndex);
        if (collision is null)
        {
            return;
        }

        livePropertyNames.Remove(collision.Value.PropertyName);
        var replacementName = GetUniquePropertyName(
            baseline.PropertyName + (collision.Value.IsOption ? "Option" : "Argument"),
            livePropertyNames);
        if (collision.Value.IsOption)
        {
            options[collision.Value.Index] = options[collision.Value.Index] with
            {
                PropertyName = replacementName,
            };
        }
        else
        {
            positionalArguments[collision.Value.Index] = positionalArguments[collision.Value.Index] with
            {
                PropertyName = replacementName,
            };
        }

        var historicalForwarders = baselineProperties
            .Where(property => property is { IsCompatibility: true, ForwardToPropertyName: not null }
                && property.ForwardToPropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal))
            .Select(static property => property.PropertyName)
            .ToHashSet(StringComparer.Ordinal);
        RetargetCompatibilityProperties(
            compatibilityProperties,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [baseline.PropertyName] = replacementName,
            },
            historicalForwarders);
        documentationCopies[baseline.PropertyName] = replacementName;
        livePropertyNames.Add(replacementName);
    }

    private static LocalMemberLocation? FindMemberCollidingWithRestoredOperand(
        string propertyName,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        CliOptionDefinition[] options,
        int? ignoredPositionalIndex)
    {
        var optionIndex = Array.FindIndex(options, option => option.PropertyName.Equals(
            propertyName,
            StringComparison.Ordinal));
        if (optionIndex >= 0)
        {
            return new LocalMemberLocation(true, optionIndex, options[optionIndex].PropertyName);
        }

        for (var index = 0; index < positionalArguments.Count; index++)
        {
            if (index != ignoredPositionalIndex
                && positionalArguments[index].PropertyName.Equals(propertyName, StringComparison.Ordinal))
            {
                return new LocalMemberLocation(false, index, positionalArguments[index].PropertyName);
            }
        }

        return null;
    }

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
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IReadOnlyList<GeneratedApiProperty> currentProperties,
        IReadOnlyList<CliOptionDefinition> options,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        IDictionary<string, string> renamedProperties,
        List<string> violations)
    {
        if (TryValidateSameNameProperty(
                command,
                baseline,
                baselineProperties,
                currentProperties,
                violations))
        {
            return;
        }

        if (baseline.IsCompatibility)
        {
            var caseVariantTarget = FindCaseVariantForwardingTarget(baseline, currentProperties);
            if (IsUnsafeBooleanStringChange(baseline, caseVariantTarget, options))
            {
                return;
            }

            PreserveCompatibilityProperty(
                command,
                caseVariantTarget is null
                    ? baseline
                    : baseline with
                    {
                        ForwardToPropertyName = caseVariantTarget.PropertyName,
                        ForwardingKind = GetRenamedPropertyForwardingKind(
                            baseline,
                            caseVariantTarget) ?? baseline.ForwardingKind,
                        ObsoleteMessage = $"Use {caseVariantTarget.PropertyName} instead.",
                    },
                compatibilityProperties,
                violations);
            return;
        }

        var replacement = currentProperties.FirstOrDefault(property =>
            HasSameCliIdentity(property, baseline));
        if (IsUnsafeBooleanStringChange(baseline, replacement, options))
        {
            if (baseline.IsRequired)
            {
                violations.Add(
                    $"{command.ClassName}.{baseline.PropertyName} was removed from the required constructor "
                    + $"because replacement {replacement!.PropertyName} does not explicitly accept Boolean text");
            }

            return;
        }

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

    private static GeneratedApiProperty? FindCaseVariantForwardingTarget(
        GeneratedApiProperty baseline,
        IReadOnlyList<GeneratedApiProperty> currentProperties)
    {
        if (baseline.ForwardToPropertyName is not null
            || baseline.ForwardingKind != CliCompatibilityForwardingKind.Direct)
        {
            return null;
        }

        var candidates = currentProperties
            .Where(property => !property.PropertyName.Equals(
                                   baseline.PropertyName,
                                   StringComparison.Ordinal)
                               && property.PropertyName.Equals(
                                   baseline.PropertyName,
                                   StringComparison.OrdinalIgnoreCase)
                               && GetRenamedPropertyForwardingKind(baseline, property) is not null)
            .ToArray();
        return candidates.Length == 1 ? candidates[0] : null;
    }

    private static bool TryValidateSameNameProperty(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
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

        var forwardingTarget = FindForwardingTargetBaseline(baselineProperties, baseline);
        if (baseline.IsCompatibility
            && baseline.ForwardToPropertyName is not null
            && (forwardingTarget is null || !HasSameCliIdentity(match, forwardingTarget)))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} compatibility alias now resolves to a different CLI switch or argument position");
        }
        else if (baseline.IsCompatibility)
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
        IReadOnlyDictionary<string, string> renamedProperties,
        IReadOnlySet<string>? excludedProperties = null)
    {
        for (var index = 0; index < compatibilityProperties.Count; index++)
        {
            var property = compatibilityProperties[index];
            if (excludedProperties?.Contains(property.PropertyName) != true
                && property.ForwardToPropertyName is { } target
                && renamedProperties.TryGetValue(target, out var replacement))
            {
                var renamedTarget = compatibilityProperties.FirstOrDefault(candidate =>
                    candidate.PropertyName.Equals(target, StringComparison.Ordinal)
                    && candidate.ForwardToPropertyName?.Equals(
                        replacement,
                        StringComparison.Ordinal) == true);
                compatibilityProperties[index] = property with
                {
                    ForwardToPropertyName = replacement,
                    ForwardingKind = renamedTarget is null
                        ? property.ForwardingKind
                        : ComposeCompatibilityForwardingKinds(
                            property,
                            property.ForwardingKind,
                            renamedTarget.ForwardingKind),
                    UseInitAccessor = property.UseInitAccessor
                                      || renamedTarget?.UseInitAccessor == true,
                };
            }
        }
    }

    private static void ResolveCompatibilityForwardingTargets(
        IList<CliCompatibilityProperty> compatibilityProperties,
        IReadOnlyList<GeneratedApiProperty> liveProperties,
        IReadOnlyList<CliCompatibilityProperty> inheritedCompatibilityProperties)
    {
        var livePropertyNames = liveProperties
            .Select(static property => property.PropertyName)
            .ToHashSet(StringComparer.Ordinal);
        var compatibilityByName = compatibilityProperties
            .Concat(inheritedCompatibilityProperties)
            .DistinctBy(static property => property.PropertyName, StringComparer.Ordinal)
            .ToDictionary(static property => property.PropertyName, StringComparer.Ordinal);

        for (var index = 0; index < compatibilityProperties.Count; index++)
        {
            compatibilityProperties[index] = ResolveCompatibilityForwardingTarget(
                compatibilityProperties[index],
                compatibilityByName,
                livePropertyNames);
        }
    }

    private static CliCompatibilityProperty ResolveCompatibilityForwardingTarget(
        CliCompatibilityProperty property,
        IReadOnlyDictionary<string, CliCompatibilityProperty> compatibilityByName,
        IReadOnlySet<string> livePropertyNames)
    {
        var resolved = FollowCompatibilityForwardingChain(
            property,
            compatibilityByName,
            livePropertyNames);
        if (resolved.PropertyName == property.ForwardToPropertyName)
        {
            return property;
        }

        if (resolved.PropertyName is null)
        {
            return property with
            {
                ForwardToPropertyName = null,
                ForwardingKind = CliCompatibilityForwardingKind.Direct,
                UseInitAccessor = false,
                ObsoleteMessage =
                    $"{property.PropertyName} is no longer supported by the installed CLI and has no effect.",
            };
        }

        return property with
        {
            ForwardToPropertyName = resolved.PropertyName,
            ForwardingKind = resolved.ForwardingKind,
            UseInitAccessor = resolved.UseInitAccessor,
        };
    }

    private static ResolvedForwardingTarget FollowCompatibilityForwardingChain(
        CliCompatibilityProperty property,
        IReadOnlyDictionary<string, CliCompatibilityProperty> compatibilityByName,
        IReadOnlySet<string> livePropertyNames)
    {
        var targetName = property.ForwardToPropertyName;
        var forwardingKind = property.ForwardingKind;
        var useInitAccessor = property.UseInitAccessor;
        var visited = new HashSet<string>(StringComparer.Ordinal) { property.PropertyName };

        while (targetName is not null && !livePropertyNames.Contains(targetName))
        {
            if (!visited.Add(targetName)
                || !compatibilityByName.TryGetValue(targetName, out var target))
            {
                return new ResolvedForwardingTarget(null, forwardingKind, false);
            }

            forwardingKind = ComposeCompatibilityForwardingKinds(
                property,
                forwardingKind,
                target.ForwardingKind);
            useInitAccessor |= target.UseInitAccessor;
            targetName = target.ForwardToPropertyName;
        }

        return new ResolvedForwardingTarget(targetName, forwardingKind, useInitAccessor);
    }

    private static CliCompatibilityForwardingKind ComposeCompatibilityForwardingKinds(
        CliCompatibilityProperty property,
        CliCompatibilityForwardingKind first,
        CliCompatibilityForwardingKind second) =>
        (first, second) switch
        {
            (CliCompatibilityForwardingKind.Direct, _) => second,
            (_, CliCompatibilityForwardingKind.Direct) => first,
            (CliCompatibilityForwardingKind.NullableInt32ToString,
                CliCompatibilityForwardingKind.NullableStringToRequiredString) =>
                CliCompatibilityForwardingKind.NullableInt32ToRequiredString,
            (CliCompatibilityForwardingKind.NullableInt32ToString,
                CliCompatibilityForwardingKind.ScalarToCollection) =>
                CliCompatibilityForwardingKind.NullableInt32ToStringCollection,
            (CliCompatibilityForwardingKind.NullableBooleanToString,
                CliCompatibilityForwardingKind.ScalarToCollection) =>
                CliCompatibilityForwardingKind.NullableBooleanToStringCollection,
            (CliCompatibilityForwardingKind.NullableInt32ToString,
                CliCompatibilityForwardingKind.NullableStringToCliOptionValue) =>
                CliCompatibilityForwardingKind.NullableInt32ToCliOptionValue,
            _ => throw new InvalidOperationException(
                $"Compatibility property '{property.PropertyName}' has unsupported composed forwarding "
                + $"conversions {first} and {second}."),
        };

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

        if (IsBooleanType(baseline.CSharpType))
        {
            if (!baseline.CSharpType.Equals("bool?", StringComparison.Ordinal))
            {
                return null;
            }

            return replacement.CSharpType switch
            {
                "string?" => CliCompatibilityForwardingKind.NullableBooleanToString,
                "IEnumerable<string>?" => CliCompatibilityForwardingKind.NullableBooleanToStringCollection,
                _ => null,
            };
        }

        if (!baseline.CSharpType.Equals("string?", StringComparison.Ordinal))
        {
            return null;
        }

        return replacement.CSharpType switch
        {
            "string" => CliCompatibilityForwardingKind.NullableStringToRequiredString,
            "IEnumerable<string>?" => CliCompatibilityForwardingKind.ScalarToCollection,
            "CliOptionValue?" => CliCompatibilityForwardingKind.NullableStringToCliOptionValue,
            _ => null,
        };
    }

    private static IReadOnlyList<GeneratedApiProperty> RemoveUnsafeBooleanStringAliases(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IReadOnlyList<CliOptionDefinition> options) =>
        [.. baselineProperties.Where(property =>
            !IsUnsafeBooleanStringAlias(property, baselineProperties, options))];

    private static bool IsUnsafeBooleanStringAlias(
        GeneratedApiProperty property,
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IReadOnlyList<CliOptionDefinition> options)
    {
        if (!property.IsCompatibility
            || property.ForwardingKind is not (CliCompatibilityForwardingKind.Direct
                or CliCompatibilityForwardingKind.NullableBooleanToString
                or CliCompatibilityForwardingKind.NullableBooleanToStringCollection))
        {
            return false;
        }

        var target = FindForwardingTargetBaseline(baselineProperties, property);
        var replacement = target is null
            ? null
            : options.Select(ToGeneratedProperty)
                .FirstOrDefault(option => HasSameCliIdentity(option, target));
        return IsUnsafeBooleanStringChange(property, replacement, options);
    }

    private static bool IsUnsafeBooleanStringChange(
        GeneratedApiProperty baseline,
        GeneratedApiProperty? replacement,
        IReadOnlyList<CliOptionDefinition> options)
    {
        if (replacement is null
            || !IsBooleanType(baseline.CSharpType)
            || baseline.IsFlag == false
            || replacement.CSharpType is not ("string?" or "IEnumerable<string>?"))
        {
            return false;
        }

        var option = options.FirstOrDefault(candidate =>
            candidate.PropertyName.Equals(replacement.PropertyName, StringComparison.Ordinal)
            && HasSameCliIdentity(ToGeneratedProperty(candidate), replacement));
        // A former named flag cannot safely forward into a positional string replacement.
        return option is null || !ExplicitlyAcceptsBooleanText(option);
    }

    private static bool ExplicitlyAcceptsBooleanText(CliOptionDefinition option) =>
        CliScraperBase.HelpDeclaresExplicitBooleanValue(option.Description ?? string.Empty);

    private static bool IsBooleanType(string cSharpType) => cSharpType is "bool" or "bool?";

    private static void PreserveCompatibilityProperty(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        ICollection<string> violations) =>
        PreserveCompatibilityProperty(
            command,
            ToCompatibilityProperty(baseline),
            compatibilityProperties,
            violations);

    private static CliCompatibilityProperty ToCompatibilityProperty(GeneratedApiProperty baseline) =>
        new()
        {
            PropertyName = baseline.PropertyName,
            CSharpType = baseline.CSharpType,
            ForwardToPropertyName = baseline.ForwardToPropertyName,
            UseInitAccessor = baseline.UseInitAccessor,
            ForwardingKind = baseline.ForwardingKind,
            ObsoleteMessage = baseline.ObsoleteMessage
                ?? $"{baseline.PropertyName} is retained for compatibility.",
        };

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
                     StringComparison.Ordinal)
                 && !CanActivateVirtualDispatchAlias(command, expected, supplied))
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

    private static bool CanActivateVirtualDispatchAlias(
        CliCommandDefinition command,
        CliCompatibilityProperty expected,
        CliCompatibilityProperty supplied) =>
        expected.ForwardToPropertyName is null
        && supplied.ForwardToPropertyName is { } targetName
        && supplied.ForwardingKind == CliCompatibilityForwardingKind.Direct
        && !supplied.UseInitAccessor
        && command.Options.Any(option =>
            option.PropertyName.Equals(targetName, StringComparison.Ordinal)
            && option.PropertyType.Equals(supplied.CSharpType, StringComparison.Ordinal));

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

        if (!HasCompatibleCliIdentity(current, baseline)
            && !AllowsRenderingPhaseMigration(command, current, baseline))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed CLI switch or argument position");
        }
    }

    private static bool AllowsRenderingPhaseMigration(
        CliCommandDefinition command,
        GeneratedApiProperty current,
        GeneratedApiProperty baseline) =>
        current.ArgumentPosition is not null
        && baseline.ArgumentPosition is not null
        && (!baseline.PrependOptionTerminator || current.PrependOptionTerminator)
        && (!baseline.RepeatOptionTerminator || current.RepeatOptionTerminator)
        && (!baseline.PrependOptionTerminatorIfValueStartsWithDash
            || current.PrependOptionTerminatorIfValueStartsWithDash)
        && command.PositionalArguments.Any(argument =>
            argument.AllowRenderingPhaseMigrationFromBaseline
            && argument.PropertyName.Equals(current.PropertyName, StringComparison.Ordinal));

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
            var positionalBaseline = baseline.ArgumentPosition is not null && baseline.Phase is null
                ? baseline with { Phase = CommandLinePhase.Passthrough }
                : baseline;
            var propertyNames = positionalArguments.Select(argument => argument.PropertyName)
                .Concat(options.Select(option => option.PropertyName));
            var positionalResult = TryRestoreRequiredMember(
                positionalBaseline,
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
            [.. GetCurrentProperties(positionalArguments, options).Where(static property => property.IsRequired)],
            compatibilityConstructors);

    private static void PreserveCompatibilityConstructors(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IReadOnlyList<CliCompatibilityConstructor> baselineConstructors,
        IReadOnlyList<GeneratedApiProperty> currentRequired,
        List<CliCompatibilityConstructor> compatibilityConstructors)
    {
        if (currentRequired.Count == 0)
        {
            compatibilityConstructors.Clear();
            return;
        }

        var baselineRequired = baselineProperties
            .Where(static property => property.IsRequired && !property.IsCompatibility)
            .ToArray();
        foreach (var constructor in baselineConstructors)
        {
            AddCompatibilityConstructor(
                compatibilityConstructors,
                RemapPrimaryConstructorArguments(
                    constructor,
                    baselineRequired,
                    currentRequired),
                currentRequired);
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

    private static CliCompatibilityConstructor RemapPrimaryConstructorArguments(
        CliCompatibilityConstructor constructor,
        IReadOnlyList<GeneratedApiProperty> baselineRequired,
        IReadOnlyList<GeneratedApiProperty> currentRequired) =>
        constructor with
        {
            PrimaryConstructorArguments = [.. currentRequired
                .Select(current => GetPreservedPrimaryConstructorArgument(
                    constructor,
                    baselineRequired,
                    current))],
        };

    private static string GetPreservedPrimaryConstructorArgument(
        CliCompatibilityConstructor constructor,
        IReadOnlyList<GeneratedApiProperty> baselineRequired,
        GeneratedApiProperty current)
    {
        var currentContract = GetConstructorParameterContract(current);
        var baselineIndex = -1;
        for (var index = 0; index < baselineRequired.Count; index++)
        {
            if (GetConstructorParameterContract(baselineRequired[index]) == currentContract)
            {
                baselineIndex = index;
                break;
            }
        }

        return baselineIndex >= 0 && baselineIndex < constructor.PrimaryConstructorArguments.Count
            ? constructor.PrimaryConstructorArguments[baselineIndex]
            : GetTypedDefault(current.CSharpType);
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
            PrimaryConstructorArguments = [.. constructor.PrimaryConstructorArguments
                .Select((argument, index) => argument.Equals("default!", StringComparison.Ordinal)
                                             && index < currentRequired.Count
                    ? GetTypedDefault(currentRequired[index].CSharpType)
                    : argument)],
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
        Dictionary<string, string> renamedProperties,
        IReadOnlyDictionary<string, string> copiedProperties)
    {
        if (renamedProperties.Count == 0 && copiedProperties.Count == 0)
        {
            return values;
        }

        var updated = values
            .Where(pair => !renamedProperties.ContainsKey(pair.Key))
            .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal);
        foreach (var value in values.Where(pair => renamedProperties.ContainsKey(pair.Key)))
        {
            updated.TryAdd(renamedProperties[value.Key], value.Value);
        }

        foreach (var copy in copiedProperties)
        {
            if (!values.TryGetValue(copy.Key, out var value))
            {
                continue;
            }

            updated.TryAdd(copy.Key, value);
            updated.TryAdd(copy.Value, value);
        }

        return updated;
    }

    private static GeneratedApiProperty[] GetCurrentProperties(
        IEnumerable<CliPositionalArgument> positionalArguments,
        IEnumerable<CliOptionDefinition> options) =>
        [.. options.Select(ToGeneratedProperty)
, .. positionalArguments.Select(ToGeneratedProperty)];

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
            UseInitAccessor: argument.IsRequired,
            Phase: argument.Phase,
            PrependOptionTerminator: argument.PrependOptionTerminator,
            RepeatOptionTerminator: argument.RepeatOptionTerminator,
            PrependOptionTerminatorIfValueStartsWithDash:
                argument.PrependOptionTerminatorIfValueStartsWithDash);

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
            return HasSamePositionalIdentity(left, right);
        }

        if (left.SwitchName is not null || right.SwitchName is not null)
        {
            return HasSameOptionIdentity(left.SwitchName, right.SwitchName);
        }

        return true;
    }

    private static bool HasSameOptionIdentity(string? left, string? right) =>
        left is not null
        && right is not null
        && NormalizeCliSwitchIdentity(left).Equals(
            NormalizeCliSwitchIdentity(right),
            StringComparison.Ordinal);

    private static bool HasCompatibleCliIdentity(
        GeneratedApiProperty current,
        GeneratedApiProperty baseline)
    {
        if (HasSameCliIdentity(current, baseline))
        {
            return true;
        }

        return current.ArgumentPosition is not null
               && baseline.ArgumentPosition is not null
               && current.ArgumentPosition == baseline.ArgumentPosition
               && (current.Phase == baseline.Phase || baseline.Phase is null)
               && (!baseline.PrependOptionTerminator || current.PrependOptionTerminator)
               && (!baseline.RepeatOptionTerminator || current.RepeatOptionTerminator)
               && (!baseline.PrependOptionTerminatorIfValueStartsWithDash
                   || current.PrependOptionTerminatorIfValueStartsWithDash);
    }

    private static bool HasSamePositionalIdentity(
        GeneratedApiProperty left,
        GeneratedApiProperty right)
    {
        var phasesMatch = left.Phase is null
                          || right.Phase is null
                          || left.Phase == right.Phase;
        return left.ArgumentPosition == right.ArgumentPosition
               && phasesMatch
               && left.PrependOptionTerminator == right.PrependOptionTerminator
               && left.RepeatOptionTerminator == right.RepeatOptionTerminator
               && left.PrependOptionTerminatorIfValueStartsWithDash
               == right.PrependOptionTerminatorIfValueStartsWithDash;
    }

    private static Dictionary<string, GeneratedApiBaseline> ReadBaseline(
        string optionsDirectory)
    {
        var baseline = new Dictionary<string, GeneratedApiBaseline>(StringComparer.Ordinal);
        foreach (var path in Directory.EnumerateFiles(
                     optionsDirectory,
                     "*.cs",
                     SearchOption.TopDirectoryOnly)
                     .Where(IsGeneratedBaselineFile))
        {
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
            foreach (var declaration in root.DescendantNodes().OfType<RecordDeclarationSyntax>())
            {
                baseline[declaration.Identifier.ValueText] = ReadBaseline(declaration);
            }
        }

        return baseline;
    }

    private static Dictionary<string, GeneratedApiBaseline> FilterToShippedTypes(
        Dictionary<string, GeneratedApiBaseline> baseline,
        string outputDirectory,
        CliToolDefinition tool)
    {
        var shippedApiPath = Path.Combine(
            outputDirectory,
            tool.OutputDirectory,
            "PublicAPI.Shipped.txt");
        if (!File.Exists(shippedApiPath))
        {
            return baseline;
        }

        var shippedApi = File.ReadLines(shippedApiPath).ToHashSet(StringComparer.Ordinal);
        var optionsNamespace = $"{tool.TargetNamespace}.Options.";
        return baseline
            .Where(pair => shippedApi.Contains(optionsNamespace + pair.Key))
            .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal);
    }

    private static GeneratedApiBaseline? ReadBaseline(
        string optionsDirectory,
        string className)
    {
        var path = new[]
            {
                Path.Combine(optionsDirectory, $"{className}.Generated.cs"),
                Path.Combine(optionsDirectory, $"{className}.cs"),
            }
            .FirstOrDefault(candidate => File.Exists(candidate) && IsGeneratedBaselineFile(candidate));
        if (path is null)
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
                     "*.cs",
                     SearchOption.TopDirectoryOnly)
                     .Where(IsGeneratedBaselineFile))
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
                Values = [.. pair.Value.Values
                    .Concat(currentValues)
                    .DistinctBy(static value => value.CliValue, StringComparer.Ordinal)],
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
        [.. declaration.Members
            .OfType<ConstructorDeclarationSyntax>()
            .Where(constructor => constructor.Modifiers.Any(SyntaxKind.PublicKeyword))
            .Where(constructor => constructor.Initializer?.IsKind(
                SyntaxKind.ThisConstructorInitializer) == true)
            .Select(constructor => ReadCompatibilityConstructor(declaration, constructor))];

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
            PrimaryConstructorArguments = [.. constructor.Initializer!.ArgumentList.Arguments.Select(argument => argument.Expression.ToString())],
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
        string namespacePrefix,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline)
    {
        if (!Directory.Exists(servicesDirectory))
        {
            return [];
        }

        var sources = Directory.EnumerateFiles(
                servicesDirectory,
                "*.cs",
                SearchOption.TopDirectoryOnly)
            .Where(IsGeneratedBaselineFile)
            .Select(File.ReadAllText);
        return [.. ReadFacadeMethods(sources, targetNamespace).Where(method => IsFacadeOwnedByTool(method, namespacePrefix, baseline))];
    }

    private static bool IsFacadeOwnedByTool(
        GeneratedFacadeMethod method,
        string namespacePrefix,
        IReadOnlyDictionary<string, GeneratedApiBaseline> baseline) =>
        method.OptionsType.Equals($"{namespacePrefix}Options", StringComparison.Ordinal)
        || (baseline.TryGetValue(method.OptionsType, out var optionsBaseline)
            && HasToolOptionsAncestor(optionsBaseline, namespacePrefix, baseline));

    private static bool IsGeneratedBaselineFile(string path)
    {
        if (path.EndsWith(".Generated.cs", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        using var reader = new StreamReader(path);
        for (var lineNumber = 0; lineNumber < 5 && !reader.EndOfStream; lineNumber++)
        {
            if (reader.ReadLine() is { } line && GeneratorUtils.ContainsAutoGeneratedMarker(line))
            {
                return true;
            }
        }

        return false;
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
                         .Where(IsPublicFacadeMethod))
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

    private static bool IsPublicFacadeMethod(MethodDeclarationSyntax method) =>
        method.Modifiers.Any(SyntaxKind.PublicKeyword)
        || (method.Parent is InterfaceDeclarationSyntax
            && method.Modifiers.Count == 0);

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
        var cliOption = FindAttribute(attributes, "CliOption");
        var cliFlag = FindAttribute(attributes, "CliFlag");
        var cliSwitch = cliOption ?? cliFlag;
        var secretValue = FindAttribute(attributes, "SecretValue");
        var range = FindAttribute(attributes, "Range")
                    ?? FindAttribute(attributes, "CliOptionValueRange");
        var regularExpression = FindAttribute(attributes, "RegularExpression")
                                ?? FindAttribute(attributes, "CliOptionValueRegularExpression");
        var obsolete = FindAttribute(attributes, "Obsolete");
        var (targetPropertyName, forwardingKind) = GetForwarding(accessorList);
        bool? isFlag = cliSwitch is null ? null : cliFlag is not null;

        return new GeneratedApiProperty(
            propertyName,
            cSharpType,
            GetStringArgument(cliSwitch),
            GetIntegerArgument(cliArgument),
            isRequired || GetBooleanNamedArgument(cliArgument, "Required"),
            obsolete is not null,
            targetPropertyName,
            GetStringArgument(obsolete),
            isRequired
            || accessorList?.Accessors.Any(static accessor =>
                accessor.IsKind(SyntaxKind.InitAccessorDeclaration)) == true,
            forwardingKind,
            isFlag,
            GetStringNamedArgument(cliSwitch, "ShortForm"),
            GetBooleanNamedArgument(cliSwitch, "PreferShortForm"),
            GetOptionValueSeparator(cliOption),
            GetEnumNamedArgument(cliOption, "ValueArity", CliOptionValueArity.Required),
            GetBooleanNamedArgument(cliOption, "GroupValues"),
            GetNullableEnumNamedArgument<CommandLinePhase>(cliSwitch, "Phase")
            ?? GetNullableEnumNamedArgument<CommandLinePhase>(cliArgument, "Phase"),
            GetBooleanNamedArgument(cliArgument, "PrependOptionTerminator"),
            GetBooleanNamedArgument(cliArgument, "RepeatOptionTerminator"),
            GetBooleanNamedArgument(cliArgument, "PrependOptionTerminatorIfValueStartsWithDash"),
            secretValue is not null,
            GetStringArguments(secretValue),
            GetValidationConstraints(range, regularExpression));
    }

    private static CliValidationConstraints? GetValidationConstraints(
        AttributeSyntax? range,
        AttributeSyntax? regularExpression)
    {
        var minValue = GetIntegerArgument(range);
        var maxValue = GetIntegerArgument(range, 1);
        var pattern = GetStringArgument(regularExpression);
        return minValue is null && maxValue is null && pattern is null
            ? null
            : new CliValidationConstraints
            {
                MinValue = minValue,
                MaxValue = maxValue,
                Pattern = pattern,
            };
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

    private static int? GetIntegerArgument(AttributeSyntax? attribute, int index = 0) =>
        attribute?.ArgumentList?.Arguments.ElementAtOrDefault(index)?.Expression is LiteralExpressionSyntax literal
            && literal.IsKind(SyntaxKind.NumericLiteralExpression)
            && literal.Token.Value is int value
                ? value
                : null;

    private static string[]? GetStringArguments(AttributeSyntax? attribute)
    {
        if (attribute is null)
        {
            return null;
        }

        return
        [
            .. attribute.ArgumentList?.Arguments
                .Select(static argument => argument.Expression)
                .OfType<LiteralExpressionSyntax>()
                .Where(static literal => literal.IsKind(SyntaxKind.StringLiteralExpression))
                .Select(static literal => literal.Token.ValueText)
                ?? [],
        ];
    }

    private static string? GetStringNamedArgument(AttributeSyntax? attribute, string name) =>
        FindNamedArgument(attribute, name)?.Expression is LiteralExpressionSyntax literal
        && literal.IsKind(SyntaxKind.StringLiteralExpression)
            ? literal.Token.ValueText
            : null;

    private static bool GetBooleanNamedArgument(AttributeSyntax? attribute, string name) =>
        FindNamedArgument(attribute, name)?.Expression.Kind() == SyntaxKind.TrueLiteralExpression;

    private static TEnum GetEnumNamedArgument<TEnum>(
        AttributeSyntax? attribute,
        string name,
        TEnum defaultValue)
        where TEnum : struct, Enum =>
        GetNullableEnumNamedArgument<TEnum>(attribute, name) ?? defaultValue;

    private static string GetOptionValueSeparator(AttributeSyntax? attribute) =>
        GetNamedEnumMember(attribute, "Format") switch
        {
            "EqualsSeparated" => "=",
            "ColonSeparated" => ":",
            "NoSeparator" => string.Empty,
            _ => " ",
        };

    private static TEnum? GetNullableEnumNamedArgument<TEnum>(
        AttributeSyntax? attribute,
        string name)
        where TEnum : struct, Enum
    {
        var memberName = GetNamedEnumMember(attribute, name);
        return Enum.TryParse<TEnum>(memberName, out var value) ? value : null;
    }

    private static string? GetNamedEnumMember(AttributeSyntax? attribute, string name)
    {
        var expression = FindNamedArgument(attribute, name)?.Expression;
        return expression switch
        {
            MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.ValueText,
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            _ => null,
        };
    }

    private static AttributeArgumentSyntax? FindNamedArgument(
        AttributeSyntax? attribute,
        string name) =>
        attribute?.ArgumentList?.Arguments.FirstOrDefault(argument =>
            argument.NameEquals?.Name.Identifier.ValueText.Equals(name, StringComparison.Ordinal) == true);

    private static (string? TargetPropertyName, CliCompatibilityForwardingKind Kind) GetForwarding(
        AccessorListSyntax? accessorList)
    {
        var expression = accessorList?.Accessors
            .FirstOrDefault(accessor => accessor.IsKind(SyntaxKind.GetAccessorDeclaration))?
            .ExpressionBody?.Expression;
        var setterExpression = accessorList?.Accessors
            .FirstOrDefault(accessor =>
                accessor.IsKind(SyntaxKind.SetAccessorDeclaration)
                || accessor.IsKind(SyntaxKind.InitAccessorDeclaration))?
            .ExpressionBody?.Expression;
        if (expression is IdentifierNameSyntax identifier)
        {
            if (AssignsCoalescedStringEmpty(setterExpression, identifier.Identifier.ValueText))
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

        if (expression is ConditionalAccessExpressionSyntax
            {
                Expression: IdentifierNameSyntax optionValue,
                WhenNotNull: MemberBindingExpressionSyntax optionValueMember,
            }
            && optionValueMember.Name.Identifier.ValueText.Equals("Value", StringComparison.Ordinal))
        {
            return (
                optionValue.Identifier.ValueText,
                CliCompatibilityForwardingKind.NullableStringToCliOptionValue);
        }

        if (TryGetNullableInt32Forwarding(expression, setterExpression, out var forwarding))
        {
            return forwarding;
        }

        if (TryGetNullableBooleanToLocalBackendForwarding(setterExpression, out forwarding)
            || TryGetNullableBooleanForwarding(expression, out forwarding))
        {
            return forwarding;
        }

        return (null, CliCompatibilityForwardingKind.Direct);
    }

    private static bool TryGetNullableInt32Forwarding(
        ExpressionSyntax? expression,
        ExpressionSyntax? setterExpression,
        out (string? TargetPropertyName, CliCompatibilityForwardingKind Kind) forwarding)
    {
        if (expression is not ConditionalExpressionSyntax
            {
                Condition: InvocationExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax
                    {
                        Expression: PredefinedTypeSyntax
                        {
                            Keyword.RawKind: (int) SyntaxKind.IntKeyword,
                        },
                        Name.Identifier.ValueText: "TryParse",
                    },
                    ArgumentList.Arguments: { Count: > 0 } arguments,
                },
            })
        {
            forwarding = default;
            return false;
        }

        if (arguments[0].Expression is IdentifierNameSyntax stringValue)
        {
            forwarding = (
                stringValue.Identifier.ValueText,
                AssignsCoalescedStringEmpty(setterExpression, stringValue.Identifier.ValueText)
                    ? CliCompatibilityForwardingKind.NullableInt32ToRequiredString
                    : CliCompatibilityForwardingKind.NullableInt32ToString);
            return true;
        }

        if (arguments[0].Expression is ConditionalAccessExpressionSyntax
            {
                Expression: IdentifierNameSyntax collectionTarget,
                WhenNotNull: InvocationExpressionSyntax
                {
                    Expression: MemberBindingExpressionSyntax collectionMember,
                },
            }
            && collectionMember.Name.Identifier.ValueText.Equals("FirstOrDefault", StringComparison.Ordinal))
        {
            forwarding = (
                collectionTarget.Identifier.ValueText,
                CliCompatibilityForwardingKind.NullableInt32ToStringCollection);
            return true;
        }

        if (arguments[0].Expression is ConditionalAccessExpressionSyntax
            {
                Expression: IdentifierNameSyntax optionValueTarget,
                WhenNotNull: MemberBindingExpressionSyntax optionValueMember,
            }
            && optionValueMember.Name.Identifier.ValueText.Equals("Value", StringComparison.Ordinal))
        {
            forwarding = (
                optionValueTarget.Identifier.ValueText,
                CliCompatibilityForwardingKind.NullableInt32ToCliOptionValue);
            return true;
        }

        forwarding = default;
        return false;
    }

    private static bool TryGetNullableBooleanForwarding(
        ExpressionSyntax? expression,
        out (string? TargetPropertyName, CliCompatibilityForwardingKind Kind) forwarding)
    {
        if (expression is not ConditionalExpressionSyntax
            {
                Condition: InvocationExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax
                    {
                        Expression: PredefinedTypeSyntax
                        {
                            Keyword.RawKind: (int) SyntaxKind.BoolKeyword,
                        },
                        Name.Identifier.ValueText: "TryParse",
                    },
                    ArgumentList.Arguments: { Count: > 0 } arguments,
                },
            })
        {
            forwarding = default;
            return false;
        }

        if (arguments[0].Expression is IdentifierNameSyntax stringValue)
        {
            forwarding = (
                stringValue.Identifier.ValueText,
                CliCompatibilityForwardingKind.NullableBooleanToString);
            return true;
        }

        if (arguments[0].Expression is ConditionalAccessExpressionSyntax
            {
                Expression: IdentifierNameSyntax collectionTarget,
                WhenNotNull: InvocationExpressionSyntax
                {
                    Expression: MemberBindingExpressionSyntax collectionMember,
                },
            }
            && collectionMember.Name.Identifier.ValueText.Equals("FirstOrDefault", StringComparison.Ordinal))
        {
            forwarding = (
                collectionTarget.Identifier.ValueText,
                CliCompatibilityForwardingKind.NullableBooleanToStringCollection);
            return true;
        }

        forwarding = default;
        return false;
    }

    private static bool TryGetNullableBooleanToLocalBackendForwarding(
        ExpressionSyntax? setterExpression,
        out (string? TargetPropertyName, CliCompatibilityForwardingKind Kind) forwarding)
    {
        if (setterExpression is AssignmentExpressionSyntax
            {
                Left: IdentifierNameSyntax target,
                Right: ConditionalExpressionSyntax
                {
                    Condition: BinaryExpressionSyntax
                    {
                        Left: IdentifierNameSyntax value,
                        Right: LiteralExpressionSyntax { RawKind: (int) SyntaxKind.TrueLiteralExpression },
                        RawKind: (int) SyntaxKind.EqualsExpression,
                    },
                    WhenTrue: LiteralExpressionSyntax localBackend,
                    WhenFalse: LiteralExpressionSyntax { RawKind: (int) SyntaxKind.NullLiteralExpression },
                },
            }
            && value.Identifier.ValueText.Equals("value", StringComparison.Ordinal)
            && localBackend.Token.ValueText.Equals("file://~", StringComparison.Ordinal))
        {
            forwarding = (
                target.Identifier.ValueText,
                CliCompatibilityForwardingKind.NullableBooleanToLocalBackendString);
            return true;
        }

        forwarding = default;
        return false;
    }

    private static bool AssignsCoalescedStringEmpty(
        ExpressionSyntax? setterExpression,
        string targetPropertyName) =>
        setterExpression is AssignmentExpressionSyntax
        {
            Left: IdentifierNameSyntax setterTarget,
            Right: BinaryExpressionSyntax
            {
                RawKind: (int) SyntaxKind.CoalesceExpression,
                Left: var forwardedValue,
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
        && setterTarget.Identifier.ValueText.Equals(targetPropertyName, StringComparison.Ordinal)
        && IsForwardedValue(forwardedValue);

    private static bool IsForwardedValue(ExpressionSyntax expression) =>
        expression is IdentifierNameSyntax { Identifier.ValueText: "value" }
        || expression is ConditionalAccessExpressionSyntax
        {
            Expression: IdentifierNameSyntax { Identifier.ValueText: "value" },
        };
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
    CliCompatibilityForwardingKind ForwardingKind = CliCompatibilityForwardingKind.Direct,
    bool? IsFlag = null,
    string? ShortForm = null,
    bool PreferShortForm = false,
    string ValueSeparator = " ",
    CliOptionValueArity ValueArity = CliOptionValueArity.Required,
    bool GroupValues = false,
    CommandLinePhase? Phase = null,
    bool PrependOptionTerminator = false,
    bool RepeatOptionTerminator = false,
    bool PrependOptionTerminatorIfValueStartsWithDash = false,
    bool IsSecret = false,
    string[]? SecretValueKeys = null,
    CliValidationConstraints? ValidationConstraints = null);

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
