using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "create-service")]
public record AwsApprunnerCreateServiceOptions(
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--source-configuration")] string SourceConfiguration
) : AwsOptions
{
    [CliOption("--instance-configuration")]
    public string? InstanceConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CliOption("--health-check-configuration")]
    public string? HealthCheckConfiguration { get; set; }

    [CliOption("--auto-scaling-configuration-arn")]
    public string? AutoScalingConfigurationArn { get; set; }

    [CliOption("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CliOption("--observability-configuration")]
    public string? ObservabilityConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}