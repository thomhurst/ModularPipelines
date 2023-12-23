using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-contacts", "create-rotation")]
public record AwsSsmContactsCreateRotationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--contact-ids")] string[] ContactIds,
[property: CommandSwitch("--time-zone-id")] string TimeZoneId,
[property: CommandSwitch("--recurrence")] string Recurrence
) : AwsOptions
{
    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}