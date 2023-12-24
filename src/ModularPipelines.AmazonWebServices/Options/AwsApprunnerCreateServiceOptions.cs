using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "create-service")]
public record AwsApprunnerCreateServiceOptions(
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--source-configuration")] string SourceConfiguration
) : AwsOptions
{
    [CommandSwitch("--instance-configuration")]
    public string? InstanceConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CommandSwitch("--health-check-configuration")]
    public string? HealthCheckConfiguration { get; set; }

    [CommandSwitch("--auto-scaling-configuration-arn")]
    public string? AutoScalingConfigurationArn { get; set; }

    [CommandSwitch("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CommandSwitch("--observability-configuration")]
    public string? ObservabilityConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}