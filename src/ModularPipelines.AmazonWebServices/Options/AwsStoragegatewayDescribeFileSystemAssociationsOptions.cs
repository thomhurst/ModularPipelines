using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "describe-file-system-associations")]
public record AwsStoragegatewayDescribeFileSystemAssociationsOptions(
[property: CliOption("--file-system-association-arn-list")] string[] FileSystemAssociationArnList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}