using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "enable-hosted-zone-dnssec")]
public record AwsRoute53EnableHostedZoneDnssecOptions(
[property: CliOption("--hosted-zone-id")] string HostedZoneId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}