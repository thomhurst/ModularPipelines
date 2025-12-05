using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "create-restore-testing-selection")]
public record AwsBackupCreateRestoreTestingSelectionOptions(
[property: CliOption("--restore-testing-plan-name")] string RestoreTestingPlanName,
[property: CliOption("--restore-testing-selection")] string RestoreTestingSelection
) : AwsOptions
{
    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}