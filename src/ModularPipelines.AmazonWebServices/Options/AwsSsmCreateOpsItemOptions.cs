using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "create-ops-item")]
public record AwsSsmCreateOpsItemOptions(
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--title")] string Title
) : AwsOptions
{
    [CommandSwitch("--ops-item-type")]
    public string? OpsItemType { get; set; }

    [CommandSwitch("--operational-data")]
    public IEnumerable<KeyValue>? OperationalData { get; set; }

    [CommandSwitch("--notifications")]
    public string[]? Notifications { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--related-ops-items")]
    public string[]? RelatedOpsItems { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

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

    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}