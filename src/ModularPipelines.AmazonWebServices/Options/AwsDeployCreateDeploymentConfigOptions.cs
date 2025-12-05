using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "create-deployment-config")]
public record AwsDeployCreateDeploymentConfigOptions(
[property: CliOption("--deployment-config-name")] string DeploymentConfigName
) : AwsOptions
{
    [CliOption("--minimum-healthy-hosts")]
    public string? MinimumHealthyHosts { get; set; }

    [CliOption("--traffic-routing-config")]
    public string? TrafficRoutingConfig { get; set; }

    [CliOption("--compute-platform")]
    public string? ComputePlatform { get; set; }

    [CliOption("--zonal-config")]
    public string? ZonalConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}