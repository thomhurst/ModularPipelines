using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront-keyvaluestore", "describe-key-value-store")]
public record AwsCloudfrontKeyvaluestoreDescribeKeyValueStoreOptions(
[property: CliOption("--kvs-arn")] string KvsArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}