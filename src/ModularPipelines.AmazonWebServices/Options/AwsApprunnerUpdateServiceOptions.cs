using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "update-service")]
public record AwsApprunnerUpdateServiceOptions(
[property: CliOption("--service-arn")] string ServiceArn
) : AwsOptions
{
    [CliOption("--source-configuration")]
    public string? SourceConfiguration { get; set; }

    [CliOption("--instance-configuration")]
    public string? InstanceConfiguration { get; set; }

    [CliOption("--auto-scaling-configuration-arn")]
    public string? AutoScalingConfigurationArn { get; set; }

    [CliOption("--health-check-configuration")]
    public string? HealthCheckConfiguration { get; set; }

    [CliOption("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CliOption("--observability-configuration")]
    public string? ObservabilityConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}