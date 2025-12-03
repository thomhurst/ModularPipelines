using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "delete-restore-testing-selection")]
public record AwsBackupDeleteRestoreTestingSelectionOptions(
[property: CliOption("--restore-testing-plan-name")] string RestoreTestingPlanName,
[property: CliOption("--restore-testing-selection-name")] string RestoreTestingSelectionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}