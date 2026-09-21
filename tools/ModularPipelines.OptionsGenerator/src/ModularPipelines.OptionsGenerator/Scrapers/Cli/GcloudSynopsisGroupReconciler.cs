using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

internal static class GcloudSynopsisGroupReconciler
{
    public static CliArgumentGroup Reconcile(CliArgumentGroup group,
        IReadOnlyList<IReadOnlyList<IReadOnlySet<string>>> synopsisChoices,
        IReadOnlyList<CliSynopsisOptionBundle> synopsisBundles)
    {
        group = group with { Groups = [.. group.Groups.Select(child => Reconcile(child, synopsisChoices, synopsisBundles))] };
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

        var reconciled = branches.Select(branch => Bundle(Filter(group, branch) with
        {
            Description = null,
            Kind = CliArgumentGroupKind.None,
        }, synopsisBundles)).ToArray();
        bool IsDirectMember(CliArgumentGroup branch) => branch.Arguments.Count == 1 && branch.Groups.Count == 0
            && group.Arguments.Contains(branch.Arguments[0]);

        return group with
        {
            Arguments = [.. reconciled.Where(IsDirectMember).Select(branch => branch.Arguments[0])],
            Groups = [.. reconciled.Where(branch => !IsDirectMember(branch))],
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

    private static CliArgumentGroup Bundle(CliArgumentGroup branch, IReadOnlyList<CliSynopsisOptionBundle> synopsisBundles)
    {
        branch = AttachResourceSelectors(branch, synopsisBundles);
        branch = RestoreOptionalBundles(branch, synopsisBundles);
        return branch.Arguments.Count == 0 && branch.Groups.Count == 1 ? branch.Groups[0] : branch;
    }

    private static CliArgumentGroup RestoreOptionalBundles(CliArgumentGroup group, IReadOnlyList<CliSynopsisOptionBundle> bundles)
    {
        var groups = group.Groups.Select(child => RestoreOptionalBundles(child, bundles)).ToList();
        if ((group.Kind & (CliArgumentGroupKind.AtMostOne | CliArgumentGroupKind.AtLeastOne)) != 0)
        {
            return group with { Groups = groups };
        }

        var arguments = group.Arguments.ToList();
        var switches = group.FlattenArguments().Select(argument => argument.SwitchName).ToHashSet(StringComparer.Ordinal);
        // A nested optional synopsis bundle can be flattened to the same help indentation
        // as its parent. Recreate its scope before interpreting conditional requirements.
        foreach (var bundle in bundles.Where(bundle => bundle.IsOptional && bundle.OptionSwitches.Count < switches.Count)
                     .OrderByDescending(bundle => bundle.OptionSwitches.Count))
        {
            var members = arguments.Where(argument => bundle.OptionSwitches.Contains(argument.SwitchName)).ToArray();
            if (members.Length != bundle.OptionSwitches.Count)
            {
                continue;
            }

            groups.Add(RestoreOptionalBundles(new CliArgumentGroup
            {
                Kind = CliArgumentGroupKind.Optional,
                Arguments = members,
            }, bundles));
            arguments.RemoveAll(argument => bundle.OptionSwitches.Contains(argument.SwitchName));
        }

        // Required members after ':' belong to an optional sub-bundle. They become
        // mandatory only when another member of that sub-bundle is selected.
        var matching = bundles.FirstOrDefault(bundle => switches.SetEquals(bundle.OptionSwitches));
        if (matching is not null)
        {
            var required = arguments.Where(argument => matching.DirectOptionalSwitches.Contains(argument.SwitchName)
                && GcloudCliScraper.ArgumentIsConditionallyRequired(argument)).ToArray();
            if (required.Length > 0)
            {
                groups.Add(new CliArgumentGroup { Kind = CliArgumentGroupKind.Optional, Arguments = required });
                arguments.RemoveAll(required.Contains);
            }
        }

        return group with { Arguments = arguments, Groups = groups };
    }

    private static CliArgumentGroup AttachResourceSelectors(CliArgumentGroup branch, IReadOnlyList<CliSynopsisOptionBundle> synopsisBundles)
    {
        var arguments = branch.Arguments.ToList();
        var groups = branch.Groups.Select(child => AttachResourceSelectors(child, synopsisBundles)).ToList();
        for (var index = 0; index < groups.Count; index++)
        {
            var child = groups[index];
            if (!child.Kind.HasFlag(CliArgumentGroupKind.Resource)
                || (child.Kind & (CliArgumentGroupKind.AtMostOne | CliArgumentGroupKind.AtLeastOne)) != 0)
            {
                continue;
            }

            var childSwitches = child.FlattenArguments().Select(argument => argument.SwitchName).ToArray();
            var resource = synopsisBundles.OrderBy(bundle => bundle.OptionSwitches.Count)
                .FirstOrDefault(bundle => child.Arguments.Any(argument => argument.SwitchName == bundle.PrimarySwitch)
                    && childSwitches.All(bundle.OptionSwitches.Contains));
            if (resource is null)
            {
                continue;
            }

            // Only move selectors confirmed by the resource's own synopsis bundle.
            // Other flags in the outer branch do not activate this optional resource.
            var selectors = arguments.Where(argument => resource.OptionSwitches.Contains(argument.SwitchName)).ToArray();
            groups[index] = child with { Arguments = [.. child.Arguments, .. selectors] };
            arguments.RemoveAll(argument => resource.OptionSwitches.Contains(argument.SwitchName));
        }

        return branch with { Arguments = arguments, Groups = groups };
    }
}
