using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock", "update-provisioned-model-throughput")]
public record AwsBedrockUpdateProvisionedModelThroughputOptions(
[property: CommandSwitch("--provisioned-model-id")] string ProvisionedModelId
) : AwsOptions
{
    [CommandSwitch("--desired-provisioned-model-name")]
    public string? DesiredProvisionedModelName { get; set; }

    [CommandSwitch("--desired-model-id")]
    public string? DesiredModelId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}