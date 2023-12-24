using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "update-restore-testing-plan")]
public record AwsBackupUpdateRestoreTestingPlanOptions(
[property: CommandSwitch("--restore-testing-plan")] string RestoreTestingPlan,
[property: CommandSwitch("--restore-testing-plan-name")] string RestoreTestingPlanName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}