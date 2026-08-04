using System.Text;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates a base options class that includes global CLI flags.
/// This follows the pattern of HelmOptions, DockerOptions, KubernetesOptions in ModularPipelines.
/// </summary>
public class GlobalOptionsBaseGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        // Always generate a base class - it's needed even without global options
        var content = GenerateBaseOptionsClass(tool);
        var fileName = $"{tool.NamespacePrefix}Options.Generated.cs";
        var relativePath = Path.Combine(tool.OutputDirectory, "Options", fileName);

        var files = new List<GeneratedFile>
        {
            new()
            {
                RelativePath = relativePath,
                Content = content
            }
        };

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
    }

    private static string GenerateBaseOptionsClass(CliToolDefinition tool)
    {
        var sb = new StringBuilder();
        var globalOptions = tool.GetGlobalOptions();

        // File header with nullable enable
        GeneratorUtils.GenerateFileHeaderWithNullable(sb);

        // Usings
        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
        sb.AppendLine("using ModularPipelines.Attributes;");
        sb.AppendLine("using ModularPipelines.Options;");

        if (globalOptions.Any(o => o.RequiresModelsNamespace))
        {
            sb.AppendLine("using ModularPipelines.Models;");
        }

        if (globalOptions.Any(o => o.ValidationConstraints is not null))
        {
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        }

        if (globalOptions.Any(o => o.EnumDefinition is not null))
        {
            sb.AppendLine($"using {tool.TargetNamespace}.Enums;");
        }

        sb.AppendLine();

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Options;");
        sb.AppendLine();

        // Class documentation
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Base options class for {tool.ToolName} CLI commands.");
        sb.AppendLine("/// Contains global flags that apply to all commands.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine("[ExcludeFromCodeCoverage]");
        sb.AppendLine($"[CliTool({GeneratorUtils.FormatStringLiteral(tool.ToolName)})]");
        sb.AppendLine("[CliGlobalOptions]");

        // Class declaration
        var className = $"{tool.NamespacePrefix}Options";
        sb.AppendLine($"public abstract record {className} : CommandLineToolOptions");
        sb.AppendLine("{");

        // Properties for global options
        foreach (var option in globalOptions.OrderBy(o => o.PropertyName))
        {
            GenerateProperty(sb, option);
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateProperty(StringBuilder sb, CliOptionDefinition option)
    {
        // XML documentation
        GeneratorUtils.GenerateXmlDocumentation(sb, GetDescription(option));

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
        sb.AppendLine($"    public virtual {option.PropertyType} {option.PropertyName} {{ get; set; }}");
    }

    private static string? GetDescription(CliOptionDefinition option)
    {
        var parts = new List<string>();
        if (option.Description is not null)
        {
            parts.Add(option.Description);
        }

        if (option.Availability is not null)
        {
            parts.Add($"Availability: {option.Availability}.");
        }

        if (option.DocumentationUrl is not null)
        {
            parts.Add($"Documentation: {option.DocumentationUrl}");
        }

        return parts.Count == 0 ? null : string.Join(" ", parts);
    }
}
