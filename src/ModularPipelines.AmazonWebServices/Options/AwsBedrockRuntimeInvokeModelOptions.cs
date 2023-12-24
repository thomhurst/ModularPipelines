using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-runtime", "invoke-model")]
public record AwsBedrockRuntimeInvokeModelOptions(
[property: CommandSwitch("--body")] string Body,
[property: CommandSwitch("--model-id")] string ModelId
) : AwsOptions
{
    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--accept")]
    public string? Accept { get; set; }
}