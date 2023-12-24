using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "create-restore-testing-selection")]
public record AwsBackupCreateRestoreTestingSelectionOptions(
[property: CommandSwitch("--restore-testing-plan-name")] string RestoreTestingPlanName,
[property: CommandSwitch("--restore-testing-selection")] string RestoreTestingSelection
) : AwsOptions
{
    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}