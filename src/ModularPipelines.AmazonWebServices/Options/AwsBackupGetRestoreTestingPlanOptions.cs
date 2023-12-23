using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "get-restore-testing-plan")]
public record AwsBackupGetRestoreTestingPlanOptions(
[property: CommandSwitch("--restore-testing-plan-name")] string RestoreTestingPlanName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}