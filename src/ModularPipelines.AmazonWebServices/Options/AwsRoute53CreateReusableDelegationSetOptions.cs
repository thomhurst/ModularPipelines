using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "create-reusable-delegation-set")]
public record AwsRoute53CreateReusableDelegationSetOptions(
[property: CliOption("--caller-reference")] string CallerReference
) : AwsOptions
{
    [CliOption("--hosted-zone-id")]
    public string? HostedZoneId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}