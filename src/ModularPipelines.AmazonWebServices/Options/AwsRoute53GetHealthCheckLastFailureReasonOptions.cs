using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "get-health-check-last-failure-reason")]
public record AwsRoute53GetHealthCheckLastFailureReasonOptions(
[property: CliOption("--health-check-id")] string HealthCheckId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}