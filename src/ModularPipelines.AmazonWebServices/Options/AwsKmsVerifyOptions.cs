using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "verify")]
public record AwsKmsVerifyOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--message")] string Message,
[property: CliOption("--signature")] string Signature,
[property: CliOption("--signing-algorithm")] string SigningAlgorithm
) : AwsOptions
{
    [CliOption("--message-type")]
    public string? MessageType { get; set; }

    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}