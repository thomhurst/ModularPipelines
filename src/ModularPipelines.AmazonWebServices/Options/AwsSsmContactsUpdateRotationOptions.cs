using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-contacts", "update-rotation")]
public record AwsSsmContactsUpdateRotationOptions(
[property: CommandSwitch("--rotation-id")] string RotationId,
[property: CommandSwitch("--recurrence")] string Recurrence
) : AwsOptions
{
    [CommandSwitch("--contact-ids")]
    public string[]? ContactIds { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--time-zone-id")]
    public string? TimeZoneId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}