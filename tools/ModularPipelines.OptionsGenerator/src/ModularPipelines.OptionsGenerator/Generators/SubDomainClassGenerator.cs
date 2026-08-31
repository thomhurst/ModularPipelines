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
        "_command" // Used for ICommandContext dependency injection
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

        // Root commands that own subcommands are exposed as ExecuteAsync() on the
        // corresponding sub-domain class.
        var parentCommands = GeneratorUtils.GetSubDomainParentCommands(tool)
            .ToDictionary(
                GeneratorUtils.GetCommandGroupIdentifier,
                StringComparer.OrdinalIgnoreCase);

        foreach (var subDomainGroup in tool.SubDomainGroups)
        {
            var commands = tool.Commands
                .Where(c => string.Equals(c.SubDomainGroup, subDomainGroup, StringComparison.OrdinalIgnoreCase))
                .ToList();
            var subDomainIdentifier = GeneratorUtils.GetSubDomainIdentifier(tool, subDomainGroup);

            // Build tree structure for this sub-domain
            var tree = CommandTreeNode.BuildTree(tool.NamespacePrefix, subDomainIdentifier, commands);

            // Check if there's a parent command for this sub-domain
            CliCommandDefinition? parentCommand = null;
            if (parentCommands.TryGetValue(subDomainIdentifier, out var cmd))
            {
                parentCommand = cmd;
            }

            // Generate files for all nodes in the tree
            GenerateFilesFromTree(
                tree,
                tool,
                files,
                parentCommand);
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
        // These will become ExecuteAsync() methods on the child classes instead
        var collidingCommands = node.GetChildParentCommands();
        var excludedCommands = collidingCommands.Values.ToHashSet();

        // Generate the command represented by this node as ExecuteAsync(). Root nodes receive
        // their top-level command; nested nodes receive a command that collided with the
        // child property used to reach them.
        var content = GenerateNodeClass(
            node,
            tool,
            parentCommand,
            excludedCommands);

        // Only facade nodes are DI abstractions. Nested groups remain concrete
        // navigation helpers owned by their facade.
        if (node.Depth == 0)
        {
            var interfaceContent = GenerateNodeInterface(
                node,
                tool,
                parentCommand,
                excludedCommands);
            var interfaceFileName = $"I{node.ClassName}.Generated.cs";
            var interfaceRelativePath = Path.Combine(tool.OutputDirectory, "Services", interfaceFileName);

            files.Add(new GeneratedFile
            {
                RelativePath = interfaceRelativePath,
                Content = interfaceContent
            });
        }

        var fileName = $"{node.ClassName}.Generated.cs";
        var relativePath = Path.Combine(tool.OutputDirectory, "Services", fileName);

        files.Add(new GeneratedFile
        {
            RelativePath = relativePath,
            Content = content
        });

        // Recursively generate files for children
        // Pass colliding command as parentCommand so it becomes ExecuteAsync() on the child
        foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
        {
            collidingCommands.TryGetValue(child.Segment, out var childParentCommand);
            GenerateFilesFromTree(
                child,
                tool,
                files,
                childParentCommand);
        }
    }

    private static string GenerateNodeInterface(
        CommandTreeNode node,
        CliToolDefinition tool,
        CliCommandDefinition? parentCommand,
        HashSet<CliCommandDefinition> excludedCommands)
    {
        var sb = new StringBuilder();

        GeneratorUtils.GenerateFileHeaderWithNullable(sb);

        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using ModularPipelines.Models;");
        sb.AppendLine("using ModularPipelines.Options;");
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");
        sb.AppendLine();
        sb.AppendLine($"namespace {tool.TargetNamespace}.Services;");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// {tool.ToolName} {node.Segment.ToLowerInvariant()} commands.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("/// <remarks>");
        sb.AppendLine("/// Nested sub-command groups are exposed as concrete services; only this top-level facade is interface-backed.");
        sb.AppendLine("/// </remarks>");
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine($"public interface I{node.ClassName}");
        sb.AppendLine("{");

        foreach (var child in node.Children.Values.OrderBy(child => child.PascalSegment))
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {tool.ToolName} {child.Segment.ToLowerInvariant()} sub-commands.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine(
                $"    {child.ClassName} {child.PascalSegment} => throw new System.NotSupportedException();");
            sb.AppendLine();
        }

        var commands = node.Commands
            .Where(command => !excludedCommands.Contains(command))
            .ToList();
        EnsureUniquePublicMemberNames(tool, node, commands, parentCommand);

        if (parentCommand is not null)
        {
            GeneratorUtils.GenerateServiceMethodSignature(sb, "Execute", parentCommand);
            sb.AppendLine();
        }

        foreach (var command in commands.OrderBy(command => command.ClassName))
        {
            var methodName = GeneratorUtils.GenerateSubDomainMethodName(
                command,
                parentCommand is not null,
                commands);
            GeneratorUtils.GenerateServiceMethodSignature(sb, methodName, command);
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateNodeClass(
        CommandTreeNode node,
        CliToolDefinition tool,
        CliCommandDefinition? parentCommand = null,
        HashSet<CliCommandDefinition>? excludedCommands = null)
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

        // Class documentation
        sb.AppendLine($"/// <summary>");
        sb.AppendLine($"/// {tool.ToolName} {node.Segment.ToLowerInvariant()} commands.");
        sb.AppendLine($"/// </summary>");
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        var interfaceClause = node.Depth == 0 ? $" : I{node.ClassName}" : string.Empty;
        sb.AppendLine($"public class {node.ClassName}{interfaceClause}");
        sb.AppendLine("{");

        AppendStateAndConstructor(sb, node);
        AppendChildProperties(sb, node, tool);
        AppendCommandMethods(sb, node, tool, parentCommand, excludedCommands);

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void AppendStateAndConstructor(StringBuilder sb, CommandTreeNode node)
    {
        sb.AppendLine("    private readonly ICommandContext _command;");

        // Child fields are populated on first property access. Generated files enable nullable analysis.
        foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
        {
            sb.AppendLine($"    private {child.ClassName}? {GetSafeFieldName(child.PascalSegment)};");
        }

        sb.AppendLine();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine($"    /// Initializes a new instance of the <see cref=\"{node.ClassName}\"/> class.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine($"    public {node.ClassName}(ICommandContext command)");
        sb.AppendLine("    {");
        sb.AppendLine("        _command = command;");
        sb.AppendLine("    }");
    }

    private static void AppendChildProperties(StringBuilder sb, CommandTreeNode node, CliToolDefinition tool)
    {
        if (node.Children.Count == 0)
        {
            return;
        }

        sb.AppendLine();
        sb.AppendLine("    #region Sub-command Groups");
        sb.AppendLine();

        foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
        {
            var fieldName = GetSafeFieldName(child.PascalSegment);
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {tool.ToolName} {child.Segment.ToLowerInvariant()} sub-commands.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public {child.ClassName} {child.PascalSegment} => {fieldName} ??= new {child.ClassName}(_command);");
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");
    }

    private static void AppendCommandMethods(
        StringBuilder sb,
        CommandTreeNode node,
        CliToolDefinition tool,
        CliCommandDefinition? parentCommand,
        HashSet<CliCommandDefinition>? excludedCommands)
    {
        var commands = excludedCommands is null
            ? node.Commands
            : node.Commands.Where(command => !excludedCommands.Contains(command)).ToList();
        EnsureUniquePublicMemberNames(tool, node, commands, parentCommand);

        if (commands.Count == 0 && parentCommand is null)
        {
            return;
        }

        sb.AppendLine();
        sb.AppendLine("    #region Commands");
        sb.AppendLine();

        if (parentCommand is not null)
        {
            GenerateExecuteMethod(sb, parentCommand);
            sb.AppendLine();
        }

        foreach (var command in commands.OrderBy(c => c.ClassName))
        {
            var methodName = GeneratorUtils.GenerateSubDomainMethodName(
                command,
                parentCommand is not null,
                commands);
            GeneratorUtils.GenerateServiceMethod(sb, methodName, command);
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");
    }

    private static void EnsureUniquePublicMemberNames(
        CliToolDefinition tool,
        CommandTreeNode node,
        IReadOnlyList<CliCommandDefinition> commands,
        CliCommandDefinition? parentCommand)
    {
        var publicMembers = node.Children.Values
            .Select(child => child.PascalSegment)
            .Concat(commands
                .Select(command => GeneratorUtils.GenerateSubDomainMethodName(
                    command,
                    parentCommand is not null,
                    commands))
                .Select(GeneratorUtils.EnsureAsyncSuffix));
        if (parentCommand is not null)
        {
            publicMembers = publicMembers.Append("ExecuteAsync");
        }

        var duplicates = publicMembers
            .GroupBy(name => name, StringComparer.OrdinalIgnoreCase)
            .Where(group => group.Skip(1).Any())
            .Select(group => group.Key)
            .ToList();
        if (duplicates.Count > 0)
        {
            throw new InvalidOperationException(
                $"The {tool.ToolName} sub-domain '{node.Segment}' generates duplicate public "
                + $"member name(s): {string.Join(", ", duplicates)}.");
        }
    }

    private static void GenerateExecuteMethod(StringBuilder sb, CliCommandDefinition command)
    {
        var description = !string.IsNullOrEmpty(command.Description)
            ? command.Description
            : "Executes the parent command directly.";
        GeneratorUtils.GenerateServiceMethod(
            sb,
            "Execute",
            command with { Description = description });
    }
}
