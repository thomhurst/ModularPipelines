using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-data-source-permissions")]
public record AwsQuicksightUpdateDataSourcePermissionsOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--data-source-id")] string DataSourceId
) : AwsOptions
{
    [CliOption("--grant-permissions")]
    public string[]? GrantPermissions { get; set; }

    [CliOption("--revoke-permissions")]
    public string[]? RevokePermissions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}