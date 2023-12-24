using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qldb", "get-block")]
public record AwsQldbGetBlockOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--block-address")] string BlockAddress
) : AwsOptions
{
    [CommandSwitch("--digest-tip-address")]
    public string? DigestTipAddress { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}