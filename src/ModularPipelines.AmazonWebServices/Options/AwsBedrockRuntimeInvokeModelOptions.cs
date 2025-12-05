using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-runtime", "invoke-model")]
public record AwsBedrockRuntimeInvokeModelOptions(
[property: CliOption("--body")] string Body,
[property: CliOption("--model-id")] string ModelId
) : AwsOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--accept")]
    public string? Accept { get; set; }
}