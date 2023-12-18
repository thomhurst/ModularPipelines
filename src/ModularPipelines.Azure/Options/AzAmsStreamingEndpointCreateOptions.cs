using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "streaming-endpoint", "create")]
public record AzAmsStreamingEndpointCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--scale-units")] string ScaleUnits
) : AzOptions
{
    [CommandSwitch("--auto-start")]
    public string? AutoStart { get; set; }

    [CommandSwitch("--availability-set-name")]
    public string? AvailabilitySetName { get; set; }

    [CommandSwitch("--cdn-profile")]
    public string? CdnProfile { get; set; }

    [CommandSwitch("--cdn-provider")]
    public string? CdnProvider { get; set; }

    [CommandSwitch("--client-access-policy")]
    public string? ClientAccessPolicy { get; set; }

    [CommandSwitch("--cross-domain-policy")]
    public string? CrossDomainPolicy { get; set; }

    [CommandSwitch("--custom-host-names")]
    public string? CustomHostNames { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--ips")]
    public string? Ips { get; set; }

    [CommandSwitch("--max-cache-age")]
    public string? MaxCacheAge { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}