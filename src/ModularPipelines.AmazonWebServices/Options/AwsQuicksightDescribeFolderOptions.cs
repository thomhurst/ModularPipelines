using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-folder")]
public record AwsQuicksightDescribeFolderOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--folder-id")] string FolderId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}