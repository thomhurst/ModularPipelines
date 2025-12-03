using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "update-restore-testing-selection")]
public record AwsBackupUpdateRestoreTestingSelectionOptions(
[property: CliOption("--restore-testing-plan-name")] string RestoreTestingPlanName,
[property: CliOption("--restore-testing-selection")] string RestoreTestingSelection,
[property: CliOption("--restore-testing-selection-name")] string RestoreTestingSelectionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}