using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "cancel-legal-hold")]
public record AwsBackupCancelLegalHoldOptions(
[property: CliOption("--legal-hold-id")] string LegalHoldId,
[property: CliOption("--cancel-description")] string CancelDescription
) : AwsOptions
{
    [CliOption("--retain-record-in-days")]
    public long? RetainRecordInDays { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}