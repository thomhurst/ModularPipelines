using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "get-flow-association")]
public record AwsConnectGetFlowAssociationOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}