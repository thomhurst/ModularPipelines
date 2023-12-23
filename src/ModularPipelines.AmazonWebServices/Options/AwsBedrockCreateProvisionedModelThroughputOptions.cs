using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock", "create-provisioned-model-throughput")]
public record AwsBedrockCreateProvisionedModelThroughputOptions(
[property: CommandSwitch("--model-units")] int ModelUnits,
[property: CommandSwitch("--provisioned-model-name")] string ProvisionedModelName,
[property: CommandSwitch("--model-id")] string ModelId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--commitment-duration")]
    public string? CommitmentDuration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}