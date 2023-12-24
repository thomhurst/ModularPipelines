using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront-keyvaluestore", "delete-key")]
public record AwsCloudfrontKeyvaluestoreDeleteKeyOptions(
[property: CommandSwitch("--kvs-arn")] string KvsArn,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--if-match")] string IfMatch
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}