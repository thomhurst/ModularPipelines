using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "manager", "security-admin-config", "rule-collection", "rule", "create")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionRuleCreateOptions(
[property: CliOption("--access")] string Access,
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--direction")] string Direction,
[property: CliOption("--kind")] string Kind,
[property: CliOption("--name")] string Name,
[property: CliOption("--priority")] string Priority,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-collection-name")] string RuleCollectionName,
[property: CliOption("--rule-name")] string RuleName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dest-port-ranges")]
    public string? DestPortRanges { get; set; }

    [CliOption("--destinations")]
    public string? Destinations { get; set; }

    [CliOption("--flag")]
    public string? Flag { get; set; }

    [CliOption("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }

    [CliOption("--sources")]
    public string? Sources { get; set; }
}