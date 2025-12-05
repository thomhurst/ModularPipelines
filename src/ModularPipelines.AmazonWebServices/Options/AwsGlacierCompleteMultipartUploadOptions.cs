using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glacier", "complete-multipart-upload")]
public record AwsGlacierCompleteMultipartUploadOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--upload-id")] string UploadId
) : AwsOptions
{
    [CliOption("--archive-size")]
    public string? ArchiveSize { get; set; }

    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}