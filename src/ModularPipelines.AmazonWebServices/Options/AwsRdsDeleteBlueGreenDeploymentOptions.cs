using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "delete-blue-green-deployment")]
public record AwsRdsDeleteBlueGreenDeploymentOptions(
[property: CommandSwitch("--blue-green-deployment-identifier")] string BlueGreenDeploymentIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}