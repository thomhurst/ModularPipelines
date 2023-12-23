using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront-keyvaluestore", "describe-key-value-store")]
public record AwsCloudfrontKeyvaluestoreDescribeKeyValueStoreOptions(
[property: CommandSwitch("--kvs-arn")] string KvsArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}