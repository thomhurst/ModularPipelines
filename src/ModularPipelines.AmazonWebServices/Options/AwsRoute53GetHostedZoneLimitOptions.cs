using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "get-hosted-zone-limit")]
public record AwsRoute53GetHostedZoneLimitOptions(
[property: CliOption("--type")] string Type,
[property: CliOption("--hosted-zone-id")] string HostedZoneId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}