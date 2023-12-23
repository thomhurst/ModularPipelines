using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "switchover-blue-green-deployment")]
public record AwsRdsSwitchoverBlueGreenDeploymentOptions(
[property: CommandSwitch("--blue-green-deployment-identifier")] string BlueGreenDeploymentIdentifier
) : AwsOptions
{
    [CommandSwitch("--switchover-timeout")]
    public int? SwitchoverTimeout { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}