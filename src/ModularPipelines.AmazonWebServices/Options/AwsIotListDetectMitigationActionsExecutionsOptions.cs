using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "list-detect-mitigation-actions-executions")]
public record AwsIotListDetectMitigationActionsExecutionsOptions : AwsOptions
{
    [CliOption("--task-id")]
    public string? TaskId { get; set; }

    [CliOption("--violation-id")]
    public string? ViolationId { get; set; }

    [CliOption("--thing-name")]
    public string? ThingName { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}