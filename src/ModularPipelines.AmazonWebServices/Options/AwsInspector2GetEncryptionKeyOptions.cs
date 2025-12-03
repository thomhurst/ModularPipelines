using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "get-encryption-key")]
public record AwsInspector2GetEncryptionKeyOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--scan-type")] string ScanType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}