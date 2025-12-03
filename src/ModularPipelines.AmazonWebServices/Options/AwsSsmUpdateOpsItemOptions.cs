using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-ops-item")]
public record AwsSsmUpdateOpsItemOptions(
[property: CliOption("--ops-item-id")] string OpsItemId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--operational-data")]
    public IEnumerable<KeyValue>? OperationalData { get; set; }

    [CliOption("--operational-data-to-delete")]
    public string[]? OperationalDataToDelete { get; set; }

    [CliOption("--notifications")]
    public string[]? Notifications { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--related-ops-items")]
    public string[]? RelatedOpsItems { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--actual-start-time")]
    public long? ActualStartTime { get; set; }

    [CliOption("--actual-end-time")]
    public long? ActualEndTime { get; set; }

    [CliOption("--planned-start-time")]
    public long? PlannedStartTime { get; set; }

    [CliOption("--planned-end-time")]
    public long? PlannedEndTime { get; set; }

    [CliOption("--ops-item-arn")]
    public string? OpsItemArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}