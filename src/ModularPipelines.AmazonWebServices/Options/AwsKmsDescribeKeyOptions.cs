using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "describe-key")]
public record AwsKmsDescribeKeyOptions(
[property: CliOption("--key-id")] string KeyId
) : AwsOptions
{
    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}