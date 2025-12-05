using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "update-byte-match-set")]
public record AwsWafUpdateByteMatchSetOptions(
[property: CliOption("--byte-match-set-id")] string ByteMatchSetId,
[property: CliOption("--change-token")] string ChangeToken,
[property: CliOption("--updates")] string[] Updates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}