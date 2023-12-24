using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-ops-item")]
public record AwsSsmUpdateOpsItemOptions(
[property: CommandSwitch("--ops-item-id")] string OpsItemId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--operational-data")]
    public IEnumerable<KeyValue>? OperationalData { get; set; }

    [CommandSwitch("--operational-data-to-delete")]
    public string[]? OperationalDataToDelete { get; set; }

    [CommandSwitch("--notifications")]
    public string[]? Notifications { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--related-ops-items")]
    public string[]? RelatedOpsItems { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--severity")]
    public string? Severity { get; set; }

    [CommandSwitch("--actual-start-time")]
    public long? ActualStartTime { get; set; }

    [CommandSwitch("--actual-end-time")]
    public long? ActualEndTime { get; set; }

    [CommandSwitch("--planned-start-time")]
    public long? PlannedStartTime { get; set; }

    [CommandSwitch("--planned-end-time")]
    public long? PlannedEndTime { get; set; }

    [CommandSwitch("--ops-item-arn")]
    public string? OpsItemArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}