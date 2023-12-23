using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "update-restore-testing-selection")]
public record AwsBackupUpdateRestoreTestingSelectionOptions(
[property: CommandSwitch("--restore-testing-plan-name")] string RestoreTestingPlanName,
[property: CommandSwitch("--restore-testing-selection")] string RestoreTestingSelection,
[property: CommandSwitch("--restore-testing-selection-name")] string RestoreTestingSelectionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}