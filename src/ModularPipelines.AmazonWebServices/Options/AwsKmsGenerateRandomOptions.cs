using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "generate-random")]
public record AwsKmsGenerateRandomOptions : AwsOptions
{
    [CliOption("--number-of-bytes")]
    public int? NumberOfBytes { get; set; }

    [CliOption("--custom-key-store-id")]
    public string? CustomKeyStoreId { get; set; }

    [CliOption("--recipient")]
    public string? Recipient { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}