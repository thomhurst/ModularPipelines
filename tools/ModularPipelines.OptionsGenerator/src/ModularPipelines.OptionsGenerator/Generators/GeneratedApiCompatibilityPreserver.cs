using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

internal static class GeneratedApiCompatibilityPreserver
{
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
        var compatibleTool = baseline.TryGetValue($"{tool.NamespacePrefix}Options", out var globalProperties)
            ? PreserveGlobalOptions(tool, globalProperties)
            : tool;
        return compatibleTool with
        {
            Commands = compatibleTool.Commands
                .Select(command => baseline.TryGetValue(command.ClassName, out var properties)
                    ? Preserve(command, properties)
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
        IReadOnlyList<GeneratedApiProperty> baselineProperties)
    {
        var compatibilityProperties = command.CompatibilityProperties.ToList();
        var positionalArguments = command.PositionalArguments.ToArray();
        var options = command.Options.ToArray();

        RestoreRequiredMemberNames(
            baselineProperties,
            positionalArguments,
            options,
            compatibilityProperties);

        var currentProperties = GetCurrentProperties(positionalArguments, options);
        var violations = new List<string>();
        foreach (var baseline in baselineProperties)
        {
            PreserveBaselineProperty(
                command,
                baseline,
                currentProperties,
                compatibilityProperties,
                violations);
        }

        AddNewRequiredMemberViolations(command, baselineProperties, currentProperties, violations);

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
        else if (!baseline.IsRequired && current.IsRequired)
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed from optional to required");
        }
        else if (!HasSameCliIdentity(current, baseline))
        {
            violations.Add(
                $"{command.ClassName}.{baseline.PropertyName} changed CLI switch or argument position");
        }
    }

    private static void AddNewRequiredMemberViolations(
        CliCommandDefinition command,
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        IEnumerable<GeneratedApiProperty> currentProperties,
        List<string> violations)
    {
        foreach (var current in currentProperties.Where(static property => property.IsRequired))
        {
            var existedInBaseline = baselineProperties.Any(baseline =>
                !baseline.IsCompatibility
                && baseline.PropertyName.Equals(current.PropertyName, StringComparison.Ordinal));
            if (!existedInBaseline)
            {
                violations.Add(
                    $"{command.ClassName}.{current.PropertyName} was added to the required constructor");
            }
        }
    }

    private static void RestoreRequiredMemberNames(
        IReadOnlyList<GeneratedApiProperty> baselineProperties,
        CliPositionalArgument[] positionalArguments,
        CliOptionDefinition[] options,
        ICollection<CliCompatibilityProperty> compatibilityProperties)
    {
        foreach (var baseline in baselineProperties.Where(property =>
                     property.IsRequired && !property.IsCompatibility))
        {
            var positionalIndex = Array.FindIndex(positionalArguments, current =>
                current.IsRequired
                && HasSameCliIdentity(ToGeneratedProperty(current), baseline)
                && ToGeneratedProperty(current).CSharpType.Equals(
                    baseline.CSharpType,
                    StringComparison.Ordinal));
            if (positionalIndex >= 0
                && !positionalArguments[positionalIndex].PropertyName.Equals(
                    baseline.PropertyName,
                    StringComparison.Ordinal))
            {
                var currentName = positionalArguments[positionalIndex].PropertyName;
                positionalArguments[positionalIndex] = positionalArguments[positionalIndex] with
                {
                    PropertyName = baseline.PropertyName,
                };
                AddRenamedCurrentProperty(
                    compatibilityProperties,
                    currentName,
                    baseline.CSharpType,
                    baseline.PropertyName);
                continue;
            }

            var optionIndex = Array.FindIndex(options, current =>
                current.IsRequired
                && HasSameCliIdentity(ToGeneratedProperty(current), baseline)
                && ToGeneratedProperty(current).CSharpType.Equals(
                    baseline.CSharpType,
                    StringComparison.Ordinal));
            if (optionIndex < 0
                || options[optionIndex].PropertyName.Equals(baseline.PropertyName, StringComparison.Ordinal))
            {
                continue;
            }

            var optionName = options[optionIndex].PropertyName;
            options[optionIndex] = options[optionIndex] with { PropertyName = baseline.PropertyName };
            AddRenamedCurrentProperty(
                compatibilityProperties,
                optionName,
                baseline.CSharpType,
                baseline.PropertyName);
        }
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

    private static Dictionary<string, IReadOnlyList<GeneratedApiProperty>> ReadBaseline(
        string optionsDirectory)
    {
        var baseline = new Dictionary<string, IReadOnlyList<GeneratedApiProperty>>(StringComparer.Ordinal);
        foreach (var path in Directory.EnumerateFiles(
                     optionsDirectory,
                     "*.Generated.cs",
                     SearchOption.TopDirectoryOnly))
        {
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
            foreach (var declaration in root.DescendantNodes().OfType<RecordDeclarationSyntax>())
            {
                baseline[declaration.Identifier.ValueText] = ReadProperties(declaration);
            }
        }

        return baseline;
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
