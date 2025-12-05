using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "count-closed-workflow-executions")]
public record AwsSwfCountClosedWorkflowExecutionsOptions(
[property: CliOption("--domain")] string Domain
) : AwsOptions
{
    [CliOption("--start-time-filter")]
    public string? StartTimeFilter { get; set; }

    [CliOption("--close-time-filter")]
    public string? CloseTimeFilter { get; set; }

    [CliOption("--execution-filter")]
    public string? ExecutionFilter { get; set; }

    [CliOption("--type-filter")]
    public string? TypeFilter { get; set; }

    [CliOption("--tag-filter")]
    public string? TagFilter { get; set; }

    [CliOption("--close-status-filter")]
    public string? CloseStatusFilter { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}