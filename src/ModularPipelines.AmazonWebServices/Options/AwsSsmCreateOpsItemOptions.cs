using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "create-ops-item")]
public record AwsSsmCreateOpsItemOptions(
[property: CliOption("--description")] string Description,
[property: CliOption("--source")] string Source,
[property: CliOption("--title")] string Title
) : AwsOptions
{
    [CliOption("--ops-item-type")]
    public string? OpsItemType { get; set; }

    [CliOption("--operational-data")]
    public IEnumerable<KeyValue>? OperationalData { get; set; }

    [CliOption("--notifications")]
    public string[]? Notifications { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--related-ops-items")]
    public string[]? RelatedOpsItems { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

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

    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}