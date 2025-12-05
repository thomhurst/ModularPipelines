using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "delete-restore-testing-plan")]
public record AwsBackupDeleteRestoreTestingPlanOptions(
[property: CliOption("--restore-testing-plan-name")] string RestoreTestingPlanName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}