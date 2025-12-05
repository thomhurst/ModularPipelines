using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "streaming-endpoint", "update")]
public record AzAmsStreamingEndpointUpdateOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--cdn-profile")]
    public string? CdnProfile { get; set; }

    [CliOption("--cdn-provider")]
    public string? CdnProvider { get; set; }

    [CliOption("--client-access-policy")]
    public string? ClientAccessPolicy { get; set; }

    [CliOption("--cross-domain-policy")]
    public string? CrossDomainPolicy { get; set; }

    [CliOption("--custom-host-names")]
    public string? CustomHostNames { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disable-cdn")]
    public bool? DisableCdn { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ips")]
    public string? Ips { get; set; }

    [CliOption("--max-cache-age")]
    public string? MaxCacheAge { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}