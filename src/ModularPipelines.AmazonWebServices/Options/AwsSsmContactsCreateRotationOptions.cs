using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "create-rotation")]
public record AwsSsmContactsCreateRotationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--contact-ids")] string[] ContactIds,
[property: CliOption("--time-zone-id")] string TimeZoneId,
[property: CliOption("--recurrence")] string Recurrence
) : AwsOptions
{
    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}