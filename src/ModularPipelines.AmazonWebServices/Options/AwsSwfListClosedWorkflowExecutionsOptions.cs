using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "list-closed-workflow-executions")]
public record AwsSwfListClosedWorkflowExecutionsOptions(
[property: CliOption("--domain")] string Domain
) : AwsOptions
{
    [CliOption("--start-time-filter")]
    public string? StartTimeFilter { get; set; }

    [CliOption("--close-time-filter")]
    public string? CloseTimeFilter { get; set; }

    [CliOption("--execution-filter")]
    public string? ExecutionFilter { get; set; }

    [CliOption("--close-status-filter")]
    public string? CloseStatusFilter { get; set; }

    [CliOption("--type-filter")]
    public string? TypeFilter { get; set; }

    [CliOption("--tag-filter")]
    public string? TagFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}