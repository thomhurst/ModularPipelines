using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "local-rulestack", "create")]
public record AzPaloAltoCloudngfwLocalRulestackCreateOptions(
[property: CommandSwitch("--local-rulestack-name")] string LocalRulestackName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--associated-subs")]
    public string? AssociatedSubs { get; set; }

    [CommandSwitch("--default-mode")]
    public string? DefaultMode { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--min-app-id-version")]
    public string? MinAppIdVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pan-etag")]
    public string? PanEtag { get; set; }

    [CommandSwitch("--pan-location")]
    public string? PanLocation { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--security-services")]
    public string? SecurityServices { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}