using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qldb", "get-revision")]
public record AwsQldbGetRevisionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--block-address")] string BlockAddress,
[property: CommandSwitch("--document-id")] string DocumentId
) : AwsOptions
{
    [CommandSwitch("--digest-tip-address")]
    public string? DigestTipAddress { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}