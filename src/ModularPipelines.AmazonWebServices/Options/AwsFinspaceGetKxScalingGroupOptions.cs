using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "get-kx-scaling-group")]
public record AwsFinspaceGetKxScalingGroupOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--scaling-group-name")] string ScalingGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}