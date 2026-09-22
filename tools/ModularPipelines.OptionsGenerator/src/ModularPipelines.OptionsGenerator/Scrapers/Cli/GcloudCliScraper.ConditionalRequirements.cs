using System.Text.RegularExpressions;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

public partial class GcloudCliScraper
{
    private static void ApplyNamedConditionalRequirements(IReadOnlyList<CliOptionDefinition> options,
        List<CliRequiredAlternativeGroup> constraints)
    {
        var bySwitch = options.ToDictionary(option => option.SwitchName, StringComparer.Ordinal);
        var dependencies = options.SelectMany(option => GetNamedRequirements(option, bySwitch))
            .GroupBy(dependency => dependency.Trigger, StringComparer.Ordinal);
        foreach (var dependency in dependencies)
        {
            if (!bySwitch.TryGetValue(dependency.Key, out var trigger))
            {
                continue;
            }

            var required = dependency.Select(item => item.Option.PropertyName).ToHashSet(StringComparer.Ordinal);
            if (required.Contains(trigger.PropertyName))
            {
                continue;
            }

            // A configuration file can replace the inline settings containing the
            // named requirements. Retain that documented exclusive alternative.
            var alternative = constraints.SelectMany(EnumerateConstraints)
                .Where(group => group.RequiredWhen is null && group.IsChoice && group.IsMutuallyExclusive
                    && !group.PropertyNames.Contains(trigger.PropertyName)
                    && group.Members.Count + group.Groups.Count > 1
                    && group.Groups.Any(branch => required.IsSubsetOf(branch.PropertyNames)))
                .OrderBy(group => group.PropertyNames.Count).FirstOrDefault();
            var conditional = alternative is null ? new CliRequiredAlternativeGroup
            {
                IsChoice = false,
                Members = [.. dependency.DistinctBy(item => item.Option.PropertyName).Select(item => new CliRequiredAlternativeMember
                {
                    PropertyName = item.Option.PropertyName,
                    OptionSwitch = item.Option.SwitchName,
                    IsRequired = true,
                })],
            } : MarkNamedRequiredMembers(alternative, required);
            constraints.Add(conditional with
            {
                IsRequired = true,
                RequiredWhen = new() { PropertyName = trigger.PropertyName, OptionSwitch = trigger.SwitchName },
            });
        }
    }

    private static IEnumerable<(CliOptionDefinition Option, string Trigger)> GetNamedRequirements(
        CliOptionDefinition option, Dictionary<string, CliOptionDefinition> bySwitch)
    {
        foreach (Match match in NamedRequirementPattern().Matches(option.Description ?? ""))
        {
            yield return (option, match.Groups["switch"].Value);
        }

        // This wording describes a companion required by the current flag, rather
        // than a flag that activates the current option's own requirement.
        foreach (Match match in RequiredCompanionPattern().Matches(option.ValueShapeDescription ?? option.Description ?? ""))
        {
            if (bySwitch.TryGetValue(match.Groups["switch"].Value, out var companion))
            {
                yield return (companion, option.SwitchName);
            }
        }
    }

    private static CliRequiredAlternativeGroup MarkNamedRequiredMembers(CliRequiredAlternativeGroup group,
        IReadOnlySet<string> required) => group with
        {
            Members = [.. group.Members.Select(member => member with
        {
            IsRequired = member.IsRequired || required.Contains(member.PropertyName),
        })],
            Groups = [.. group.Groups.Select(child => MarkNamedRequiredMembers(child, required))],
        };

    [GeneratedRegex(@"\bRequired\s+to\s+be\s+set\s+when\s+(?<switch>--[\w-]+)\s+is\s+used\b", RegexOptions.IgnoreCase)]
    private static partial Regex NamedRequirementPattern();

    [GeneratedRegex(@"\bIf\s+specified,?\s+(?:the\s+)?(?<switch>--[\w-]+)\s+must\s+also\s+be\s+specified\b", RegexOptions.IgnoreCase)]
    private static partial Regex RequiredCompanionPattern();
}
