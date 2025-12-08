using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates the service interface with methods for each command.
/// </summary>
public class ServiceInterfaceGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        var content = GenerateInterface(tool);
        var fileName = $"I{tool.NamespacePrefix}.Generated.cs";
        var relativePath = Path.Combine(tool.OutputDirectory, "Services", fileName);

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

    private static string GenerateInterface(CliToolDefinition tool)
    {
        var sb = new StringBuilder();

        // File header
        GeneratorUtils.GenerateFileHeader(sb);

        sb.AppendLine("using ModularPipelines.Models;");
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");
        sb.AppendLine();

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Services;");
        sb.AppendLine();

        // Interface declaration
        sb.AppendLine($"/// <summary>");
        sb.AppendLine($"/// Generated interface for {tool.ToolName} CLI commands.");
        sb.AppendLine($"/// </summary>");
        sb.AppendLine($"public partial interface I{tool.NamespacePrefix}");
        sb.AppendLine("{");

        // Sub-domain properties (like Docker's DockerNetwork, DockerContainer, etc.)
        var subDomains = tool.SubDomainGroups.OrderBy(s => s).ToList();
        if (subDomains.Count > 0)
        {
            sb.AppendLine("    #region Sub-domain Services");
            sb.AppendLine();

            foreach (var subDomain in subDomains)
            {
                var pascalSubDomain = GeneratorUtils.ToPascalCase(subDomain);
                var subDomainClassName = $"{tool.NamespacePrefix}{pascalSubDomain}";
                sb.AppendLine($"    /// <summary>");
                sb.AppendLine($"    /// Gets the {subDomain.ToLowerInvariant()} sub-domain service.");
                sb.AppendLine($"    /// </summary>");
                sb.AppendLine($"    {subDomainClassName} {pascalSubDomain} {{ get; }}");
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
            sb.AppendLine();
        }

        // Root-level commands (commands without a sub-domain)
        var rootCommands = tool.Commands.Where(c => c.SubDomainGroup is null).ToList();
        if (rootCommands.Count > 0)
        {
            sb.AppendLine("    #region Commands");
            sb.AppendLine();

            foreach (var command in rootCommands.OrderBy(c => c.ClassName))
            {
                GenerateMethodSignature(sb, command);
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateMethodSignature(StringBuilder sb, CliCommandDefinition command)
    {
        // XML documentation
        if (!string.IsNullOrEmpty(command.Description))
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {EscapeXmlComment(command.Description)}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    /// <param name=\"options\">The command options.</param>");
            sb.AppendLine("    /// <param name=\"cancellationToken\">Cancellation token.</param>");
            sb.AppendLine("    /// <returns>The command result.</returns>");
        }

        // Generate method name from command parts
        var methodName = GenerateMethodName(command);

        sb.AppendLine($"    Task<CommandResult> {methodName}({command.ClassName} options, CancellationToken cancellationToken = default);");
    }

    private static string GenerateMethodName(CliCommandDefinition command)
    {
        // Convert command parts to method name
        // e.g., ["container", "create"] -> "ContainerCreate"
        // Handle hyphens within parts (e.g., "build-server" -> "BuildServer")
        return string.Join("", command.CommandParts
            .SelectMany(p => p.Split('-', StringSplitOptions.RemoveEmptyEntries))
            .Select(p => char.ToUpperInvariant(p[0]) + p[1..].ToLowerInvariant()));
    }

    private static string EscapeXmlComment(string text) => GeneratorUtils.EscapeXmlComment(text);
}
