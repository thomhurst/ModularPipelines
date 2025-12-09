using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sts", "decode-authorization-message")]
public record AwsStsDecodeAuthorizationMessageOptions(
[property: CliOption("--encoded-message")] string EncodedMessage
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}