using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "update-restore-testing-plan")]
public record AwsBackupUpdateRestoreTestingPlanOptions(
[property: CliOption("--restore-testing-plan")] string RestoreTestingPlan,
[property: CliOption("--restore-testing-plan-name")] string RestoreTestingPlanName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}