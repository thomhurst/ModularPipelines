using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "refresh-cache")]
public record AwsStoragegatewayRefreshCacheOptions(
[property: CliOption("--file-share-arn")] string FileShareArn
) : AwsOptions
{
    [CliOption("--folder-list")]
    public string[]? FolderList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}