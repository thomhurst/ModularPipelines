using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront-keyvaluestore", "get-key")]
public record AwsCloudfrontKeyvaluestoreGetKeyOptions(
[property: CliOption("--kvs-arn")] string KvsArn,
[property: CliOption("--key")] string Key
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}