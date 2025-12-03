using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "disconnect-custom-key-store")]
public record AwsKmsDisconnectCustomKeyStoreOptions(
[property: CliOption("--custom-key-store-id")] string CustomKeyStoreId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}