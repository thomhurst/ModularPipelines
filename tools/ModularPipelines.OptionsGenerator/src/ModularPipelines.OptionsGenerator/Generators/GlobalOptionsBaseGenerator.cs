using System.Text;
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

        // File header with nullable enable
        GeneratorUtils.GenerateFileHeaderWithNullable(sb);

        // Usings
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
        sb.AppendLine("using ModularPipelines.Attributes;");
        sb.AppendLine("using ModularPipelines.Options;");

        if (tool.GlobalOptions.Any(o => o.IsKeyValue))
        {
            sb.AppendLine("using ModularPipelines.Models;");
        }

        if (tool.GlobalOptions.Any(o => o.ValidationConstraints is not null))
        {
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        }

        if (tool.GlobalOptions.Any(o => o.EnumDefinition is not null))
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
        sb.AppendLine("[ExcludeFromCodeCoverage]");

        // Class declaration using primary constructor with tool name
        var className = $"{tool.NamespacePrefix}Options";
        sb.AppendLine($"public abstract record {className}() : CommandLineToolOptions(\"{tool.ToolName}\")");
        sb.AppendLine("{");

        // Properties for global options
        foreach (var option in tool.GlobalOptions.OrderBy(o => o.PropertyName))
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
        GeneratorUtils.GenerateXmlDocumentation(sb, option.Description);

        // Validation attributes
        if (option.ValidationConstraints is not null)
        {
            GeneratorUtils.GenerateValidationAttributes(sb, option.ValidationConstraints);
        }

        // Command attribute
        var attribute = GeneratorUtils.GenerateCliAttributeString(option);
        sb.AppendLine($"    [{attribute}]");

        // Property
        sb.AppendLine($"    public virtual {option.CSharpType} {option.PropertyName} {{ get; set; }}");
    }
}
