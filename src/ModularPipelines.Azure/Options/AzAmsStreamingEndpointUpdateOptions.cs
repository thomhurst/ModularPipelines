using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "streaming-endpoint", "update")]
public record AzAmsStreamingEndpointUpdateOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [CommandSwitch("--disable-cdn")]
    public string? DisableCdn { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ips")]
    public string? Ips { get; set; }

    [CommandSwitch("--max-cache-age")]
    public string? MaxCacheAge { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

