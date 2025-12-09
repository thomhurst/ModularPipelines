using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "respond-activity-task-completed")]
public record AwsSwfRespondActivityTaskCompletedOptions(
[property: CliOption("--task-token")] string TaskToken
) : AwsOptions
{
    [CliOption("--result")]
    public string? Result { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}