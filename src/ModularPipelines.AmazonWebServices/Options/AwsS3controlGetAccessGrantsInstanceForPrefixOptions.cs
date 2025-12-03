using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "get-access-grants-instance-for-prefix")]
public record AwsS3controlGetAccessGrantsInstanceForPrefixOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--s3-prefix")] string S3Prefix
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}