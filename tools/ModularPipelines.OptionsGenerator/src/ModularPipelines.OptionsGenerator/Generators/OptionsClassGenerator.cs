using System.Text;
using ModularPipelines.Attributes;
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
        var compatibilityConstructors = command.AliasCompatibilityConstructors
            .GetValueOrDefault(aliasClassName, []);
        var compatibilityProperties = command.AliasCompatibilityProperties
            .GetValueOrDefault(aliasClassName, []);
        var compatibilityPropertyNames = compatibilityProperties
            .Select(static property => property.PropertyName)
            .ToHashSet(StringComparer.Ordinal);
        var requiredParameters = GeneratorUtils.GetRequiredConstructorParameters(command);
        var enumOptions = command.Options
            .Where(option => option.EnumDefinition is not null
                             && option.ValueArity != CliOptionValueArity.Optional
                             && !compatibilityPropertyNames.Contains(option.PropertyName))
            .ToArray();
        var usesCompatibilityEnums = compatibilityProperties.Any(property =>
            !property.AliasCSharpType.Equals(property.CanonicalCSharpType, StringComparison.Ordinal));
        var sb = new StringBuilder();
        GeneratorUtils.GenerateFileHeaderWithNullable(sb, command.DocumentationUrl);
        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
        if (requiredParameters.Any(static parameter =>
                parameter.Option?.ValueArity == CliOptionValueArity.Optional))
        {
            sb.AppendLine("using ModularPipelines.Models;");
        }

        if (enumOptions.Length > 0 || usesCompatibilityEnums)
        {
            if (enumOptions.Length > 0)
            {
                sb.AppendLine("using ModularPipelines.Attributes;");
            }

            sb.AppendLine($"using {tool.TargetNamespace}.Enums;");
        }

        sb.AppendLine();
        sb.AppendLine($"namespace {tool.TargetNamespace}.Options;");
        sb.AppendLine();
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine("[ExcludeFromCodeCoverage]");
        if (enumOptions.Length == 0
            && requiredParameters.Count == 0
            && compatibilityConstructors.Count == 0
            && compatibilityProperties.Count == 0)
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
            GenerateCompatibilityConstructors(
                sb,
                aliasClassName,
                compatibilityConstructors);
            foreach (var option in enumOptions)
            {
                GenerateCompatibilityEnumProperty(sb, option, tool, alias);
            }

            foreach (var property in compatibilityProperties)
            {
                GenerateAliasCompatibilityProperty(sb, property);
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
            $"        {GeneratorUtils.GetAliasedRequiredConstructorParameterType(parameter, tool, alias)} {parameter.PropertyName}");
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

    private static string GetCompatibilityBaseArgument(
        GeneratorUtils.RequiredConstructorParameter parameter,
        string parameterName)
    {
        var canonicalEnumName = parameter.Option?.EnumDefinition?.EnumName;
        if (canonicalEnumName is null
            || parameter.Option?.ValueArity == CliOptionValueArity.Optional)
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
        var nullableOperator = option.CSharpType.EndsWith('?')
            ? "?"
            : string.Empty;
        sb.AppendLine(
            $"        get => base.{option.PropertyName}{nullableOperator}.Select("
            + $"static value => ({aliasEnumName})(int)value);");
        sb.AppendLine(
            $"        set => base.{option.PropertyName} = value{nullableOperator}.Select("
            + $"static value => ({canonicalEnumName})(int)value);");
    }

    private static void GenerateAliasCompatibilityProperty(
        StringBuilder sb,
        CliAliasCompatibilityProperty property)
    {
        var isDirectForward = property.AliasCSharpType.Equals(
            property.CanonicalCSharpType,
            StringComparison.Ordinal);
        var aliasEnumName = GeneratorUtils.GetEnumTypeName(property.AliasCSharpType);
        var canonicalEnumName = GeneratorUtils.GetEnumTypeName(property.CanonicalCSharpType);
        var aliasIsEnumerable = property.AliasCSharpType.StartsWith("IEnumerable<", StringComparison.Ordinal);
        var canonicalIsEnumerable = property.CanonicalCSharpType.StartsWith("IEnumerable<", StringComparison.Ordinal);
        if (aliasIsEnumerable != canonicalIsEnumerable)
        {
            throw new InvalidOperationException(
                $"Cannot retain alias property {property.PropertyName} because its collection shape changed.");
        }

        var aliasIsNullable = property.AliasCSharpType.EndsWith('?');
        var canonicalIsNullable = property.CanonicalCSharpType.EndsWith('?');
        if (aliasIsNullable != canonicalIsNullable)
        {
            throw new InvalidOperationException(
                $"Cannot retain alias property {property.PropertyName} because its nullability changed.");
        }

        sb.AppendLine($"    [Obsolete({GeneratorUtils.FormatStringLiteral(property.ObsoleteMessage)})]");
        sb.AppendLine($"    public new {property.AliasCSharpType} {property.PropertyName}");
        sb.AppendLine("    {");
        var accessor = property.UseInitAccessor ? "init" : "set";
        if (isDirectForward)
        {
            sb.AppendLine($"        get => base.{property.PropertyName};");
            sb.AppendLine($"        {accessor} => base.{property.PropertyName} = value;");
        }
        else if (aliasIsEnumerable)
        {
            var baseNullableOperator = canonicalIsNullable ? "?" : string.Empty;
            var aliasNullableOperator = aliasIsNullable ? "?" : string.Empty;
            sb.AppendLine(
                $"        get => base.{property.PropertyName}{baseNullableOperator}.Select("
                + $"static value => ({aliasEnumName})(int)value);");
            sb.AppendLine(
                $"        {accessor} => base.{property.PropertyName} = value{aliasNullableOperator}.Select("
                + $"static value => ({canonicalEnumName})(int)value);");
        }
        else if (aliasIsNullable)
        {
            sb.AppendLine($"        get => base.{property.PropertyName} is null");
            sb.AppendLine("            ? null");
            sb.AppendLine($"            : ({aliasEnumName})(int)base.{property.PropertyName}.Value;");
            sb.AppendLine($"        {accessor} => base.{property.PropertyName} = value is null");
            sb.AppendLine("            ? null");
            sb.AppendLine($"            : ({canonicalEnumName})(int)value.Value;");
        }
        else
        {
            sb.AppendLine($"        get => ({aliasEnumName})(int)base.{property.PropertyName};");
            sb.AppendLine($"        {accessor} => base.{property.PropertyName} = ({canonicalEnumName})(int)value;");
        }

        sb.AppendLine("    }");
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
        GenerateCompatibilityConstructors(
            sb,
            command.ClassName,
            command.CompatibilityConstructors);
        GenerateProperties(sb, command, positionalArguments, existingPropertyNames);
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateCompatibilityConstructors(
        StringBuilder sb,
        string className,
        IReadOnlyList<CliCompatibilityConstructor> constructors)
    {
        foreach (var constructor in constructors)
        {
            var parameters = constructor.Parameters
                .Select(parameter => $"{parameter.CSharpType} {parameter.PropertyName}");
            sb.AppendLine($"    public {className}({string.Join(", ", parameters)})");
            sb.AppendLine($"        : this({string.Join(", ", constructor.PrimaryConstructorArguments)})");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine();

            if (constructor.PreserveDeconstruct)
            {
                var deconstructParameters = constructor.Parameters
                    .Select(parameter => $"out {parameter.CSharpType} {parameter.PropertyName}");
                sb.AppendLine($"    public void Deconstruct({string.Join(", ", deconstructParameters)})");
                sb.AppendLine("    {");
                foreach (var parameter in constructor.Parameters)
                {
                    sb.AppendLine($"        {parameter.PropertyName} = this.{parameter.PropertyName};");
                }

                sb.AppendLine("    }");
                sb.AppendLine();
            }
        }
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
        if (command.Options.Any(o => o.EnumDefinition is not null)
            || command.CompatibilityProperties.Any(property => tool.AllEnums.Any(definition =>
                definition.EnumName.Equals(
                    GeneratorUtils.GetEnumTypeName(property.CSharpType),
                    StringComparison.Ordinal))))
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

            GeneratorUtils.GenerateCompatibilityProperty(
                sb,
                compatibilityProperty,
                GetNewModifier(compatibilityProperty.PropertyName));
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
            GeneratorUtils.GenerateValidationAttributes(
                sb,
                option.ValidationConstraints,
                useCliOptionValueAttributes: option.ValueArity == CliOptionValueArity.Optional);
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
        sb.AppendLine($"    public {GetNewModifier(option.PropertyName)}{option.PropertyType} {option.PropertyName} {{ get; set; }}");
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

    private static string GetNewModifier(string propertyName) =>
        InheritedPropertyCollisionResolver.IsInheritedPropertyName(propertyName) ? "new " : "";

    private static string GetPositionalAttributeString(CliPositionalArgument positional)
    {
        var parts = new List<string> { positional.PositionIndex.ToString() };
        parts.Add($"Phase = CommandLinePhase.{positional.Phase}");

        if (positional.PrependOptionTerminator)
        {
            parts.Add("PrependOptionTerminator = true");
        }

        if (positional.PrependOptionTerminatorIfValueStartsWithDash)
        {
            parts.Add("PrependOptionTerminatorIfValueStartsWithDash = true");
        }

        if (positional.IsRequired)
        {
            parts.Add("Required = true");
        }

        return $"CliArgument({string.Join(", ", parts)})";
    }
}
