using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "create-health-check")]
public record AwsRoute53CreateHealthCheckOptions(
[property: CliOption("--caller-reference")] string CallerReference,
[property: CliOption("--health-check-config")] string HealthCheckConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}