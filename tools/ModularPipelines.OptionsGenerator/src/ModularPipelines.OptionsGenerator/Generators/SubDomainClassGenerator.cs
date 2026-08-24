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
                GeneratorUtils.GetCommandGroupAliases(tool, subDomainIdentifier),
                parentCommand);
        }

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
    }

    private static void GenerateFilesFromTree(
        CommandTreeNode node,
        CliToolDefinition tool,
        List<GeneratedFile> files,
        IReadOnlyList<CliCommandGroupAlias> commandGroupAliases,
        CliCommandDefinition? parentCommand = null)
    {
        parentCommand ??= tool.Commands.FirstOrDefault(command =>
            command.PreserveExecuteFacade
            && command.ClassName.Equals($"{node.ClassName}Options", StringComparison.Ordinal));

        // Build map of commands that collide with child property names
        // These will become ExecuteAsync() methods on the child classes instead
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

        var excludedCommands = node.Commands
            .Where(command => !node.EmitsNamedFacade(command))
            .ToHashSet();

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

        foreach (var alias in commandGroupAliases)
        {
            if (node.Depth == 0)
            {
                files.Add(GenerateCompatibilityInterface(
                    node,
                    tool,
                    alias,
                    parentCommand,
                    excludedCommands));
            }

            files.Add(GenerateCompatibilityClass(
                node,
                tool,
                alias,
                parentCommand,
                excludedCommands));
        }

        // Recursively generate files for children
        // Pass colliding command as parentCommand so it becomes ExecuteAsync() on the child
        foreach (var child in node.Children.Values.OrderBy(c => c.PascalSegment))
        {
            collidingCommands.TryGetValue(child.Segment, out var childParentCommand);
            GenerateFilesFromTree(
                child,
                tool,
                files,
                commandGroupAliases,
                childParentCommand);
        }
    }

    private static GeneratedFile GenerateCompatibilityInterface(
        CommandTreeNode node,
        CliToolDefinition tool,
        CliCommandGroupAlias alias,
        CliCommandDefinition? parentCommand,
        HashSet<CliCommandDefinition> excludedCommands)
    {
        var aliasClassName = GeneratorUtils.GetAliasedClassName(
            tool,
            alias,
            node.ClassName);
        var sb = new StringBuilder();
        GeneratorUtils.GenerateFileHeaderWithNullable(sb);
        sb.AppendLine("#pragma warning disable CS0618 // Compatibility facade references obsolete alias wrappers.");
        sb.AppendLine();
        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using ModularPipelines.Models;");
        sb.AppendLine("using ModularPipelines.Options;");
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");
        sb.AppendLine();
        sb.AppendLine($"namespace {tool.TargetNamespace}.Services;");
        sb.AppendLine();
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine($"public interface I{aliasClassName}");
        sb.AppendLine("{");

        foreach (var child in node.Children.Values.OrderBy(child => child.PascalSegment))
        {
            var aliasChildClassName = GeneratorUtils.GetAliasedClassName(
                tool,
                alias,
                child.ClassName);
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {tool.ToolName} {child.Segment.ToLowerInvariant()} sub-commands.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    {aliasChildClassName} {child.PascalSegment} {{ get; }}");
            sb.AppendLine();
        }

        if (parentCommand is not null)
        {
            GenerateCompatibilityMethodSignature(sb, tool, alias, "Execute", parentCommand);
            sb.AppendLine();
        }

        foreach (var (command, methodName) in GetCompatibilityCommands(
                     node,
                     parentCommand,
                     excludedCommands))
        {
            GenerateCompatibilityMethodSignature(sb, tool, alias, methodName, command);
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return new GeneratedFile
        {
            RelativePath = Path.Combine(
                tool.OutputDirectory,
                "Services",
                $"I{aliasClassName}.Generated.cs"),
            Content = sb.ToString(),
        };
    }

    private static GeneratedFile GenerateCompatibilityClass(
        CommandTreeNode node,
        CliToolDefinition tool,
        CliCommandGroupAlias alias,
        CliCommandDefinition? parentCommand,
        HashSet<CliCommandDefinition> excludedCommands)
    {
        var aliasClassName = GeneratorUtils.GetAliasedClassName(
            tool,
            alias,
            node.ClassName);
        var sb = new StringBuilder();
        GeneratorUtils.GenerateFileHeaderWithNullable(sb);
        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using ModularPipelines.Context.Domains.Shell;");
        sb.AppendLine("using ModularPipelines.Models;");
        sb.AppendLine("using ModularPipelines.Options;");
        sb.AppendLine($"using {tool.TargetNamespace}.Options;");
        sb.AppendLine();
        sb.AppendLine($"namespace {tool.TargetNamespace}.Services;");
        sb.AppendLine();
        sb.AppendLine($"[Obsolete({GeneratorUtils.FormatStringLiteral(alias.ObsoleteMessage)})]");
        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        var interfaceClause = node.Depth == 0 ? $", I{aliasClassName}" : string.Empty;
        sb.AppendLine($"public class {aliasClassName} : {node.ClassName}{interfaceClause}");
        sb.AppendLine("{");
        sb.AppendLine("    private readonly ICommandContext _command;");

        foreach (var child in node.Children.Values.OrderBy(child => child.PascalSegment))
        {
            var aliasChildClassName = GeneratorUtils.GetAliasedClassName(
                tool,
                alias,
                child.ClassName);
            var fieldName = GetSafeFieldName(child.PascalSegment);
            sb.AppendLine($"    private {aliasChildClassName}? {fieldName};");
        }

        sb.AppendLine();
        sb.AppendLine($"    public {aliasClassName}(ICommandContext command)");
        sb.AppendLine("        : base(command)");
        sb.AppendLine("    {");
        sb.AppendLine("        _command = command;");
        sb.AppendLine("    }");

        foreach (var child in node.Children.Values.OrderBy(child => child.PascalSegment))
        {
            var aliasChildClassName = GeneratorUtils.GetAliasedClassName(
                tool,
                alias,
                child.ClassName);
            var fieldName = GetSafeFieldName(child.PascalSegment);
            sb.AppendLine();
            sb.AppendLine($"    public new {aliasChildClassName} {child.PascalSegment} =>");
            sb.AppendLine($"        {fieldName} ??= new {aliasChildClassName}(_command);");
        }

        if (parentCommand is not null)
        {
            sb.AppendLine();
            GenerateCompatibilityForwardingMethod(
                sb,
                tool,
                alias,
                "Execute",
                parentCommand);
        }

        foreach (var (command, methodName) in GetCompatibilityCommands(
                     node,
                     parentCommand,
                     excludedCommands))
        {
            sb.AppendLine();
            GenerateCompatibilityForwardingMethod(
                sb,
                tool,
                alias,
                methodName,
                command);
        }

        sb.AppendLine("}");

        return new GeneratedFile
        {
            RelativePath = Path.Combine(
                tool.OutputDirectory,
                "Services",
                $"{aliasClassName}.Generated.cs"),
            Content = sb.ToString(),
        };
    }

    private static IEnumerable<(CliCommandDefinition Command, string MethodName)> GetCompatibilityCommands(
        CommandTreeNode node,
        CliCommandDefinition? parentCommand,
        HashSet<CliCommandDefinition> excludedCommands)
    {
        var commands = node.Commands
            .Where(command => !excludedCommands.Contains(command))
            .ToList();
        return commands
            .OrderBy(command => command.ClassName)
            .Select(command => (
                command,
                GeneratorUtils.GenerateSubDomainMethodName(
                    command,
                    parentCommand is not null,
                    commands)));
    }

    private static void GenerateCompatibilityMethodSignature(
        StringBuilder sb,
        CliToolDefinition tool,
        CliCommandGroupAlias alias,
        string methodName,
        CliCommandDefinition command)
    {
        methodName = GeneratorUtils.EnsureAsyncSuffix(methodName);

        if (!string.IsNullOrEmpty(command.Description))
        {
            GeneratorUtils.GenerateXmlDocumentation(sb, command.Description);
            sb.AppendLine("    /// <param name=\"options\">The command options.</param>");
            sb.AppendLine("    /// <param name=\"executionOptions\">The execution configuration options.</param>");
            sb.AppendLine("    /// <param name=\"cancellationToken\">Cancellation token.</param>");
            sb.AppendLine("    /// <returns>The command result.</returns>");
        }

        sb.AppendLine(
            $"    Task<CommandResult> {methodName}("
            + $"{BuildCompatibilityOptionsParameter(tool, alias, command)}, "
            + $"{GeneratorUtils.ExecutionOptionsParameter}, "
            + "CancellationToken cancellationToken = default);");

        foreach (var compatibilityMethod in GeneratorUtils.GetCompatibilityMethods(command, methodName))
        {
            sb.AppendLine();
            sb.AppendLine(
                $"    [Obsolete({GeneratorUtils.FormatStringLiteral(compatibilityMethod.ObsoleteMessage)})]");
            sb.AppendLine(
                $"    Task<CommandResult> {compatibilityMethod.MethodName}("
                + $"{BuildCompatibilityOptionsParameter(tool, alias, command)}, "
                + $"{GeneratorUtils.ExecutionOptionsParameter}, "
                + "CancellationToken cancellationToken = default);");
        }
    }

    private static void GenerateCompatibilityForwardingMethod(
        StringBuilder sb,
        CliToolDefinition tool,
        CliCommandGroupAlias alias,
        string methodName,
        CliCommandDefinition command)
    {
        methodName = GeneratorUtils.EnsureAsyncSuffix(methodName);

        sb.AppendLine($"    public virtual Task<CommandResult> {methodName}(");
        sb.AppendLine($"        {BuildCompatibilityOptionsParameter(tool, alias, command)},");
        sb.AppendLine($"        {GeneratorUtils.ExecutionOptionsParameter},");
        sb.AppendLine("        CancellationToken cancellationToken = default)");
        sb.AppendLine("    {");
        sb.AppendLine(
            $"        return base.{methodName}(options, executionOptions, cancellationToken);");
        sb.AppendLine("    }");

        foreach (var compatibilityMethod in GeneratorUtils.GetCompatibilityMethods(command, methodName))
        {
            sb.AppendLine();
            sb.AppendLine(
                $"    [Obsolete({GeneratorUtils.FormatStringLiteral(compatibilityMethod.ObsoleteMessage)})]");
            sb.AppendLine($"    public virtual Task<CommandResult> {compatibilityMethod.MethodName}(");
            sb.AppendLine($"        {BuildCompatibilityOptionsParameter(tool, alias, command)},");
            sb.AppendLine($"        {GeneratorUtils.ExecutionOptionsParameter},");
            sb.AppendLine("        CancellationToken cancellationToken = default)");
            sb.AppendLine("    {");
            sb.AppendLine(
                $"        return base.{compatibilityMethod.MethodName}(options, executionOptions, cancellationToken);");
            sb.AppendLine("    }");
        }
    }

    private static string BuildCompatibilityOptionsParameter(
        CliToolDefinition tool,
        CliCommandGroupAlias alias,
        CliCommandDefinition command)
    {
        var aliasOptionsClassName = GeneratorUtils.GetAliasedClassName(
            tool,
            alias,
            command.ClassName);
        return GeneratorUtils.RequiresOptionsParameter(command)
            ? $"{aliasOptionsClassName} options"
            : $"{aliasOptionsClassName}? options = null";
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
