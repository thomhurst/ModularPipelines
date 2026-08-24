using ModularPipelines.OptionsGenerator.Generators;

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
        // The caller resolves legacy casing or an explicit scraper override.
        var rootClassName = $"{toolPrefix}{subDomain}";

        var root = new CommandTreeNode
        {
            Segment = subDomain,
            PascalSegment = subDomain,
            ClassName = rootClassName,
            Depth = 0
        };

        PopulateNode(root, commands, 1);

        return root;
    }

    private static void PopulateNode(
        CommandTreeNode node,
        IReadOnlyList<CliCommandDefinition> commands,
        int partIndex)
    {
        foreach (var command in commands.Where(command => partIndex >= command.CommandParts.Length - 1))
        {
            node.Commands.Add(command);
        }

        var childCommandGroups = commands
            .Where(command => partIndex < command.CommandParts.Length - 1)
            .GroupBy(command => command.CommandParts[partIndex], StringComparer.OrdinalIgnoreCase);
        foreach (var childCommandGroup in childCommandGroups)
        {
            var segment = childCommandGroup.Key;
            var childCommands = childCommandGroup.ToArray();
            var identifierOverrides = childCommands
                .Select(command => command.CommandPartIdentifierOverrides.TryGetValue(
                    partIndex,
                    out var identifierOverride)
                        ? identifierOverride
                        : null)
                .OfType<string>()
                .Distinct(StringComparer.Ordinal)
                .ToArray();
            if (identifierOverrides.Length > 1)
            {
                throw new InvalidOperationException(
                    $"Command part '{segment}' has conflicting generated identifiers: "
                    + $"{string.Join(", ", identifierOverrides)}.");
            }

            var pascalSegment = identifierOverrides.SingleOrDefault() ?? ToPascalCase(segment);
            var child = new CommandTreeNode
            {
                Segment = segment,
                PascalSegment = pascalSegment,
                ClassName = $"{node.ClassName}{pascalSegment}",
                Depth = partIndex
            };
            node.Children[segment] = child;
            PopulateNode(child, childCommands, partIndex + 1);
        }
    }

    private static string ToPascalCase(string input) => GeneratorUtils.ToPascalCase(input);
}
