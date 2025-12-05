using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "update-encryption-key")]
public record AwsInspector2UpdateEncryptionKeyOptions(
[property: CliOption("--kms-key-id")] string KmsKeyId,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--scan-type")] string ScanType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}