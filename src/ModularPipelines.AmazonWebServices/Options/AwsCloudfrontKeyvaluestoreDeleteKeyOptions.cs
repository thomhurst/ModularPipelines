using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront-keyvaluestore", "delete-key")]
public record AwsCloudfrontKeyvaluestoreDeleteKeyOptions(
[property: CliOption("--kvs-arn")] string KvsArn,
[property: CliOption("--key")] string Key,
[property: CliOption("--if-match")] string IfMatch
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}