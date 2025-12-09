namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents a node in the command hierarchy tree.
/// Used to generate nested classes matching CLI command structure.
/// </summary>
public class CommandTreeNode
{
    /// <summary>
    /// The segment name (e.g., "workspace-add-ons", "deployments", "create").
    /// </summary>
    public required string Segment { get; init; }

    /// <summary>
    /// The PascalCase version of the segment (e.g., "WorkspaceAddOns", "Deployments", "Create").
    /// </summary>
    public required string PascalSegment { get; init; }

    /// <summary>
    /// The full class name for this node (e.g., "GcloudWorkspaceAddOns", "GcloudWorkspaceAddOnsDeployments").
    /// </summary>
    public required string ClassName { get; init; }

    /// <summary>
    /// Child nodes (sub-commands at the next level).
    /// </summary>
    public Dictionary<string, CommandTreeNode> Children { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Commands that are direct children of this node (leaf commands).
    /// </summary>
    public List<CliCommandDefinition> Commands { get; } = [];

    /// <summary>
    /// True if this is a leaf node with an executable command.
    /// </summary>
    public bool IsLeaf => Commands.Count > 0 && Children.Count == 0;

    /// <summary>
    /// True if this node has nested children.
    /// </summary>
    public bool HasChildren => Children.Count > 0;

    /// <summary>
    /// Depth in the tree (0 = root).
    /// </summary>
    public int Depth { get; set; }

    /// <summary>
    /// Builds a command tree from a collection of commands for a specific sub-domain.
    /// </summary>
    public static CommandTreeNode BuildTree(
        string toolPrefix,
        string subDomain,
        IReadOnlyList<CliCommandDefinition> commands)
    {
        var pascalSubDomain = ToPascalCase(subDomain);
        var rootClassName = $"{toolPrefix}{pascalSubDomain}";

        var root = new CommandTreeNode
        {
            Segment = subDomain,
            PascalSegment = pascalSubDomain,
            ClassName = rootClassName,
            Depth = 0
        };

        foreach (var command in commands)
        {
            InsertCommand(root, command, toolPrefix, 1);
        }

        return root;
    }

    private static void InsertCommand(CommandTreeNode node, CliCommandDefinition command, string toolPrefix, int partIndex)
    {
        // CommandParts are the parts after the tool name
        // e.g., for "gcloud workspace-add-ons deployments create", parts are ["workspace-add-ons", "deployments", "create"]
        var parts = command.CommandParts;

        // If we're at the last part, this command belongs to the CURRENT node (not a child)
        // The method name will be the last segment
        if (partIndex >= parts.Length - 1)
        {
            // We're at the parent of the leaf command
            node.Commands.Add(command);
            return;
        }

        // Create child nodes for intermediate segments
        var segment = parts[partIndex];
        var pascalSegment = ToPascalCase(segment);

        if (!node.Children.TryGetValue(segment, out var child))
        {
            child = new CommandTreeNode
            {
                Segment = segment,
                PascalSegment = pascalSegment,
                ClassName = $"{node.ClassName}{pascalSegment}",
                Depth = partIndex
            };
            node.Children[segment] = child;
        }

        InsertCommand(child, command, toolPrefix, partIndex + 1);
    }

    private static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var parts = input.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", parts.Select(p =>
            char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p[1..].ToLowerInvariant() : "")));
    }
}
