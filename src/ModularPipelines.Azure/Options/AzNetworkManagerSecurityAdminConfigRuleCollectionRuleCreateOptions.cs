using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "security-admin-config", "rule-collection", "rule", "create")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionRuleCreateOptions(
[property: CommandSwitch("--access")] string Access,
[property: CommandSwitch("--configuration-name")] string ConfigurationName,
[property: CommandSwitch("--direction")] string Direction,
[property: CommandSwitch("--kind")] string Kind,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-collection-name")] string RuleCollectionName,
[property: CommandSwitch("--rule-name")] string RuleName
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--dest-port-ranges")]
    public string? DestPortRanges { get; set; }

    [CommandSwitch("--destinations")]
    public string? Destinations { get; set; }

    [CommandSwitch("--flag")]
    public string? Flag { get; set; }

    [CommandSwitch("--source-port-ranges")]
    public string? SourcePortRanges { get; set; }

    [CommandSwitch("--sources")]
    public string? Sources { get; set; }
}

