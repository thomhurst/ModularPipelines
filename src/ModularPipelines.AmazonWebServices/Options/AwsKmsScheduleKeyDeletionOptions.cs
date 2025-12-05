using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "schedule-key-deletion")]
public record AwsKmsScheduleKeyDeletionOptions(
[property: CliOption("--key-id")] string KeyId
) : AwsOptions
{
    [CliOption("--pending-window-in-days")]
    public int? PendingWindowInDays { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}