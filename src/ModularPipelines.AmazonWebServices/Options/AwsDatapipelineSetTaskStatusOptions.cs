using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "set-task-status")]
public record AwsDatapipelineSetTaskStatusOptions(
[property: CliOption("--task-id")] string TaskId,
[property: CliOption("--task-status")] string TaskStatus
) : AwsOptions
{
    [CliOption("--error-id")]
    public string? ErrorId { get; set; }

    [CliOption("--error-message")]
    public string? ErrorMessage { get; set; }

    [CliOption("--error-stack-trace")]
    public string? ErrorStackTrace { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}