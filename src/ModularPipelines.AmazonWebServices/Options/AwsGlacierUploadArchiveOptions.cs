using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glacier", "upload-archive")]
public record AwsGlacierUploadArchiveOptions(
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--account-id")] string AccountId
) : AwsOptions
{
    [CliOption("--archive-description")]
    public string? ArchiveDescription { get; set; }

    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}