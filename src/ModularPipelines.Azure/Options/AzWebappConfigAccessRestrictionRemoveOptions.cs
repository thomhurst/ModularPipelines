using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "access-restriction", "remove")]
public record AzWebappConfigAccessRestrictionRemoveOptions : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [BooleanCommandSwitch("--scm-site")]
    public bool? ScmSite { get; set; }

    [CommandSwitch("--service-tag")]
    public string? ServiceTag { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}