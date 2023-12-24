using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-contacts", "list-preview-rotation-shifts")]
public record AwsSsmContactsListPreviewRotationShiftsOptions(
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--members")] string[] Members,
[property: CommandSwitch("--time-zone-id")] string TimeZoneId,
[property: CommandSwitch("--recurrence")] string Recurrence
) : AwsOptions
{
    [CommandSwitch("--rotation-start-time")]
    public long? RotationStartTime { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--overrides")]
    public string[]? Overrides { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}