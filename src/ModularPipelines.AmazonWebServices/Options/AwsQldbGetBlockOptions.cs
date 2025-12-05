using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qldb", "get-block")]
public record AwsQldbGetBlockOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--block-address")] string BlockAddress
) : AwsOptions
{
    [CliOption("--digest-tip-address")]
    public string? DigestTipAddress { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}