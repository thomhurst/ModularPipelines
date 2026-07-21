using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates the service implementation class that implements the I{Tool} interface.
/// Follows the existing ModularPipelines pattern of delegating to ICommand.ExecuteCommandLineTool().
/// </summary>
public class ServiceImplementationGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
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
        var subDomains = tool.SubDomainGroups.ToList();

        // Class documentation
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated implementation for {tool.ToolName} CLI commands.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine($"internal partial class {className} : {interfaceName}");
        sb.AppendLine("{");

        // Private field for ICommand
        sb.AppendLine("    private readonly ICommand _command;");
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
                constructorParams.Add($"        {subDomainClassName} {paramName}");
            }
            constructorParams.Add("        ICommand command");

            sb.AppendLine(string.Join(",\n", constructorParams));
            sb.AppendLine("    )");
            sb.AppendLine("    {");

            // Assign sub-domains
            foreach (var subDomain in subDomains.OrderBy(s => s))
            {
                var rawParamName = char.ToLowerInvariant(subDomain[0]) + subDomain[1..];
                var paramName = GeneratorUtils.EscapeIdentifier(rawParamName);
                sb.AppendLine($"        {subDomain} = {paramName};");
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
            sb.AppendLine($"    public {className}(ICommand command)");
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
                sb.AppendLine($"    public {subDomainClassName} {subDomain} {{ get; }}");
                sb.AppendLine();
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
