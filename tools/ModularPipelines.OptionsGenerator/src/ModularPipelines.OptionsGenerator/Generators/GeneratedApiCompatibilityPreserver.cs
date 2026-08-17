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
        var preservedTool = compatibleTool with
        {
            Commands = compatibleTool.Commands
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
                .ToArray(),
        };
        RejectRemovedFacadeMethods(preservedTool, facadeMethods);
        return preservedTool;
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
        var aliasEnumName = GeneratorUtils.GetEnumTypeName(baselineProperty.CSharpType);
        if (!enumBaseline.ContainsKey(aliasEnumName)
            || currentAliasProperties.Contains((baselineProperty.PropertyName, baselineProperty.CSharpType))
            || compatibilityProperties.Any(existing => existing.PropertyName.Equals(
                baselineProperty.PropertyName,
                StringComparison.Ordinal)))
        {
            return null;
        }

        var canonicalProperty = canonicalProperties.FirstOrDefault(property =>
            property.PropertyName.Equals(baselineProperty.PropertyName, StringComparison.Ordinal));
        if (canonicalProperty is null
            || !enumBaseline.ContainsKey(GeneratorUtils.GetEnumTypeName(canonicalProperty.CSharpType)))
        {
            return null;
        }

        return new CliAliasCompatibilityProperty
        {
            PropertyName = baselineProperty.PropertyName,
            AliasCSharpType = baselineProperty.CSharpType,
            CanonicalCSharpType = canonicalProperty.CSharpType,
            ObsoleteMessage = baselineProperty.ObsoleteMessage
                ?? $"{baselineProperty.PropertyName} is retained for compatibility.",
        };
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
        var compatibilityProperties = command.CompatibilityProperties.ToList();
        var compatibilityConstructors = command.CompatibilityConstructors.ToList();
        var positionalArguments = command.PositionalArguments.ToArray();
        var options = command.Options.ToArray();
        var violations = new List<string>();
        var renamedProperties = new Dictionary<string, string>(StringComparer.Ordinal);

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
            PreserveBaselineProperty(
                command,
                baseline,
                currentProperties,
                compatibilityProperties,
                violations);
        }

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

    private static void PreserveBaselineProperty(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        IReadOnlyList<GeneratedApiProperty> currentProperties,
        ICollection<CliCompatibilityProperty> compatibilityProperties,
        List<string> violations)
    {
        if (baseline.IsCompatibility)
        {
            PreserveCompatibilityProperty(baseline, compatibilityProperties);
            return;
        }

        var sameName = currentProperties.FirstOrDefault(property =>
            property.PropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal));
        if (sameName is not null)
        {
            ValidateMatchingProperty(command, baseline, sameName, violations);
            return;
        }

        var replacement = currentProperties.FirstOrDefault(property =>
            HasSameCliIdentity(property, baseline));
        if (TryRecordRemovedPropertyViolation(command, baseline, replacement, violations))
        {
            return;
        }

        AddCompatibilityProperty(
            compatibilityProperties,
            new CliCompatibilityProperty
            {
                PropertyName = baseline.PropertyName,
                CSharpType = baseline.CSharpType,
                ForwardToPropertyName = replacement?.PropertyName,
                ObsoleteMessage = replacement is null
                    ? $"{baseline.PropertyName} is no longer supported by the installed CLI and has no effect."
                    : $"Use {replacement.PropertyName} instead.",
            });
    }

    private static bool TryRecordRemovedPropertyViolation(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        GeneratedApiProperty? replacement,
        ICollection<string> violations)
    {
        if (replacement is not null
            && !replacement.CSharpType.Equals(baseline.CSharpType, StringComparison.Ordinal))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed type from "
                + $"{baseline.CSharpType} to {replacement.CSharpType} "
                + $"while being renamed to {replacement.PropertyName}");
            return true;
        }

        if (baseline.ArgumentPosition is not null && replacement is null)
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

    private static void PreserveCompatibilityProperty(
        GeneratedApiProperty baseline,
        ICollection<CliCompatibilityProperty> compatibilityProperties) =>
        AddCompatibilityProperty(
            compatibilityProperties,
            new CliCompatibilityProperty
            {
                PropertyName = baseline.PropertyName,
                CSharpType = baseline.CSharpType,
                ForwardToPropertyName = baseline.ForwardToPropertyName,
                UseInitAccessor = baseline.UseInitAccessor,
                ObsoleteMessage = baseline.ObsoleteMessage
                    ?? $"{baseline.PropertyName} is retained for compatibility.",
            });

    private static void ValidateMatchingProperty(
        CliCommandDefinition command,
        GeneratedApiProperty baseline,
        GeneratedApiProperty current,
        List<string> violations)
    {
        if (!current.CSharpType.Equals(baseline.CSharpType, StringComparison.Ordinal))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed type from "
                + $"{baseline.CSharpType} to {current.CSharpType}");
        }
        else if (baseline.IsRequired && !current.IsRequired)
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed from required to optional");
        }
        else if (!baseline.IsRequired && current.IsRequired)
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed from optional to required "
                + "and would remove its public setter");
        }
        else if (!HasSameCliIdentity(current, baseline))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed CLI switch or argument position");
        }
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
        var addedRequired = currentRequired
            .Where(current => !baselineRequired.Any(baseline =>
                baseline.PropertyName.Equals(current.PropertyName, StringComparison.Ordinal)
                && baseline.CSharpType.Equals(current.CSharpType, StringComparison.Ordinal)))
            .Select(static property => property.PropertyName)
            .ToArray();
        if (currentRequired.Count > baselineRequired.Length && addedRequired.Length > 0)
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
                    : "default!")
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

        var existing = constructors.FirstOrDefault(candidate => HasSameConstructorSignature(
            candidate.Parameters,
            constructor.Parameters));
        if (existing is not null)
        {
            if (constructor.PreserveDeconstruct && !existing.PreserveDeconstruct)
            {
                constructors.Remove(existing);
                constructors.Add(existing with { PreserveDeconstruct = true });
            }

            return;
        }

        constructors.Add(constructor);
    }

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
            null);

    private static GeneratedApiProperty ToGeneratedProperty(CliOptionDefinition option) =>
        new(
            option.PropertyName,
            option.IsRequired ? option.PropertyType.TrimEnd('?') : option.PropertyType,
            option.SwitchName,
            null,
            option.IsRequired,
            false,
            null,
            null);

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
                baseline[declaration.Identifier.ValueText] = new GeneratedApiBaseline(
                    ReadProperties(declaration),
                    ReadCompatibilityConstructors(declaration));
            }
        }

        return baseline;
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

        return new GeneratedApiProperty(
            propertyName,
            cSharpType,
            GetStringArgument(cliOption),
            GetIntegerArgument(cliArgument),
            isRequired,
            obsolete is not null,
            GetForwardTarget(accessorList),
            GetStringArgument(obsolete),
            accessorList?.Accessors.Any(static accessor =>
                accessor.IsKind(SyntaxKind.InitAccessorDeclaration)) == true);
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

    private static string? GetForwardTarget(AccessorListSyntax? accessorList) =>
        accessorList?.Accessors
            .FirstOrDefault(accessor => accessor.IsKind(SyntaxKind.GetAccessorDeclaration))?
            .ExpressionBody?.Expression is IdentifierNameSyntax identifier
                ? identifier.Identifier.ValueText
                : null;
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
    bool UseInitAccessor = false);

internal sealed record GeneratedApiBaseline(
    IReadOnlyList<GeneratedApiProperty> Properties,
    IReadOnlyList<CliCompatibilityConstructor> Constructors);

internal sealed record GeneratedFacadeMethod(
    string DeclaringType,
    string MethodName,
    string OptionsType,
    bool IsOptionsOptional);
