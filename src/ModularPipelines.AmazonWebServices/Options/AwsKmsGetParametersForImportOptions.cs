using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "get-parameters-for-import")]
public record AwsKmsGetParametersForImportOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--wrapping-algorithm")] string WrappingAlgorithm,
[property: CliOption("--wrapping-key-spec")] string WrappingKeySpec
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}