using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "update-service")]
public record AwsApprunnerUpdateServiceOptions(
[property: CommandSwitch("--service-arn")] string ServiceArn
) : AwsOptions
{
    [CommandSwitch("--source-configuration")]
    public string? SourceConfiguration { get; set; }

    [CommandSwitch("--instance-configuration")]
    public string? InstanceConfiguration { get; set; }

    [CommandSwitch("--auto-scaling-configuration-arn")]
    public string? AutoScalingConfigurationArn { get; set; }

    [CommandSwitch("--health-check-configuration")]
    public string? HealthCheckConfiguration { get; set; }

    [CommandSwitch("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CommandSwitch("--observability-configuration")]
    public string? ObservabilityConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}