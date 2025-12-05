using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "reset-encryption-key")]
public record AwsInspector2ResetEncryptionKeyOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--scan-type")] string ScanType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}