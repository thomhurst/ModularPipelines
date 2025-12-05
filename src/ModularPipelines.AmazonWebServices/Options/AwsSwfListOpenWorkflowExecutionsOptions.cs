using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "list-open-workflow-executions")]
public record AwsSwfListOpenWorkflowExecutionsOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--start-time-filter")] string StartTimeFilter
) : AwsOptions
{
    [CliOption("--type-filter")]
    public string? TypeFilter { get; set; }

    [CliOption("--tag-filter")]
    public string? TagFilter { get; set; }

    [CliOption("--execution-filter")]
    public string? ExecutionFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}