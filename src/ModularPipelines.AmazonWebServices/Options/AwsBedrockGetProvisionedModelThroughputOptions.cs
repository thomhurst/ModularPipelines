using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock", "get-provisioned-model-throughput")]
public record AwsBedrockGetProvisionedModelThroughputOptions(
[property: CommandSwitch("--provisioned-model-id")] string ProvisionedModelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}