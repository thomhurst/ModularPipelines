using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        var baseline = ReadBaseline(optionsDirectory);
        var compatibleTool = baseline.TryGetValue($"{tool.NamespacePrefix}Options", out var globalBaseline)
            ? PreserveGlobalOptions(tool, globalBaseline.Properties)
            : tool;
        var executeFacadeOptionTypes = ReadExecuteFacadeOptionTypes(
            Path.Combine(outputDirectory, tool.OutputDirectory, "Services"));
        return compatibleTool with
        {
            Commands = compatibleTool.Commands
                .Select(command => baseline.TryGetValue(command.ClassName, out var commandBaseline)
                    ? Preserve(command, commandBaseline.Properties, commandBaseline.Constructors)
                    : command)
                .Select(command => executeFacadeOptionTypes.Contains(command.ClassName)
                    ? command with { PreserveExecuteFacade = true }
                    : command)
                .ToArray(),
        };
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

        PreserveCompatibilityConstructors(
            baselineProperties,
            baselineConstructors,
            positionalArguments,
            options,
            compatibilityConstructors);

        if (violations.Count > 0)
        {
            throw new InvalidOperationException(
                $"Generated API compatibility validation failed for {command.FullCommand}:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, violations.Select(violation => $"- {violation}")));
        }

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
        if (replacement is not null
            && !replacement.CSharpType.Equals(baseline.CSharpType, StringComparison.Ordinal))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed type from "
                + $"{baseline.CSharpType} to {replacement.CSharpType} "
                + $"while being renamed to {replacement.PropertyName}");
            return;
        }

        if (baseline.ArgumentPosition is not null && replacement is null)
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} positional argument was removed");
            return;
        }

        if (baseline.IsRequired && replacement is null)
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} was removed from the required constructor");
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
                ObsoleteMessage = $"Use {forwardToPropertyName} instead.",
            });

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
        List<CliCompatibilityConstructor> compatibilityConstructors)
    {
        var currentRequired = GetCurrentProperties(positionalArguments, options)
            .Where(static property => property.IsRequired)
            .ToArray();
        if (currentRequired.Length == 0)
        {
            compatibilityConstructors.Clear();
            return;
        }

        foreach (var constructor in baselineConstructors)
        {
            AddCompatibilityConstructor(compatibilityConstructors, constructor, currentRequired);
        }

        var baselineRequired = baselineProperties
            .Where(static property => property.IsRequired && !property.IsCompatibility)
            .ToArray();
        if (HasSameConstructorSignature(baselineRequired, currentRequired))
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
            },
            currentRequired);
    }

    private static void AddCompatibilityConstructor(
        ICollection<CliCompatibilityConstructor> constructors,
        CliCompatibilityConstructor constructor,
        IReadOnlyList<GeneratedApiProperty> currentRequired)
    {
        if (HasSameConstructorSignature(constructor.Parameters, currentRequired)
            || constructors.Any(existing => HasSameConstructorSignature(
                existing.Parameters,
                constructor.Parameters)))
        {
            return;
        }

        constructors.Add(constructor);
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

        return left.Select(GetConstructorParameterType)
            .SequenceEqual(right.Select(GetConstructorParameterType), StringComparer.Ordinal);
    }

    private static string GetConstructorParameterType<T>(T parameter) => parameter switch
    {
        GeneratedApiProperty property => property.CSharpType,
        CliCompatibilityConstructorParameter compatibilityParameter => compatibilityParameter.CSharpType,
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
        positionalArguments.Select(ToGeneratedProperty)
            .Concat(options.Select(ToGeneratedProperty))
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

    private static CliCompatibilityConstructor[] ReadCompatibilityConstructors(
        RecordDeclarationSyntax declaration) =>
        declaration.Members
            .OfType<ConstructorDeclarationSyntax>()
            .Where(constructor => constructor.Modifiers.Any(SyntaxKind.PublicKeyword))
            .Where(constructor => constructor.Initializer?.IsKind(
                SyntaxKind.ThisConstructorInitializer) == true)
            .Select(constructor => new CliCompatibilityConstructor
            {
                Parameters = constructor.ParameterList.Parameters
                    .Select(parameter => new CliCompatibilityConstructorParameter(
                        parameter.Identifier.ValueText,
                        parameter.Type?.ToString() ?? string.Empty))
                    .ToArray(),
                PrimaryConstructorArguments = constructor.Initializer!.ArgumentList.Arguments
                    .Select(argument => argument.Expression.ToString())
                    .ToArray(),
            })
            .ToArray();

    private static HashSet<string> ReadExecuteFacadeOptionTypes(string servicesDirectory)
    {
        var optionTypes = new HashSet<string>(StringComparer.Ordinal);
        if (!Directory.Exists(servicesDirectory))
        {
            return optionTypes;
        }

        foreach (var path in Directory.EnumerateFiles(
                     servicesDirectory,
                     "*.Generated.cs",
                     SearchOption.TopDirectoryOnly))
        {
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
            foreach (var method in root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                         .Where(method => method.Identifier.ValueText.Equals(
                             "ExecuteAsync",
                             StringComparison.Ordinal))
                         .Where(method => method.Modifiers.Any(SyntaxKind.PublicKeyword)))
            {
                var optionsType = method.ParameterList.Parameters.FirstOrDefault()?.Type?.ToString();
                if (!string.IsNullOrWhiteSpace(optionsType))
                {
                    optionTypes.Add(optionsType.TrimEnd('?'));
                }
            }
        }

        return optionTypes;
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
            GetStringArgument(obsolete));
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
    string? ObsoleteMessage);

internal sealed record GeneratedApiBaseline(
    IReadOnlyList<GeneratedApiProperty> Properties,
    IReadOnlyList<CliCompatibilityConstructor> Constructors);
