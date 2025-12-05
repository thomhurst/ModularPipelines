using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "switchover-blue-green-deployment")]
public record AwsRdsSwitchoverBlueGreenDeploymentOptions(
[property: CliOption("--blue-green-deployment-identifier")] string BlueGreenDeploymentIdentifier
) : AwsOptions
{
    [CliOption("--switchover-timeout")]
    public int? SwitchoverTimeout { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}