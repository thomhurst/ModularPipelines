using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qldb", "get-revision")]
public record AwsQldbGetRevisionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--block-address")] string BlockAddress,
[property: CliOption("--document-id")] string DocumentId
) : AwsOptions
{
    [CliOption("--digest-tip-address")]
    public string? DigestTipAddress { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}