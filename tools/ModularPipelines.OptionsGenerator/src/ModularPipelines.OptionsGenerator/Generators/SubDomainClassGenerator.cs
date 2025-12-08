using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates sub-domain classes that group related commands.
/// </summary>
public class SubDomainClassGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        var files = new List<GeneratedFile>();

        foreach (var subDomain in tool.SubDomainGroups)
        {
            var commands = tool.Commands.Where(c => c.SubDomainGroup == subDomain).ToList();
            var pascalSubDomain = GeneratorUtils.ToPascalCase(subDomain);
            var content = GenerateSubDomainClass(subDomain, pascalSubDomain, commands, tool);
            var fileName = $"{tool.NamespacePrefix}{pascalSubDomain}.cs";
            var relativePath = Path.Combine(tool.OutputDirectory, "Services", fileName);

            files.Add(new GeneratedFile
            {
                RelativePath = relativePath,
                Content = content
            });
        }

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
    }

    private static string GenerateSubDomainClass(string subDomain, string pascalSubDomain, IReadOnlyList<CliCommandDefinition> commands, CliToolDefinition tool)
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

        var className = $"{tool.NamespacePrefix}{pascalSubDomain}";

        // Class documentation
        sb.AppendLine($"/// <summary>");
        sb.AppendLine($"/// {tool.ToolName} {subDomain.ToLowerInvariant()} commands.");
        sb.AppendLine($"/// </summary>");
        sb.AppendLine($"public class {className}");
        sb.AppendLine("{");

        // Constructor with ICommand dependency
        sb.AppendLine("    private readonly ICommand _command;");
        sb.AppendLine();
        sb.AppendLine($"    /// <summary>");
        sb.AppendLine($"    /// Initializes a new instance of the <see cref=\"{className}\"/> class.");
        sb.AppendLine($"    /// </summary>");
        sb.AppendLine($"    public {className}(ICommand command)");
        sb.AppendLine("    {");
        sb.AppendLine("        _command = command;");
        sb.AppendLine("    }");
        sb.AppendLine();

        // Generate methods for each command
        foreach (var command in commands.OrderBy(c => c.ClassName))
        {
            GenerateMethod(sb, command);
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateMethod(StringBuilder sb, CliCommandDefinition command)
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

        // Method name from the last command part (since sub-domain is already in the class name)
        // Handle hyphens within the command part (e.g., "accept-ownership-status" -> "AcceptOwnershipStatus")
        var methodName = "Execute";
        if (command.CommandParts.Length > 0)
        {
            var lastPart = command.CommandParts[^1];
            var parts = lastPart.Split('-', StringSplitOptions.RemoveEmptyEntries);
            methodName = string.Join("", parts.Select(p =>
                char.ToUpperInvariant(p[0]) + p[1..].ToLowerInvariant()));
        }

        sb.AppendLine($"    public virtual async Task<CommandResult> {methodName}(");
        sb.AppendLine($"        {command.ClassName} options,");
        sb.AppendLine("        CancellationToken cancellationToken = default)");
        sb.AppendLine("    {");
        sb.AppendLine("        return await _command.ExecuteCommandLineTool(options, cancellationToken);");
        sb.AppendLine("    }");
    }

    private static string EscapeXmlComment(string text) => GeneratorUtils.EscapeXmlComment(text);
}
