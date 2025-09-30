using System.Text;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Formats dependency trees as text with visual tree structure.
/// Creates a hierarchical representation using box-drawing characters.
/// </summary>
/// <example>
/// <code>
/// // Example output:
/// // ▶ RootModule
/// //   └─ DependentModule1
/// //     └─ DependentModule2
/// //   └─ DependentModule3
/// //
/// var formatter = new DependencyTreeFormatter();
/// var tree = formatter.FormatTree(rootModules);
/// Console.WriteLine(tree);
/// </code>
/// </example>
internal class DependencyTreeFormatter : IDependencyTreeFormatter
{
    public string FormatTree(IEnumerable<ModuleDependencyModel> rootModules)
    {
        var stringBuilder = new StringBuilder();
        var alreadyPrinted = new HashSet<ModuleDependencyModel>();

        foreach (var rootModule in rootModules.OrderBy(m => m.AllDescendantDependencies().Count()))
        {
            if (alreadyPrinted.Contains(rootModule))
            {
                continue;
            }

            stringBuilder.AppendLine();
            AppendNode(stringBuilder, rootModule, 1, alreadyPrinted);
        }

        return stringBuilder.ToString();
    }

    private void AppendNode(StringBuilder sb, ModuleDependencyModel node, int depth, ISet<ModuleDependencyModel> alreadyPrinted)
    {
        alreadyPrinted.Add(node);

        var indent = new string(' ', (depth - 1) * 2);
        var prefix = depth == 1 ? "▶ " : "└─ ";

        sb.Append(indent);
        sb.Append(prefix);
        sb.AppendLine(node.Module.GetType().Name);

        foreach (var dependent in node.IsDependencyFor)
        {
            AppendNode(sb, dependent, depth + 1, alreadyPrinted);
        }
    }
}

/// <summary>
/// Interface for formatting dependency trees.
/// </summary>
internal interface IDependencyTreeFormatter
{
    /// <summary>
    /// Formats a collection of root dependency modules as a tree structure.
    /// </summary>
    /// <param name="rootModules">The root modules to format.</param>
    /// <returns>A formatted string representation of the dependency tree.</returns>
    string FormatTree(IEnumerable<ModuleDependencyModel> rootModules);
}