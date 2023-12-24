using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront-keyvaluestore", "update-keys")]
public record AwsCloudfrontKeyvaluestoreUpdateKeysOptions(
[property: CommandSwitch("--kvs-arn")] string KvsArn,
[property: CommandSwitch("--if-match")] string IfMatch
) : AwsOptions
{
    [CommandSwitch("--puts")]
    public string[]? Puts { get; set; }

    [CommandSwitch("--deletes")]
    public string[]? Deletes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}