using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "cancel-legal-hold")]
public record AwsBackupCancelLegalHoldOptions(
[property: CommandSwitch("--legal-hold-id")] string LegalHoldId,
[property: CommandSwitch("--cancel-description")] string CancelDescription
) : AwsOptions
{
    [CommandSwitch("--retain-record-in-days")]
    public long? RetainRecordInDays { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}