using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "update-rotation")]
public record AwsSsmContactsUpdateRotationOptions(
[property: CliOption("--rotation-id")] string RotationId,
[property: CliOption("--recurrence")] string Recurrence
) : AwsOptions
{
    [CliOption("--contact-ids")]
    public string[]? ContactIds { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--time-zone-id")]
    public string? TimeZoneId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}