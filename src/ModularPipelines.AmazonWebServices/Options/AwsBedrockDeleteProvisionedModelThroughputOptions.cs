using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock", "delete-provisioned-model-throughput")]
public record AwsBedrockDeleteProvisionedModelThroughputOptions(
[property: CommandSwitch("--provisioned-model-id")] string ProvisionedModelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}