using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-folder-permissions")]
public record AwsQuicksightDescribeFolderPermissionsOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--folder-id")] string FolderId
) : AwsOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}