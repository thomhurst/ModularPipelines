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
        var virtualDispatchAliases = GetVirtualDispatchAliases(
            globalOptions,
            tool.GlobalCompatibilityProperties);

        // File header with nullable enable
        GeneratorUtils.GenerateFileHeaderWithNullable(sb);

        // Usings
        if (globalOptions.Any(static option => option.IsSecret))
        {
            sb.AppendLine("using ModularPipelines.Secrets;");
        }

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

        if (globalOptions.Any(o => o.EnumDefinition is not null)
            || tool.GlobalCompatibilityProperties.Any(property => tool.AllEnums.Any(definition =>
                definition.EnumName.Equals(
                    GeneratorUtils.GetEnumTypeName(property.CSharpType),
                    StringComparison.Ordinal))))
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
        if (tool.GlobalOptionsBeforeSubcommands)
        {
            sb.AppendLine("[CliGlobalOptions]");
        }

        // Class declaration
        var className = $"{tool.NamespacePrefix}Options";
        sb.AppendLine($"public abstract record {className} : CommandLineToolOptions");
        sb.AppendLine("{");

        // Properties for global options
        foreach (var option in globalOptions.OrderBy(o => o.PropertyName))
        {
            virtualDispatchAliases.TryGetValue(option.PropertyName, out var virtualDispatchAlias);
            GenerateProperty(sb, option, virtualDispatchAlias?.PropertyName);
            sb.AppendLine();
        }

        foreach (var compatibilityProperty in tool.GlobalCompatibilityProperties)
        {
            var newModifier = InheritedPropertyCollisionResolver.IsInheritedPropertyName(
                compatibilityProperty.PropertyName)
                ? "new "
                : string.Empty;
            GeneratorUtils.GenerateCompatibilityProperty(
                sb,
                GetEmittedCompatibilityProperty(compatibilityProperty, virtualDispatchAliases),
                $"{newModifier}virtual ");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static IReadOnlyDictionary<string, CliCompatibilityProperty> GetVirtualDispatchAliases(
        IReadOnlyList<CliOptionDefinition> globalOptions,
        IReadOnlyList<CliCompatibilityProperty> compatibilityProperties)
    {
        return compatibilityProperties
            .Where(property => property is
            {
                ForwardToPropertyName: not null,
                ForwardingKind: CliCompatibilityForwardingKind.Direct,
                UseInitAccessor: false,
            })
            .Where(property => globalOptions.Any(option =>
                option.PropertyName.Equals(property.ForwardToPropertyName, StringComparison.Ordinal)
                && option.PropertyType.Equals(property.CSharpType, StringComparison.Ordinal)))
            .GroupBy(property => property.ForwardToPropertyName!, StringComparer.Ordinal)
            .Where(group => group.Count() == 1)
            .ToDictionary(group => group.Key, group => group.Single(), StringComparer.Ordinal);
    }

    private static CliCompatibilityProperty GetEmittedCompatibilityProperty(
        CliCompatibilityProperty compatibilityProperty,
        IReadOnlyDictionary<string, CliCompatibilityProperty> virtualDispatchAliases)
    {
        if (compatibilityProperty.ForwardToPropertyName is not { } target
            || !virtualDispatchAliases.TryGetValue(target, out var virtualDispatchAlias)
            || !virtualDispatchAlias.PropertyName.Equals(
                compatibilityProperty.PropertyName,
                StringComparison.Ordinal))
        {
            return compatibilityProperty;
        }

        return compatibilityProperty with { ForwardToPropertyName = null };
    }

    private static void GenerateProperty(
        StringBuilder sb,
        CliOptionDefinition option,
        string? virtualDispatchAlias)
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
        if (virtualDispatchAlias is null)
        {
            sb.AppendLine($"    public virtual {option.PropertyType} {option.PropertyName} {{ get; set; }}");
            return;
        }

        sb.AppendLine("#pragma warning disable CS0618");
        sb.AppendLine($"    public virtual {option.PropertyType} {option.PropertyName}");
        sb.AppendLine("    {");
        sb.AppendLine($"        get => {virtualDispatchAlias};");
        sb.AppendLine($"        set => {virtualDispatchAlias} = value;");
        sb.AppendLine("    }");
        sb.AppendLine("#pragma warning restore CS0618");
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
