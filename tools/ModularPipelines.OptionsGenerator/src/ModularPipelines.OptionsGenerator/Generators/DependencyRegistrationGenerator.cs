using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates the DI registration extensions class.
/// This follows the pattern of HelmExtensions, DockerExtensions, KubernetesExtensions in ModularPipelines.
/// </summary>
public class DependencyRegistrationGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
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
        GeneratorUtils.GenerateFileHeader(sb);

        // Usings
        sb.AppendLine("using System.Runtime.CompilerServices;");
        sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        sb.AppendLine("using Microsoft.Extensions.DependencyInjection.Extensions;");
        sb.AppendLine("using ModularPipelines.Context;");
        sb.AppendLine("using ModularPipelines.Engine;");
        sb.AppendLine($"using {tool.TargetNamespace}.Services;");
        sb.AppendLine();

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Extensions;");
        sb.AppendLine();

        var className = $"{tool.NamespacePrefix}Extensions";
        var serviceName = tool.NamespacePrefix;
        var interfaceName = $"I{tool.NamespacePrefix}";

        // Class documentation
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated extensions for registering {tool.ToolName} services.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public static class {className}");
        sb.AppendLine("{");

        // ModuleInitializer method
        sb.AppendLine("#pragma warning disable CA2255");
        sb.AppendLine("    [ModuleInitializer]");
        sb.AppendLine("#pragma warning restore CA2255");
        sb.AppendLine($"    public static void Register{serviceName}Context()");
        sb.AppendLine("    {");
        sb.AppendLine($"        ModularPipelinesContextRegistry.RegisterContext(collection => Register{serviceName}Context(collection));");
        sb.AppendLine("    }");
        sb.AppendLine();

        // IServiceCollection extension method
        sb.AppendLine("    /// <summary>");
        sb.AppendLine($"    /// Registers {tool.ToolName} services with the dependency injection container.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"services\">The service collection.</param>");
        sb.AppendLine("    /// <returns>The service collection for chaining.</returns>");
        sb.AppendLine($"    public static IServiceCollection Register{serviceName}Context(this IServiceCollection services)");
        sb.AppendLine("    {");

        // Register main service
        // Use Services.{serviceName} to avoid ambiguity with namespace (e.g., ModularPipelines.Helm vs Helm class)
        sb.AppendLine($"        services.TryAddScoped<{interfaceName}, Services.{serviceName}>();");

        // Register sub-domain services
        var subDomains = tool.SubDomainGroups.OrderBy(s => s).ToList();
        foreach (var subDomain in subDomains)
        {
            var pascalSubDomain = GeneratorUtils.ToPascalCase(subDomain);
            var subDomainClassName = $"{tool.NamespacePrefix}{pascalSubDomain}";
            sb.AppendLine($"        services.TryAddScoped<{subDomainClassName}>();");
        }

        sb.AppendLine("        return services;");
        sb.AppendLine("    }");
        sb.AppendLine();

        // Context extension method
        sb.AppendLine("    /// <summary>");
        sb.AppendLine($"    /// Gets the {tool.ToolName} service from the pipeline context.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"context\">The pipeline context.</param>");
        sb.AppendLine($"    /// <returns>The {tool.ToolName} service.</returns>");
        sb.AppendLine($"    public static {interfaceName} {serviceName}(this IPipelineHookContext context)");
        sb.AppendLine("    {");
        sb.AppendLine($"        return context.ServiceProvider.GetRequiredService<{interfaceName}>();");
        sb.AppendLine("    }");

        sb.AppendLine("}");

        return sb.ToString();
    }
}
