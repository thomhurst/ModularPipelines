using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "list-hosted-zones-by-name")]
public record AwsRoute53ListHostedZonesByNameOptions : AwsOptions
{
    [CliOption("--dns-name")]
    public string? DnsName { get; set; }

    [CliOption("--hosted-zone-id")]
    public string? HostedZoneId { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}