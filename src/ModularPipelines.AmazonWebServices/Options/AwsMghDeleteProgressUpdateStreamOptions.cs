using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgh", "delete-progress-update-stream")]
public record AwsMghDeleteProgressUpdateStreamOptions(
[property: CommandSwitch("--progress-update-stream-name")] string ProgressUpdateStreamName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}