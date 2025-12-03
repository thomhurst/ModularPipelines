using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "list-preview-rotation-shifts")]
public record AwsSsmContactsListPreviewRotationShiftsOptions(
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--members")] string[] Members,
[property: CliOption("--time-zone-id")] string TimeZoneId,
[property: CliOption("--recurrence")] string Recurrence
) : AwsOptions
{
    [CliOption("--rotation-start-time")]
    public long? RotationStartTime { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--overrides")]
    public string[]? Overrides { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}