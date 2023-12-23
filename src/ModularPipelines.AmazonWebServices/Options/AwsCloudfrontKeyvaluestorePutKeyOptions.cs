using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront-keyvaluestore", "put-key")]
public record AwsCloudfrontKeyvaluestorePutKeyOptions(
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--value")] string Value,
[property: CommandSwitch("--kvs-arn")] string KvsArn,
[property: CommandSwitch("--if-match")] string IfMatch
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}