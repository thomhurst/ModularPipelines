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
        // Only generate if there are global options
        if (tool.GlobalOptions.Count == 0)
        {
            return Task.FromResult<IReadOnlyList<GeneratedFile>>([]);
        }

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
        if (!string.IsNullOrEmpty(option.Description))
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {EscapeXmlComment(option.Description)}");
            sb.AppendLine("    /// </summary>");
        }

        // Validation attributes
        if (option.ValidationConstraints is not null)
        {
            GenerateValidationAttributes(sb, option.ValidationConstraints);
        }

        // Command attribute
        var attribute = GetAttributeString(option);
        sb.AppendLine($"    [{attribute}]");

        // Property
        sb.AppendLine($"    public virtual {option.CSharpType} {option.PropertyName} {{ get; set; }}");
    }

    private static void GenerateValidationAttributes(StringBuilder sb, CliValidationConstraints constraints)
    {
        if (constraints.MinValue.HasValue || constraints.MaxValue.HasValue)
        {
            var min = constraints.MinValue ?? int.MinValue;
            var max = constraints.MaxValue ?? int.MaxValue;
            sb.AppendLine($"    [Range({min}, {max})]");
        }

        if (!string.IsNullOrEmpty(constraints.Pattern))
        {
            sb.AppendLine($"    [RegularExpression(@\"{constraints.Pattern}\")]");
        }
    }

    private static string GetAttributeString(CliOptionDefinition option)
    {
        if (option.IsFlag)
        {
            // Use CliFlag for boolean flags
            var parts = new List<string> { $"\"{option.SwitchName}\"" };

            if (!string.IsNullOrEmpty(option.ShortForm))
            {
                parts.Add($"ShortForm = \"{option.ShortForm}\"");
            }

            return $"CliFlag({string.Join(", ", parts)})";
        }

        // Use CliOption for value options
        var optionParts = new List<string> { $"\"{option.SwitchName}\"" };

        if (!string.IsNullOrEmpty(option.ShortForm))
        {
            optionParts.Add($"ShortForm = \"{option.ShortForm}\"");
        }

        if (option.ValueSeparator == "=")
        {
            optionParts.Add("Format = OptionFormat.EqualsSeparated");
        }
        else if (option.ValueSeparator == ":")
        {
            optionParts.Add("Format = OptionFormat.ColonSeparated");
        }
        else if (option.ValueSeparator != " " && !string.IsNullOrEmpty(option.ValueSeparator))
        {
            optionParts.Add($"CustomSeparator = \"{option.ValueSeparator}\"");
        }

        if (option.AcceptsMultipleValues)
        {
            optionParts.Add("AllowMultiple = true");
        }

        return $"CliOption({string.Join(", ", optionParts)})";
    }

    private static string EscapeXmlComment(string text) => GeneratorUtils.EscapeXmlComment(text);
}
