using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "create-hosted-zone")]
public record AwsRoute53CreateHostedZoneOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--caller-reference")] string CallerReference
) : AwsOptions
{
    [CliOption("--vpc")]
    public string? Vpc { get; set; }

    [CliOption("--hosted-zone-config")]
    public string? HostedZoneConfig { get; set; }

    [CliOption("--delegation-set-id")]
    public string? DelegationSetId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}