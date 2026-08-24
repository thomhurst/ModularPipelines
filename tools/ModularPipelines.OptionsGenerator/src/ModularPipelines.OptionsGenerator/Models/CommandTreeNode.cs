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
    /// Commands that move to <c>ExecuteAsync</c> on matching child command groups.
    /// </summary>
    internal IReadOnlyDictionary<string, CliCommandDefinition> GetChildParentCommands()
    {
        var collisions = Commands
            .Select(command => new
            {
                Command = command,
                Child = Children.Values.FirstOrDefault(child => child.PascalSegment.Equals(
                    GeneratorUtils.GenerateMethodNameFromLastCommandPart(command),
                    StringComparison.OrdinalIgnoreCase)),
            })
            .Where(collision => collision.Child is not null)
            .ToList();
        var ambiguousCollision = collisions
            .GroupBy(collision => collision.Child!.Segment, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Skip(1).Any());
        if (ambiguousCollision is not null)
        {
            throw new InvalidOperationException(
                $"Command group '{ambiguousCollision.Key}' has multiple executable parent commands: "
                + $"{string.Join(", ", ambiguousCollision.Select(collision => collision.Command.FullCommand))}.");
        }

        return collisions.ToDictionary(
            collision => collision.Child!.Segment,
            collision => collision.Command,
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Direct commands that remain named facades on this node.
    /// </summary>
    internal IReadOnlyList<CliCommandDefinition> GetNamedFacadeCommands()
    {
        var movedCommands = GetChildParentCommands().Values
            .Where(command => !command.PreserveNamedFacade)
            .ToHashSet();
        return Commands.Where(command => !movedCommands.Contains(command)).ToList();
    }

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
            var pascalSegment = ToPascalCase(segment);
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
