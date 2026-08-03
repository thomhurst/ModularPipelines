using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates strongly-typed C# options classes using the new CLI attribute system.
/// </summary>
public class OptionsClassGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        tool = InheritedPropertyCollisionResolver.Resolve(tool);
        var files = new List<GeneratedFile>();

        foreach (var command in tool.Commands)
        {
            var content = GenerateOptionsClass(command, tool);
            var fileName = $"{command.ClassName}.Generated.cs";
            var relativePath = Path.Combine(tool.OutputDirectory, "Options", fileName);

            files.Add(new GeneratedFile
            {
                RelativePath = relativePath,
                Content = content
            });

            if (command.CommandParts.Length == 0)
            {
                continue;
            }

            foreach (var alias in tool.CommandGroupAliases.Where(alias =>
                         command.CommandParts[0].Equals(
                             alias.CanonicalCommand,
                             StringComparison.OrdinalIgnoreCase)))
            {
                files.Add(GenerateCompatibilityOptionsAlias(command, tool, alias));
            }
        }

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
    }

    private static GeneratedFile GenerateCompatibilityOptionsAlias(
        CliCommandDefinition command,
        CliToolDefinition tool,
        CliCommandGroupAlias alias)
    {
        var aliasClassName = GeneratorUtils.GetAliasedClassName(
            tool,
            alias,
            command.ClassName);
        var requiredParameters = GeneratorUtils.GetRequiredConstructorParameters(command);
        var enumOptions = command.Options
            .Where(option => option.EnumDefinition is not null)
            .ToArray();
        var sb = new StringBuilder();
        GeneratorUtils.GenerateFileHeaderWithNullable(sb, command.DocumentationUrl);
        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
        if (enumOptions.Length > 0)
        {
            sb.AppendLine("using ModularPipelines.Attributes;");
            sb.AppendLine($"using {tool.TargetNamespace}.Enums;");
        }

        sb.AppendLine();
        sb.AppendLine($"namespace {tool.TargetNamespace}.Options;");
        sb.AppendLine();
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine("[ExcludeFromCodeCoverage]");
        if (enumOptions.Length == 0 && requiredParameters.Count == 0)
        {
            sb.AppendLine($"public record {aliasClassName} : {command.ClassName};");
        }
        else
        {
            sb.AppendLine($"public record {aliasClassName} : {command.ClassName}");
            sb.AppendLine("{");
            GenerateCompatibilityConstructor(
                sb,
                aliasClassName,
                requiredParameters,
                tool,
                alias);
            foreach (var option in enumOptions)
            {
                GenerateCompatibilityEnumProperty(sb, option, tool, alias);
            }

            sb.AppendLine("}");
        }

        return new GeneratedFile
        {
            RelativePath = Path.Combine(
                tool.OutputDirectory,
                "Options",
                $"{aliasClassName}.Generated.cs"),
            Content = sb.ToString(),
        };
    }

    private static void GenerateCompatibilityConstructor(
        StringBuilder sb,
        string aliasClassName,
        IReadOnlyList<GeneratorUtils.RequiredConstructorParameter> requiredParameters,
        CliToolDefinition tool,
        CliCommandGroupAlias alias)
    {
        if (requiredParameters.Count == 0)
        {
            return;
        }

        var parameterDeclarations = requiredParameters.Select(parameter =>
            $"        {GetCompatibilityParameterType(parameter, tool, alias)} {parameter.PropertyName}");
        var baseArguments = requiredParameters.Select(parameter =>
            GetCompatibilityBaseArgument(parameter, parameter.PropertyName));
        sb.AppendLine($"    public {aliasClassName}(");
        sb.AppendLine(string.Join($",{Environment.NewLine}", parameterDeclarations));
        sb.AppendLine("    )");
        sb.AppendLine($"        : base({string.Join(", ", baseArguments)})");
        sb.AppendLine("    {");
        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private static string GetCompatibilityParameterType(
        GeneratorUtils.RequiredConstructorParameter parameter,
        CliToolDefinition tool,
        CliCommandGroupAlias alias)
    {
        var type = parameter.CSharpType.TrimEnd('?');
        var canonicalEnumName = parameter.Option?.EnumDefinition?.EnumName;
        if (canonicalEnumName is null)
        {
            return type;
        }

        var aliasEnumName = GeneratorUtils.GetAliasedClassName(tool, alias, canonicalEnumName);
        return type.Replace(canonicalEnumName, aliasEnumName, StringComparison.Ordinal);
    }

    private static string GetCompatibilityBaseArgument(
        GeneratorUtils.RequiredConstructorParameter parameter,
        string parameterName)
    {
        var canonicalEnumName = parameter.Option?.EnumDefinition?.EnumName;
        if (canonicalEnumName is null)
        {
            return parameterName;
        }

        return parameter.CSharpType.Contains("IEnumerable<", StringComparison.Ordinal)
            ? $"{parameterName}.Select(static value => ({canonicalEnumName})(int)value)"
            : $"({canonicalEnumName})(int){parameterName}";
    }

    private static void GenerateCompatibilityEnumProperty(
        StringBuilder sb,
        CliOptionDefinition option,
        CliToolDefinition tool,
        CliCommandGroupAlias alias)
    {
        var canonicalEnumName = option.EnumDefinition!.EnumName;
        var aliasEnumName = GeneratorUtils.GetAliasedClassName(
            tool,
            alias,
            canonicalEnumName);
        var aliasType = option.CSharpType.Replace(
            canonicalEnumName,
            aliasEnumName,
            StringComparison.Ordinal);
        var isEnumerable = option.CSharpType.TrimEnd('?').Equals(
            $"IEnumerable<{canonicalEnumName}>",
            StringComparison.Ordinal);
        var isNullable = option.CSharpType.Equals(
            $"{canonicalEnumName}?",
            StringComparison.Ordinal);

        sb.AppendLine($"    [{GeneratorUtils.GenerateCliAttributeString(option)}]");
        sb.AppendLine($"    public new {aliasType} {option.PropertyName}");
        sb.AppendLine("    {");
        if (isEnumerable)
        {
            GenerateCompatibilityEnumCollectionAccessors(
                sb,
                option,
                canonicalEnumName,
                aliasEnumName);
        }
        else if (isNullable)
        {
            sb.AppendLine($"        get => base.{option.PropertyName} is null");
            sb.AppendLine("            ? null");
            sb.AppendLine($"            : ({aliasEnumName})(int)base.{option.PropertyName}.Value;");
            sb.AppendLine($"        set => base.{option.PropertyName} = value is null");
            sb.AppendLine("            ? null");
            sb.AppendLine($"            : ({canonicalEnumName})(int)value.Value;");
        }
        else
        {
            sb.AppendLine(
                $"        get => ({aliasEnumName})(int)base.{option.PropertyName};");
            sb.AppendLine(
                $"        set => base.{option.PropertyName} = ({canonicalEnumName})(int)value;");
        }

        sb.AppendLine("    }");
    }

    private static void GenerateCompatibilityEnumCollectionAccessors(
        StringBuilder sb,
        CliOptionDefinition option,
        string canonicalEnumName,
        string aliasEnumName)
    {
        var nullableOperator = option.CSharpType.EndsWith(
            "?",
            StringComparison.Ordinal)
            ? "?"
            : string.Empty;
        sb.AppendLine(
            $"        get => base.{option.PropertyName}{nullableOperator}.Select("
            + $"static value => ({aliasEnumName})(int)value);");
        sb.AppendLine(
            $"        set => base.{option.PropertyName} = value{nullableOperator}.Select("
            + $"static value => ({canonicalEnumName})(int)value);");
    }

    private static string GenerateOptionsClass(CliCommandDefinition command, CliToolDefinition tool)
    {
        var sb = new StringBuilder();

        // File header
        GenerateFileHeader(sb, command.DocumentationUrl);

        GenerateUsings(sb, command, tool);

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Options;");
        sb.AppendLine();

        // XML documentation
        GeneratorUtils.GenerateXmlDocumentation(sb, command.Description, "");

        GenerateClassAttributes(sb, command);

        // Class declaration. The returned set contains the names emitted as
        // primary-constructor parameters, so a name scraped as both required and
        // optional can't produce two members (CS0102).
        var positionalArguments = CliPositionalArgument.MergeDuplicates(command.PositionalArguments);
        var existingPropertyNames = GenerateClassDeclaration(sb, command, positionalArguments);

        sb.AppendLine("{");
        GenerateProperties(sb, command, positionalArguments, existingPropertyNames);
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateUsings(StringBuilder sb, CliCommandDefinition command, CliToolDefinition tool)
    {
        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
        sb.AppendLine("using ModularPipelines.Attributes;");

        // Include the existing Options namespace where the base class lives
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");

        if (command.Options.Any(o => o.RequiresModelsNamespace))
        {
            sb.AppendLine("using ModularPipelines.Models;");
        }

        if (command.Options.Any(o => o.ValidationConstraints is not null))
        {
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        }

        // Include enums namespace if any options use enum types
        if (command.Options.Any(o => o.EnumDefinition is not null))
        {
            sb.AppendLine($"using {tool.TargetNamespace}.Enums;");
        }

        sb.AppendLine();
    }

    private static void GenerateClassAttributes(StringBuilder sb, CliCommandDefinition command)
    {
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine("[ExcludeFromCodeCoverage]");

        // CliSubCommand attribute - contains only the subcommand parts (tool name comes from base class)
        if (command.CommandParts.Length > 0)
        {
            var args = string.Join(
                ", ",
                command.CommandParts.Select(GeneratorUtils.FormatStringLiteral));
            sb.AppendLine($"[CliSubCommand({args})]");
        }
    }

    private static void GenerateProperties(
        StringBuilder sb,
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        HashSet<string> existingPropertyNames)
    {
        // Properties for non-required options
        foreach (var option in command.Options.Where(o => !o.IsRequired))
        {
            if (!existingPropertyNames.Add(option.PropertyName))
            {
                continue; // Skip duplicates
            }
            GenerateProperty(sb, option);
            sb.AppendLine();
        }

        // Positional arguments - skip duplicates
        foreach (var positional in positionalArguments.Where(p => !p.IsRequired))
        {
            if (existingPropertyNames.Contains(positional.PropertyName))
            {
                continue; // Skip duplicates
            }
            GeneratePositionalArgument(sb, positional);
            existingPropertyNames.Add(positional.PropertyName);
            sb.AppendLine();
        }

        // Compatibility aliases may intentionally differ from current members only by casing.
        // CLR and C# member names are case-sensitive, unlike scraper duplicate detection.
        var emittedCompatibilityNames = existingPropertyNames.ToHashSet(StringComparer.Ordinal);
        foreach (var compatibilityProperty in command.CompatibilityProperties)
        {
            if (!emittedCompatibilityNames.Add(compatibilityProperty.PropertyName))
            {
                continue;
            }

            GenerateCompatibilityProperty(sb, compatibilityProperty);
            sb.AppendLine();
        }
    }

    private static void GenerateFileHeader(StringBuilder sb, string? documentationUrl)
    {
        GeneratorUtils.GenerateFileHeaderWithNullable(sb, documentationUrl);
    }

    /// <summary>
    /// Emits the class declaration and returns the member names emitted as
    /// primary-constructor parameters, so property emission can skip duplicates.
    /// </summary>
    private static HashSet<string> GenerateClassDeclaration(
        StringBuilder sb,
        CliCommandDefinition command,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        var constructorParameters = GeneratorUtils.GetRequiredConstructorParameters(command, positionalArguments);
        var existingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (constructorParameters.Count > 0)
        {
            // Use primary constructor for required parameters
            var parameters = new List<string>();

            foreach (var parameter in constructorParameters)
            {
                var attribute = parameter.Option is { } option
                    ? GeneratorUtils.GenerateCliAttributeString(option)
                    : GetPositionalAttributeString(parameter.PositionalArgument!);
                var secretAttribute = parameter.Option is { IsSecret: true } secretOption
                    ? $"{GeneratorUtils.GenerateSecretAttribute(secretOption)}, "
                    : parameter.IsSecret ? "SecretValue, " : "";
                parameters.Add(
                    $"    [property: {secretAttribute}{attribute}] " +
                    $"{parameter.CSharpType.TrimEnd('?')} {parameter.PropertyName}");
                existingNames.Add(parameter.PropertyName);
            }

            sb.AppendLine($"public record {command.ClassName}(");
            sb.AppendLine(string.Join($",{Environment.NewLine}", parameters));
            sb.AppendLine($") : {command.ParentClassName}");
        }
        else
        {
            sb.AppendLine($"public record {command.ClassName} : {command.ParentClassName}");
        }

        return existingNames;
    }

    private static void GenerateProperty(StringBuilder sb, CliOptionDefinition option)
    {
        // XML documentation
        GeneratorUtils.GenerateXmlDocumentation(sb, option.Description);

        // Validation attributes
        if (option.ValidationConstraints is not null)
        {
            GeneratorUtils.GenerateValidationAttributes(sb, option.ValidationConstraints);
        }

        // Secret attribute for sensitive values
        if (option.IsSecret)
        {
            sb.AppendLine($"    [{GeneratorUtils.GenerateSecretAttribute(option)}]");
        }

        // Command attribute
        var attribute = GeneratorUtils.GenerateCliAttributeString(option);
        sb.AppendLine($"    [{attribute}]");

        // Property
        sb.AppendLine($"    public {option.CSharpType} {option.PropertyName} {{ get; set; }}");
    }

    private static void GeneratePositionalArgument(StringBuilder sb, CliPositionalArgument positional)
    {
        GeneratorUtils.GenerateXmlDocumentation(sb, positional.Description);

        if (positional.IsSecret)
        {
            sb.AppendLine("    [SecretValue]");
        }

        var attrString = GetPositionalAttributeString(positional);
        sb.AppendLine($"    [{attrString}]");
        sb.AppendLine($"    public {positional.CSharpType} {positional.PropertyName} {{ get; set; }}");
    }

    private static void GenerateCompatibilityProperty(StringBuilder sb, CliCompatibilityProperty property)
    {
        sb.AppendLine($"    [Obsolete({GeneratorUtils.FormatStringLiteral(property.ObsoleteMessage)})]");

        if (property.ForwardToPropertyName is null)
        {
            sb.AppendLine($"    public {GetNewModifier(property.PropertyName)}{property.CSharpType} {property.PropertyName} {{ get; set; }}");
            return;
        }

        sb.AppendLine($"    public {GetNewModifier(property.PropertyName)}{property.CSharpType} {property.PropertyName}");
        sb.AppendLine("    {");
        sb.AppendLine($"        get => {property.ForwardToPropertyName};");
        sb.AppendLine($"        set => {property.ForwardToPropertyName} = value;");
        sb.AppendLine("    }");
    }

    private static string GetNewModifier(string propertyName) =>
        InheritedPropertyCollisionResolver.IsInheritedPropertyName(propertyName) ? "new " : "";

    private static string GetPositionalAttributeString(CliPositionalArgument positional)
    {
        var parts = new List<string> { positional.PositionIndex.ToString() };

        // Map to ArgumentPlacement enum
        var placement = positional.Placement switch
        {
            PositionalArgumentPosition.BeforeOptions or PositionalArgumentPosition.BeforeSwitches =>
                "ArgumentPlacement.BeforeOptions",
            PositionalArgumentPosition.ImmediatelyAfterCommand =>
                "ArgumentPlacement.ImmediatelyAfterCommand",
            _ => null // AfterOptions is the default, no need to specify
        };

        if (placement is not null)
        {
            parts.Add($"Placement = {placement}");
        }

        if (positional.PrependOptionTerminator)
        {
            parts.Add("PrependOptionTerminator = true");
        }

        // Note: We intentionally do NOT add the Name property here.
        // The Name property is only for documentation/help text and causes
        // CommandArgumentBuilder to skip the argument (it assumes Name means
        // the argument is handled via placeholder replacement).

        return $"CliArgument({string.Join(", ", parts)})";
    }
}
