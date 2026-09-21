using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

internal static class GcloudSynopsisGroupReconciler
{
    public static CliArgumentGroup Reconcile(CliArgumentGroup group,
        IReadOnlyList<IReadOnlyList<IReadOnlySet<string>>> synopsisChoices)
    {
        group = group with { Groups = [.. group.Groups.Select(child => Reconcile(child, synopsisChoices))] };
        if ((group.Kind & (CliArgumentGroupKind.AtMostOne | CliArgumentGroupKind.AtLeastOne)) == 0)
        {
            return group;
        }

        var switches = group.FlattenArguments().Select(argument => argument.SwitchName).ToHashSet(StringComparer.Ordinal);
        var choice = synopsisChoices.FirstOrDefault(branches => branches.All(branch => branch.Count > 0)
            && switches.SetEquals(branches.SelectMany(branch => branch)));
        if (choice is null)
        {
            return group;
        }

        var branches = choice.Select(branch => branch.ToHashSet(StringComparer.Ordinal)).ToList();
        // Help can omit wrappers around a nested choice in SYNOPSIS. The documented
        // nested choice establishes that its alternatives belong to one outer branch.
        foreach (var nested in Descendants(group.Groups).Where(child =>
                     (child.Kind & (CliArgumentGroupKind.AtMostOne | CliArgumentGroupKind.AtLeastOne)) != 0))
        {
            var nestedSwitches = nested.FlattenArguments().Select(argument => argument.SwitchName).ToArray();
            var overlapping = branches.Where(branch => branch.Overlaps(nestedSwitches)).ToArray();
            if (overlapping.Length < 2)
            {
                continue;
            }

            foreach (var branch in overlapping.Skip(1))
            {
                overlapping[0].UnionWith(branch);
                branches.Remove(branch);
            }
        }

        // Only disjoint branches can safely replace indentation-based membership.
        if (branches.Count < 2 || branches.Sum(branch => branch.Count) != switches.Count)
        {
            return group;
        }

        return group with
        {
            Arguments = [],
            Groups = [.. branches.Select(branch => Bundle(Filter(group, branch) with
            {
                Description = null,
                Kind = CliArgumentGroupKind.None,
            }))],
        };
    }

    private static IEnumerable<CliArgumentGroup> Descendants(IEnumerable<CliArgumentGroup> groups) =>
        groups.SelectMany(group => new[] { group }.Concat(Descendants(group.Groups)));

    private static CliArgumentGroup Filter(CliArgumentGroup group, IReadOnlySet<string> switches) => group with
    {
        Arguments = [.. group.Arguments.Where(argument => switches.Contains(argument.SwitchName))],
        Groups = [.. group.Groups.Select(child => Filter(child, switches))
            .Where(child => child.Arguments.Count > 0 || child.Groups.Count > 0)],
    };

    private static CliArgumentGroup Bundle(CliArgumentGroup branch)
    {
        if (branch.Groups.Count != 1)
        {
            return branch;
        }

        var child = branch.Groups[0];
        if (branch.Arguments.Count == 0)
        {
            return child;
        }

        // Resource selectors can share their parent's indentation. A synopsis-confirmed
        // branch keeps those selectors with the resource's conditionally required name.
        return child.Kind.HasFlag(CliArgumentGroupKind.Resource)
            ? child with { Arguments = [.. child.Arguments, .. branch.Arguments] }
            : branch;
    }
}
