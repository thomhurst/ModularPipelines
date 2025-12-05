using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront-keyvaluestore", "put-key")]
public record AwsCloudfrontKeyvaluestorePutKeyOptions(
[property: CliOption("--key")] string Key,
[property: CliOption("--value")] string Value,
[property: CliOption("--kvs-arn")] string KvsArn,
[property: CliOption("--if-match")] string IfMatch
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}