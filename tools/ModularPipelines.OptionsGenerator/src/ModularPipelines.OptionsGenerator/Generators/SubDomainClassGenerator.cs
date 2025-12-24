using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates sub-domain classes that group related commands with nested hierarchy.
/// Commands are accessible via nested properties matching the CLI structure.
/// E.g., gcloud workspace-add-ons deployments create -> Gcloud.WorkspaceAddOns.Deployments.Create(...)
/// </summary>
public class SubDomainClassGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        var files = new List<GeneratedFile>();

        // Find root commands that match sub-domain names (e.g., "helm completion")
        // These will be added as Execute() methods on the sub-domain class
        var parentCommands = tool.Commands
            .Where(c => c.SubDomainGroup is null && c.CommandParts.Length == 1)
            .ToDictionary(c => c.CommandParts[0], c => c, StringComparer.OrdinalIgnoreCase);

        foreach (var subDomain in tool.SubDomainGroups)
        {
            var commands = tool.Commands.Where(c => c.SubDomainGroup == subDomain).ToList();

            // Build tree structure for this sub-domain
            var tree = CommandTreeNode.BuildTree(tool.NamespacePrefix, subDomain, commands);

            // Check if there's a parent command for this sub-domain
            CliCommandDefinition? parentCommand = null;
            if (parentCommands.TryGetValue(subDomain, out var cmd))
            {
                parentCommand = cmd;
            }

            // Generate files for all nodes in the tree
            GenerateFilesFromTree(tree, tool, files, parentCommand);
        }

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
    }

    private static void GenerateFilesFromTree(
        CommandTreeNode node,
        CliToolDefinition tool,
        List<GeneratedFile> files,
        CliCommandDefinition? parentCommand = null)
    {
        // Build map of commands that collide with child property names
        // These will become Execute() methods on the child classes instead
        var collidingCommands = new Dictionary<string, CliCommandDefinition>(StringComparer.OrdinalIgnoreCase);
        foreach (var command in node.Commands)
        {
            var methodName = GetMethodNameFromCommand(command);
            var matchingChild = node.Children.Values
                .FirstOrDefault(c => c.PascalSegment.Equals(methodName, StringComparison.OrdinalIgnoreCase));

            if (matchingChild != null)
            {
                collidingCommands[matchingChild.Segment] = command;
            }
        }

        // Generate file for this node
        // Pass parentCommand only for root nodes (depth 0), and pass excluded commands
        var content = GenerateNodeClass(
            node,
            tool,
            node.Depth == 0 ? parentCommand : null,
            collidingCommands.Values.ToHashSet());

        var fileName = $"{node.ClassName}.cs";
        var relativePath = Path.Combine(tool.OutputDirectory, "Services", fileName);

        files.Add(new GeneratedFile
        {
            RelativePath = relativePath,
            Content = content
        });

        // Recursively generate files for children
        // Pass colliding command as parentCommand so it becomes Execute() on the child
        foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
        {
            collidingCommands.TryGetValue(child.Segment, out var childParentCommand);
            GenerateFilesFromTree(child, tool, files, childParentCommand);
        }
    }

    private static string GenerateNodeClass(
        CommandTreeNode node,
        CliToolDefinition tool,
        CliCommandDefinition? parentCommand = null,
        HashSet<CliCommandDefinition>? excludedCommands = null)
    {
        var sb = new StringBuilder();

        // File header
        GeneratorUtils.GenerateFileHeader(sb);

        sb.AppendLine("using ModularPipelines.Context;");
        sb.AppendLine("using ModularPipelines.Models;");
        sb.AppendLine("using ModularPipelines.Options;");
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");
        sb.AppendLine();

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Services;");
        sb.AppendLine();

        // Class documentation
        sb.AppendLine($"/// <summary>");
        sb.AppendLine($"/// {tool.ToolName} {node.Segment.ToLowerInvariant()} commands.");
        sb.AppendLine($"/// </summary>");
        sb.AppendLine($"public class {node.ClassName}");
        sb.AppendLine("{");

        // Private field for ICommand
        sb.AppendLine("    private readonly ICommand _command;");

        // Private fields for child instances (lazy)
        foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
        {
            sb.AppendLine($"    private {child.ClassName}? _{char.ToLowerInvariant(child.PascalSegment[0])}{child.PascalSegment[1..]};");
        }

        sb.AppendLine();

        // Constructor
        sb.AppendLine($"    /// <summary>");
        sb.AppendLine($"    /// Initializes a new instance of the <see cref=\"{node.ClassName}\"/> class.");
        sb.AppendLine($"    /// </summary>");
        sb.AppendLine($"    public {node.ClassName}(ICommand command)");
        sb.AppendLine("    {");
        sb.AppendLine("        _command = command;");
        sb.AppendLine("    }");

        // Properties for child sub-command groups
        if (node.Children.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("    #region Sub-command Groups");
            sb.AppendLine();

            foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
            {
                var fieldName = $"_{char.ToLowerInvariant(child.PascalSegment[0])}{child.PascalSegment[1..]}";

                sb.AppendLine($"    /// <summary>");
                sb.AppendLine($"    /// {tool.ToolName} {child.Segment.ToLowerInvariant()} sub-commands.");
                sb.AppendLine($"    /// </summary>");
                sb.AppendLine($"    public {child.ClassName} {child.PascalSegment} => {fieldName} ??= new {child.ClassName}(_command);");
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
        }

        // Methods for direct commands at this level
        // Filter out commands that collide with child property names
        var filteredCommands = excludedCommands != null
            ? node.Commands.Where(c => !excludedCommands.Contains(c)).ToList()
            : node.Commands;

        var hasCommands = filteredCommands.Count > 0 || parentCommand is not null;
        if (hasCommands)
        {
            sb.AppendLine();
            sb.AppendLine("    #region Commands");
            sb.AppendLine();

            // Add Execute method for the parent command if it exists
            if (parentCommand is not null)
            {
                GenerateExecuteMethod(sb, parentCommand);
                sb.AppendLine();
            }

            foreach (var command in filteredCommands.OrderBy(c => c.ClassName))
            {
                GenerateMethod(sb, command, node);
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateExecuteMethod(StringBuilder sb, CliCommandDefinition command)
    {
        // Single method - users set LogSettings on options if they need custom logging
        sb.AppendLine("    /// <summary>");
        if (!string.IsNullOrEmpty(command.Description))
        {
            sb.AppendLine($"    /// {EscapeXmlComment(command.Description)}");
        }
        else
        {
            sb.AppendLine("    /// Executes the parent command directly.");
        }
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"options\">The command options.</param>");
        sb.AppendLine("    /// <param name=\"cancellationToken\">Cancellation token.</param>");
        sb.AppendLine("    /// <returns>The command result.</returns>");
        sb.AppendLine($"    public virtual async Task<CommandResult> Execute(");
        sb.AppendLine($"        {command.ClassName}? options = default,");
        sb.AppendLine("        CancellationToken cancellationToken = default)");
        sb.AppendLine("    {");
        sb.AppendLine($"        return await _command.ExecuteCommandLineTool(options ?? new {command.ClassName}(), cancellationToken);");
        sb.AppendLine("    }");
    }

    private static void GenerateMethod(StringBuilder sb, CliCommandDefinition command, CommandTreeNode node)
    {
        var methodName = GetMethodName(command, node);

        // Single method - users set LogSettings on options if they need custom logging
        if (!string.IsNullOrEmpty(command.Description))
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {EscapeXmlComment(command.Description)}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    /// <param name=\"options\">The command options.</param>");
            sb.AppendLine("    /// <param name=\"cancellationToken\">Cancellation token.</param>");
            sb.AppendLine("    /// <returns>The command result.</returns>");
        }

        sb.AppendLine($"    public virtual async Task<CommandResult> {methodName}(");
        sb.AppendLine($"        {command.ClassName} options,");
        sb.AppendLine("        CancellationToken cancellationToken = default)");
        sb.AppendLine("    {");
        sb.AppendLine("        return await _command.ExecuteCommandLineTool(options, cancellationToken);");
        sb.AppendLine("    }");
    }

    private static string GetMethodName(CliCommandDefinition command, CommandTreeNode node)
    {
        return GetMethodNameFromCommand(command);
    }

    private static string GetMethodNameFromCommand(CliCommandDefinition command)
    {
        if (command.CommandParts.Length == 0)
        {
            return "Execute";
        }

        var lastPart = command.CommandParts[^1];
        var parts = lastPart.Split('-', StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", parts.Select(p =>
            char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p[1..].ToLowerInvariant() : "")));
    }

    private static string EscapeXmlComment(string text) => GeneratorUtils.EscapeXmlComment(text);
}
