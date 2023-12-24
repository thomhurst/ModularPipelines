using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "list-hosted-zones-by-name")]
public record AwsRoute53ListHostedZonesByNameOptions : AwsOptions
{
    [CommandSwitch("--dns-name")]
    public string? DnsName { get; set; }

    [CommandSwitch("--hosted-zone-id")]
    public string? HostedZoneId { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}