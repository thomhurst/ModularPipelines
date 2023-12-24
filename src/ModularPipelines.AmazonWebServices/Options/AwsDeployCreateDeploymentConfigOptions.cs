using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "create-deployment-config")]
public record AwsDeployCreateDeploymentConfigOptions(
[property: CommandSwitch("--deployment-config-name")] string DeploymentConfigName
) : AwsOptions
{
    [CommandSwitch("--minimum-healthy-hosts")]
    public string? MinimumHealthyHosts { get; set; }

    [CommandSwitch("--traffic-routing-config")]
    public string? TrafficRoutingConfig { get; set; }

    [CommandSwitch("--compute-platform")]
    public string? ComputePlatform { get; set; }

    [CommandSwitch("--zonal-config")]
    public string? ZonalConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}