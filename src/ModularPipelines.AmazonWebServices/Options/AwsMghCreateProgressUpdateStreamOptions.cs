using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgh", "create-progress-update-stream")]
public record AwsMghCreateProgressUpdateStreamOptions(
[property: CommandSwitch("--progress-update-stream-name")] string ProgressUpdateStreamName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}