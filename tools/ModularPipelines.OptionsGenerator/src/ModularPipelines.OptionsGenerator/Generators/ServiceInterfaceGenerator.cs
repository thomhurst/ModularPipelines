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
        if (!tool.GenerateCommandFacade)
        {
            return Task.FromResult<IReadOnlyList<GeneratedFile>>([]);
        }

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

        GeneratorUtils.GenerateFileHeaderWithNullable(sb);

        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using ModularPipelines.Models;");
        sb.AppendLine("using ModularPipelines.Options;");
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");
        sb.AppendLine();

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Services;");
        sb.AppendLine();

        // Interface declaration
        sb.AppendLine($"/// <summary>");
        sb.AppendLine($"/// Generated interface for {tool.ToolName} CLI commands.");
        sb.AppendLine($"/// </summary>");
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
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
                var subDomainIdentifier = GeneratorUtils.GetSubDomainIdentifier(tool, subDomain);
                var subDomainClassName = $"{tool.NamespacePrefix}{subDomainIdentifier}";
                sb.AppendLine($"    /// <summary>");
                sb.AppendLine($"    /// Gets the {subDomain.ToLowerInvariant()} sub-domain service.");
                sb.AppendLine($"    /// </summary>");
                if (GeneratorUtils.IsCompatibilityOnlySubDomain(tool, subDomain))
                {
                    sb.AppendLine(
                        $"    [Obsolete({GeneratorUtils.FormatStringLiteral(GeneratorUtils.CompatibilityOnlyObsoleteMessage)})]");
                }

                sb.AppendLine(
                    $"    I{subDomainClassName} {subDomainIdentifier} => throw new System.NotSupportedException();");
                sb.AppendLine();

                foreach (var alias in GeneratorUtils.GetCommandGroupAliases(
                             tool,
                             subDomainIdentifier))
                {
                    var aliasIdentifier =
                        GeneratorUtils.GetAliasCommandGroupIdentifier(alias);
                    sb.AppendLine(
                        $"    [Obsolete({GeneratorUtils.FormatStringLiteral(alias.ObsoleteMessage)})]");
                    sb.AppendLine($"    I{tool.NamespacePrefix}{aliasIdentifier} {aliasIdentifier} {{ get; }}");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("    #endregion");
            sb.AppendLine();
        }

        // Root-level commands (commands without a sub-domain). The collision filter is
        // shared with ServiceImplementationGenerator so interface and implementation
        // always agree.
        var nonCollidingRootCommands = GeneratorUtils.GetNonCollidingRootCommands(tool);

        if (nonCollidingRootCommands.Count > 0)
        {
            sb.AppendLine("    #region Commands");
            sb.AppendLine();

            foreach (var command in nonCollidingRootCommands.OrderBy(c => c.ClassName))
            {
                var methodName = GeneratorUtils.GenerateMethodNameFromCommandParts(command.CommandParts);
                GeneratorUtils.GenerateServiceMethodSignature(sb, methodName, command);
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}
