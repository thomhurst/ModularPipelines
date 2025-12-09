using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront-keyvaluestore", "update-keys")]
public record AwsCloudfrontKeyvaluestoreUpdateKeysOptions(
[property: CliOption("--kvs-arn")] string KvsArn,
[property: CliOption("--if-match")] string IfMatch
) : AwsOptions
{
    [CliOption("--puts")]
    public string[]? Puts { get; set; }

    [CliOption("--deletes")]
    public string[]? Deletes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}