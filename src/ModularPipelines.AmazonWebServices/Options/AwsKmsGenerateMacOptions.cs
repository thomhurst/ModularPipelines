using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "generate-mac")]
public record AwsKmsGenerateMacOptions(
[property: CliOption("--message")] string Message,
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--mac-algorithm")] string MacAlgorithm
) : AwsOptions
{
    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}