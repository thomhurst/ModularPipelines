using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "verify-mac")]
public record AwsKmsVerifyMacOptions(
[property: CliOption("--message")] string Message,
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--mac-algorithm")] string MacAlgorithm,
[property: CliOption("--mac")] string Mac
) : AwsOptions
{
    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}