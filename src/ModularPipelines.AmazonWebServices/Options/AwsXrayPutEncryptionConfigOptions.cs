using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "put-encryption-config")]
public record AwsXrayPutEncryptionConfigOptions(
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}