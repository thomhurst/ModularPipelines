using ModularPipelines.Models;
using Spectre.Console;

namespace ModularPipelines.Engine;

/// <summary>
/// Formats dependency trees using Spectre.Console Tree widget.
/// Creates a visual hierarchy with proper connectors, colors, and DAG support.
/// </summary>
/// <example>
/// <code>
/// // Example output:
/// // Module Dependencies
/// // ├── BuildModule
/// // │   └── CompileModule
/// // │       └── TestModule
/// // └── DeployModule
/// //     └── TestModule (↑)
/// //
/// var formatter = new DependencyTreeFormatter();
/// var tree = formatter.FormatTree(rootModules);
/// AnsiConsole.Write(tree);
/// </code>
/// </example>
internal class DependencyTreeFormatter : IDependencyTreeFormatter
{
    public Tree FormatTree(IEnumerable<ModuleDependencyModel> rootModules)
    {
        var tree = new Tree("[bold]Module Dependencies[/]");
        var alreadyPrinted = new HashSet<ModuleDependencyModel>();

        foreach (var rootModule in rootModules.OrderBy(m => m.AllDescendantDependencies().Count()))
        {
            if (alreadyPrinted.Contains(rootModule))
            {
                continue;
            }

            AddNode(tree, rootModule, alreadyPrinted);
        }

        return tree;
    }

    private void AddNode(IHasTreeNodes parent, ModuleDependencyModel node, HashSet<ModuleDependencyModel> alreadyPrinted)
    {
        var isReference = alreadyPrinted.Contains(node);
        alreadyPrinted.Add(node);

        var moduleName = node.Module.GetType().Name;
        var label = isReference
            ? $"[dim italic]{moduleName}[/] [grey](↑)[/]"
            : $"[blue]{moduleName}[/]";

        var treeNode = parent.AddNode(label);

        // Don't recurse into references - their children are shown elsewhere
        if (!isReference)
        {
            foreach (var dependent in node.IsDependencyFor)
            {
                AddNode(treeNode, dependent, alreadyPrinted);
            }
        }
    }
}

/// <summary>
/// Interface for formatting dependency trees.
/// </summary>
internal interface IDependencyTreeFormatter
{
    /// <summary>
    /// Formats a collection of root dependency modules as a Spectre.Console tree.
    /// </summary>
    /// <param name="rootModules">The root modules to format.</param>
    /// <returns>A Spectre.Console Tree representing the dependency hierarchy.</returns>
    Tree FormatTree(IEnumerable<ModuleDependencyModel> rootModules);
}