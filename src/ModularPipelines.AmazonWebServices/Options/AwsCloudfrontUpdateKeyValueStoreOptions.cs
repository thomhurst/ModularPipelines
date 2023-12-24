using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "update-key-value-store")]
public record AwsCloudfrontUpdateKeyValueStoreOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--comment")] string Comment,
[property: CommandSwitch("--if-match")] string IfMatch
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}