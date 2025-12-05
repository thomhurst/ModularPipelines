using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "get-restore-testing-plan")]
public record AwsBackupGetRestoreTestingPlanOptions(
[property: CliOption("--restore-testing-plan-name")] string RestoreTestingPlanName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}