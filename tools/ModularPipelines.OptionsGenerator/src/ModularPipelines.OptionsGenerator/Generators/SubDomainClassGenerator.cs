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
    /// <summary>
    /// Reserved field names that cannot be used for backing fields.
    /// </summary>
    private static readonly HashSet<string> ReservedFieldNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "_command" // Used for ICommand dependency injection
    };

    /// <summary>
    /// Generates a safe backing field name for a child sub-command group.
    /// Avoids conflicts with reserved field names like _command.
    /// </summary>
    private static string GetSafeFieldName(string pascalSegment)
    {
        var baseName = $"_{char.ToLowerInvariant(pascalSegment[0])}{pascalSegment[1..]}";

        if (ReservedFieldNames.Contains(baseName))
        {
            return $"{baseName}SubGroup";
        }

        return baseName;
    }

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
            var methodName = GeneratorUtils.GenerateMethodNameFromLastCommandPart(command);
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

        sb.AppendLine("using System.CodeDom.Compiler;");
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
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine($"public class {node.ClassName}");
        sb.AppendLine("{");

        // Private field for ICommand
        sb.AppendLine("    private readonly ICommand _command;");

        // Private fields for child instances (lazy)
        foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
        {
            var fieldName = GetSafeFieldName(child.PascalSegment);
            sb.AppendLine($"    private {child.ClassName}? {fieldName};");
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
                var fieldName = GetSafeFieldName(child.PascalSegment);

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
                var methodName = GeneratorUtils.GenerateMethodNameFromLastCommandPart(command);
                GeneratorUtils.GenerateServiceMethod(sb, methodName, command);
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateExecuteMethod(StringBuilder sb, CliCommandDefinition command)
    {
        // Execute method for parent commands uses custom description if none provided
        // and always has nullable options (unlike regular commands which check RequiredOptions)
        var description = !string.IsNullOrEmpty(command.Description)
            ? command.Description
            : "Executes the parent command directly.";
        GeneratorUtils.GenerateXmlDocumentation(sb, description);
        sb.AppendLine("    /// <param name=\"options\">The command options.</param>");
        sb.AppendLine("    /// <param name=\"executionOptions\">The execution configuration options.</param>");
        sb.AppendLine("    /// <param name=\"cancellationToken\">Cancellation token.</param>");
        sb.AppendLine("    /// <returns>The command result.</returns>");
        sb.AppendLine($"    public virtual async Task<CommandResult> Execute(");
        sb.AppendLine($"        {command.ClassName}? options = default,");
        sb.AppendLine("        CommandExecutionOptions? executionOptions = null,");
        sb.AppendLine("        CancellationToken cancellationToken = default)");
        sb.AppendLine("    {");
        sb.AppendLine($"        return await _command.ExecuteCommandLineTool(options ?? new {command.ClassName}(), executionOptions, cancellationToken);");
        sb.AppendLine("    }");
    }
}
