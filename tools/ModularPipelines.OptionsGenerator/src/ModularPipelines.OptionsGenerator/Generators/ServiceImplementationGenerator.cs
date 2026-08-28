using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates the service implementation class that implements the I{Tool} interface.
/// Follows the existing ModularPipelines pattern of delegating to ICommandContext.ExecuteCommandLineToolAsync().
/// </summary>
public class ServiceImplementationGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        if (!tool.GenerateCommandFacade)
        {
            return Task.FromResult<IReadOnlyList<GeneratedFile>>([]);
        }

        var files = new List<GeneratedFile>();

        // Generate the main service implementation
        var mainContent = GenerateMainServiceClass(tool);
        var mainFileName = $"{tool.NamespacePrefix}.Generated.cs";
        var mainRelativePath = Path.Combine(tool.OutputDirectory, "Services", mainFileName);

        files.Add(new GeneratedFile
        {
            RelativePath = mainRelativePath,
            Content = mainContent
        });

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
    }

    private static string GenerateMainServiceClass(CliToolDefinition tool)
    {
        var sb = new StringBuilder();

        GeneratorUtils.GenerateFileHeaderWithNullable(sb);

        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using ModularPipelines.Context;");
        sb.AppendLine("using ModularPipelines.Context.Domains.Shell;");
        sb.AppendLine("using ModularPipelines.Models;");
        sb.AppendLine("using ModularPipelines.Options;");
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");
        sb.AppendLine();

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Services;");
        sb.AppendLine();

        var className = tool.NamespacePrefix;
        var interfaceName = $"I{tool.NamespacePrefix}";

        // Check if we have sub-domains
        var subDomains = tool.SubDomainGroups
            .Select(group => GeneratorUtils.GetSubDomainIdentifier(tool, group))
            .ToList();
        var compatibilityOnlySubDomains = tool.SubDomainGroups
            .Where(group => GeneratorUtils.IsCompatibilityOnlySubDomain(tool, group))
            .Select(group => GeneratorUtils.GetSubDomainIdentifier(tool, group))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Class documentation
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated implementation for {tool.ToolName} CLI commands.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine($"internal partial class {className} : {interfaceName}");
        sb.AppendLine("{");

        // Private field for ICommandContext
        sb.AppendLine("    private readonly ICommandContext _command;");
        sb.AppendLine();

        // Constructor
        if (subDomains.Count > 0)
        {
            // Constructor with sub-domain injection
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Initializes a new instance of the <see cref=\"{className}\"/> class.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public {className}(");

            var constructorParams = new List<string>();
            foreach (var subDomain in subDomains.OrderBy(s => s))
            {
                var subDomainClassName = $"{tool.NamespacePrefix}{subDomain}";
                var rawParamName = char.ToLowerInvariant(subDomain[0]) + subDomain[1..];
                var paramName = GeneratorUtils.EscapeIdentifier(rawParamName);
                constructorParams.Add($"        I{subDomainClassName} {paramName}");

                foreach (var alias in GeneratorUtils.GetCommandGroupAliases(tool, subDomain))
                {
                    var aliasIdentifier = GeneratorUtils.GetAliasCommandGroupIdentifier(alias);
                    var aliasParamName = GeneratorUtils.EscapeIdentifier(
                        char.ToLowerInvariant(aliasIdentifier[0]) + aliasIdentifier[1..]);
                    constructorParams.Add(
                        $"        I{tool.NamespacePrefix}{aliasIdentifier} {aliasParamName}");
                }
            }
            constructorParams.Add("        ICommandContext command");

            sb.AppendLine(string.Join(",\n", constructorParams));
            sb.AppendLine("    )");
            sb.AppendLine("    {");

            // Assign sub-domains
            foreach (var subDomain in subDomains.OrderBy(s => s))
            {
                var rawParamName = char.ToLowerInvariant(subDomain[0]) + subDomain[1..];
                var paramName = GeneratorUtils.EscapeIdentifier(rawParamName);
                var isCompatibilityOnly = compatibilityOnlySubDomains.Contains(subDomain);
                if (isCompatibilityOnly)
                {
                    sb.AppendLine("        #pragma warning disable CS0618");
                }

                sb.AppendLine($"        {subDomain} = {paramName};");
                if (isCompatibilityOnly)
                {
                    sb.AppendLine("        #pragma warning restore CS0618");
                }

                foreach (var alias in GeneratorUtils.GetCommandGroupAliases(tool, subDomain))
                {
                    var aliasIdentifier = GeneratorUtils.GetAliasCommandGroupIdentifier(alias);
                    var aliasParamName = GeneratorUtils.EscapeIdentifier(
                        char.ToLowerInvariant(aliasIdentifier[0]) + aliasIdentifier[1..]);
                    sb.AppendLine($"        {aliasIdentifier} = {aliasParamName};");
                }
            }

            sb.AppendLine("        _command = command;");
            sb.AppendLine("    }");
        }
        else
        {
            // Simple constructor
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Initializes a new instance of the <see cref=\"{className}\"/> class.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public {className}(ICommandContext command)");
            sb.AppendLine("    {");
            sb.AppendLine("        _command = command;");
            sb.AppendLine("    }");
        }

        sb.AppendLine();

        // Sub-domain properties
        if (subDomains.Count > 0)
        {
            sb.AppendLine("    #region Sub-domain Services");
            sb.AppendLine();

            foreach (var subDomain in subDomains.OrderBy(s => s))
            {
                var subDomainClassName = $"{tool.NamespacePrefix}{subDomain}";
                sb.AppendLine($"    /// <inheritdoc />");
                if (compatibilityOnlySubDomains.Contains(subDomain))
                {
                    sb.AppendLine(
                        $"    [Obsolete({GeneratorUtils.FormatStringLiteral(GeneratorUtils.CompatibilityOnlyObsoleteMessage)})]");
                }

                sb.AppendLine($"    public I{subDomainClassName} {subDomain} {{ get; }}");
                sb.AppendLine();

                foreach (var alias in GeneratorUtils.GetCommandGroupAliases(tool, subDomain))
                {
                    var aliasIdentifier =
                        GeneratorUtils.GetAliasCommandGroupIdentifier(alias);
                    sb.AppendLine("    /// <inheritdoc />");
                    sb.AppendLine(
                        $"    public I{tool.NamespacePrefix}{aliasIdentifier} "
                        + $"{aliasIdentifier} {{ get; }}");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("    #endregion");
            sb.AppendLine();
        }

        // Root-level command methods. The collision filter is shared with
        // ServiceInterfaceGenerator so interface and implementation always agree.
        var nonCollidingRootCommands = GeneratorUtils.GetNonCollidingRootCommands(tool);

        if (nonCollidingRootCommands.Count > 0)
        {
            sb.AppendLine("    #region Commands");
            sb.AppendLine();

            foreach (var command in nonCollidingRootCommands.OrderBy(c => c.ClassName))
            {
                var methodName = GeneratorUtils.GenerateMethodNameFromCommandParts(command.CommandParts);
                GeneratorUtils.GenerateServiceMethod(sb, methodName, command, includeXmlDoc: false);
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}
