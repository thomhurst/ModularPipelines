using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-analysis-permissions")]
public record AwsQuicksightUpdateAnalysisPermissionsOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--analysis-id")] string AnalysisId
) : AwsOptions
{
    [CommandSwitch("--grant-permissions")]
    public string[]? GrantPermissions { get; set; }

    [CommandSwitch("--revoke-permissions")]
    public string[]? RevokePermissions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}