using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront-keyvaluestore", "get-key")]
public record AwsCloudfrontKeyvaluestoreGetKeyOptions(
[property: CommandSwitch("--kvs-arn")] string KvsArn,
[property: CommandSwitch("--key")] string Key
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}