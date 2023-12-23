using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "delete-restore-testing-plan")]
public record AwsBackupDeleteRestoreTestingPlanOptions(
[property: CommandSwitch("--restore-testing-plan-name")] string RestoreTestingPlanName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}