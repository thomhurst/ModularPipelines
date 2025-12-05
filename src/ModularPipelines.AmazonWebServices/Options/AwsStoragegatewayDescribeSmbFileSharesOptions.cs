using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "describe-smb-file-shares")]
public record AwsStoragegatewayDescribeSmbFileSharesOptions(
[property: CliOption("--file-share-arn-list")] string[] FileShareArnList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}