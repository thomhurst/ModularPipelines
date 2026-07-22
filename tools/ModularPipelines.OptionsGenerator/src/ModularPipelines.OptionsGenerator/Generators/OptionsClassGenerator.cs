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
        }

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
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
            var args = string.Join(", ", command.CommandParts.Select(p => $"\"{p}\""));
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
        var requiredOptions = command.RequiredOptions;
        var requiredPositionals = positionalArguments.Where(p => p.IsRequired).ToList();
        var existingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (requiredOptions.Count > 0 || requiredPositionals.Count > 0)
        {
            // Use primary constructor for required parameters
            var parameters = new List<string>();

            foreach (var opt in requiredOptions)
            {
                var attr = GeneratorUtils.GenerateCliAttributeString(opt);
                var secretAttr = opt.IsSecret ? "SecretValue, " : "";
                parameters.Add($"    [property: {secretAttr}{attr}] {opt.CSharpType.TrimEnd('?')} {opt.PropertyName}");
                existingNames.Add(opt.PropertyName);
            }

            foreach (var pos in requiredPositionals)
            {
                if (existingNames.Contains(pos.PropertyName))
                {
                    continue; // Skip duplicate
                }
                var posAttr = GetPositionalAttributeString(pos);
                var secretAttr = pos.IsSecret ? "SecretValue, " : "";
                parameters.Add($"    [property: {secretAttr}{posAttr}] {pos.CSharpType.TrimEnd('?')} {pos.PropertyName}");
                existingNames.Add(pos.PropertyName);
            }

            sb.AppendLine($"public record {command.ClassName}(");
            sb.AppendLine(string.Join(",\n", parameters));
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
            sb.AppendLine("    [SecretValue]");
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
        var obsoleteMessage = property.ObsoleteMessage
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"");
        sb.AppendLine($"    [Obsolete(\"{obsoleteMessage}\")]");

        if (property.ForwardToPropertyName is null)
        {
            sb.AppendLine($"    public {property.CSharpType} {property.PropertyName} {{ get; set; }}");
            return;
        }

        sb.AppendLine($"    public {property.CSharpType} {property.PropertyName}");
        sb.AppendLine("    {");
        sb.AppendLine($"        get => {property.ForwardToPropertyName};");
        sb.AppendLine($"        set => {property.ForwardToPropertyName} = value;");
        sb.AppendLine("    }");
    }

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

        // Note: We intentionally do NOT add the Name property here.
        // The Name property is only for documentation/help text and causes
        // CommandArgumentBuilder to skip the argument (it assumes Name means
        // the argument is handled via placeholder replacement).

        return $"CliArgument({string.Join(", ", parts)})";
    }
}
