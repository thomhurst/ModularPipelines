using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates the DI registration extensions class using C# 14 extension members.
/// This follows the pattern of HelmExtensions, DockerExtensions, KubernetesExtensions in ModularPipelines.
/// </summary>
public class DependencyRegistrationGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        if (!tool.GenerateCommandFacade && tool.SubDomainGroups.Count == 0)
        {
            return Task.FromResult<IReadOnlyList<GeneratedFile>>([]);
        }

        var content = GenerateExtensionsClass(tool);
        var fileName = $"{tool.NamespacePrefix}Extensions.Generated.cs";
        var relativePath = Path.Combine(tool.OutputDirectory, "Extensions", fileName);

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

    private static string GenerateExtensionsClass(CliToolDefinition tool)
    {
        var sb = new StringBuilder();

        // File header
        GeneratorUtils.GenerateFileHeaderWithNullable(sb);

        // Usings
        if (tool.CommandGroupAliases.Count > 0)
        {
            sb.AppendLine("#pragma warning disable CS0618 // Compatibility aliases are intentionally registered.");
            sb.AppendLine();
        }

        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        sb.AppendLine("using Microsoft.Extensions.DependencyInjection.Extensions;");
        sb.AppendLine("using ModularPipelines.Attributes;");
        if (tool.GenerateCommandFacade)
        {
            sb.AppendLine("using ModularPipelines.Context;");
        }

        sb.AppendLine($"using {tool.TargetNamespace}.Services;");
        sb.AppendLine();

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Extensions;");
        sb.AppendLine();

        var className = $"{tool.NamespacePrefix}Extensions";
        var serviceName = tool.NamespacePrefix;
        var interfaceName = $"I{tool.NamespacePrefix}";

        // Class documentation for DI registration
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated extensions for registering {tool.ToolName} services.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine($"public static class {className}");
        sb.AppendLine("{");

        // IServiceCollection extension method
        sb.AppendLine("    /// <summary>");
        sb.AppendLine($"    /// Registers {tool.ToolName} services with the dependency injection container.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"services\">The service collection.</param>");
        sb.AppendLine("    /// <returns>The service collection for chaining.</returns>");
        sb.AppendLine("    [ModularPipelinesIntegration]");
        sb.AppendLine($"    public static IServiceCollection Register{serviceName}Context(this IServiceCollection services)");
        sb.AppendLine("    {");

        if (tool.GenerateCommandFacade)
        {
            // Use Services.{serviceName} to avoid ambiguity with namespace (e.g., ModularPipelines.Helm vs Helm class)
            sb.AppendLine($"        services.TryAddScoped<{interfaceName}, Services.{serviceName}>();");
        }

        // Register sub-domain services
        var subDomains = tool.SubDomainGroups.OrderBy(s => s).ToList();
        foreach (var subDomain in subDomains)
        {
            var subDomainIdentifier = GeneratorUtils.GetSubDomainIdentifier(tool, subDomain);
            var subDomainClassName = $"{tool.NamespacePrefix}{subDomainIdentifier}";
            sb.AppendLine($"        services.TryAddScoped<I{subDomainClassName}, {subDomainClassName}>();");

            foreach (var alias in GeneratorUtils.GetCommandGroupAliases(
                         tool,
                         subDomainIdentifier))
            {
                var aliasIdentifier = GeneratorUtils.GetAliasCommandGroupIdentifier(alias);
                sb.AppendLine(
                    $"        services.TryAddScoped<I{tool.NamespacePrefix}{aliasIdentifier}, "
                    + $"{tool.NamespacePrefix}{aliasIdentifier}>();");
            }
        }

        sb.AppendLine("        return services;");
        sb.AppendLine("    }");
        sb.AppendLine();

        if (tool.GenerateCommandFacade)
        {
            // Traditional extension method retained as a pre-C# 14 compatibility accessor.
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Gets the {tool.ToolName} service from the pipeline context for compatibility.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    /// <param name=\"context\">The pipeline context.</param>");
            sb.AppendLine($"    /// <returns>The <see cref=\"{interfaceName}\"/> service for executing {tool.ToolName} commands.</returns>");
            sb.AppendLine("    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]");
            sb.AppendLine($"    [global::System.Obsolete(\"Use context.Tools.Get<I{serviceName}>().\")]");
            sb.AppendLine($"    public static {interfaceName} {serviceName}(this IPipelineContext context) => context.Services.GetRequiredService<{interfaceName}>();");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}
