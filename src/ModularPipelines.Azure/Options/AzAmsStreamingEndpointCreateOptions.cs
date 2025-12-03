using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "streaming-endpoint", "create")]
public record AzAmsStreamingEndpointCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scale-units")] string ScaleUnits
) : AzOptions
{
    [CliOption("--auto-start")]
    public string? AutoStart { get; set; }

    [CliOption("--availability-set-name")]
    public string? AvailabilitySetName { get; set; }

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

    [CliOption("--ips")]
    public string? Ips { get; set; }

    [CliOption("--max-cache-age")]
    public string? MaxCacheAge { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}