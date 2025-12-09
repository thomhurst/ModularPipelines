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

        // File header
        GeneratorUtils.GenerateFileHeader(sb);

        sb.AppendLine("using ModularPipelines.Context;");
        sb.AppendLine("using ModularPipelines.Models;");
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");
        sb.AppendLine();

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Services;");
        sb.AppendLine();

        var className = tool.NamespacePrefix;
        var interfaceName = $"I{tool.NamespacePrefix}";

        // Check if we have sub-domains
        var subDomains = tool.SubDomainGroups.ToList();
        var rootCommands = tool.Commands.Where(c => c.SubDomainGroup is null).ToList();

        // Class documentation
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated implementation for {tool.ToolName} CLI commands.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"internal class {className} : {interfaceName}");
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
                var pascalSubDomain = GeneratorUtils.ToPascalCase(subDomain);
                var subDomainClassName = $"{tool.NamespacePrefix}{pascalSubDomain}";
                var rawParamName = char.ToLowerInvariant(pascalSubDomain[0]) + pascalSubDomain[1..];
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
                var pascalSubDomain = GeneratorUtils.ToPascalCase(subDomain);
                var rawParamName = char.ToLowerInvariant(pascalSubDomain[0]) + pascalSubDomain[1..];
                var paramName = GeneratorUtils.EscapeIdentifier(rawParamName);
                sb.AppendLine($"        {pascalSubDomain} = {paramName};");
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
                var pascalSubDomain = GeneratorUtils.ToPascalCase(subDomain);
                var subDomainClassName = $"{tool.NamespacePrefix}{pascalSubDomain}";
                sb.AppendLine($"    /// <inheritdoc />");
                sb.AppendLine($"    public {subDomainClassName} {pascalSubDomain} {{ get; }}");
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
            sb.AppendLine();
        }

        // Root-level command methods
        // Skip commands that would collide with sub-domain property names
        var subDomainNames = new HashSet<string>(
            subDomains.Select(s => GeneratorUtils.ToPascalCase(s)),
            StringComparer.OrdinalIgnoreCase);

        var nonCollidingRootCommands = rootCommands
            .Where(c => !subDomainNames.Contains(GenerateMethodName(c)))
            .ToList();

        if (nonCollidingRootCommands.Count > 0)
        {
            sb.AppendLine("    #region Commands");
            sb.AppendLine();

            foreach (var command in nonCollidingRootCommands.OrderBy(c => c.ClassName))
            {
                GenerateMethod(sb, command, tool);
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateMethod(StringBuilder sb, CliCommandDefinition command, CliToolDefinition tool)
    {
        var methodName = GenerateMethodName(command);
        var hasRequiredParams = command.RequiredOptions.Count > 0 ||
                                command.PositionalArguments.Any(p => p.IsRequired);

        // XML documentation
        sb.AppendLine("    /// <inheritdoc />");

        // Method signature - nullable options if no required params
        var optionsParam = hasRequiredParams
            ? $"{command.ClassName} options"
            : $"{command.ClassName}? options = default";

        sb.AppendLine($"    public virtual async Task<CommandResult> {methodName}(");
        sb.AppendLine($"        {optionsParam},");
        sb.AppendLine("        CancellationToken cancellationToken = default)");
        sb.AppendLine("    {");

        // Method body
        if (hasRequiredParams)
        {
            sb.AppendLine("        return await _command.ExecuteCommandLineTool(options, cancellationToken);");
        }
        else
        {
            sb.AppendLine($"        return await _command.ExecuteCommandLineTool(options ?? new {command.ClassName}(), cancellationToken);");
        }

        sb.AppendLine("    }");
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
}
