using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "count-open-workflow-executions")]
public record AwsSwfCountOpenWorkflowExecutionsOptions(
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

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}