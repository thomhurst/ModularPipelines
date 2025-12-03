using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "report-task-progress")]
public record AwsDatapipelineReportTaskProgressOptions(
[property: CliOption("--task-id")] string TaskId
) : AwsOptions
{
    [CliOption("--fields")]
    public string[]? Fields { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}