using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-app-block-builder-streaming-url")]
public record AwsAppstreamCreateAppBlockBuilderStreamingUrlOptions(
[property: CommandSwitch("--app-block-builder-name")] string AppBlockBuilderName
) : AwsOptions
{
    [CommandSwitch("--validity")]
    public long? Validity { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}