using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "sign")]
public record AwsKmsSignOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--message")] string Message,
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