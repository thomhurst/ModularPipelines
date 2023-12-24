using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-data-source-permissions")]
public record AwsQuicksightUpdateDataSourcePermissionsOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--data-source-id")] string DataSourceId
) : AwsOptions
{
    [CommandSwitch("--grant-permissions")]
    public string[]? GrantPermissions { get; set; }

    [CommandSwitch("--revoke-permissions")]
    public string[]? RevokePermissions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}