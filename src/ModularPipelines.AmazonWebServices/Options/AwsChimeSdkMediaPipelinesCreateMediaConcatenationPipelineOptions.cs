using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "create-media-concatenation-pipeline")]
public record AwsChimeSdkMediaPipelinesCreateMediaConcatenationPipelineOptions(
[property: CommandSwitch("--sources")] string[] Sources,
[property: CommandSwitch("--sinks")] string[] Sinks
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}