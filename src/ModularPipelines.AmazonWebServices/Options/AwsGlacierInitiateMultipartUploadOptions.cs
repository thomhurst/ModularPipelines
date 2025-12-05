using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glacier", "initiate-multipart-upload")]
public record AwsGlacierInitiateMultipartUploadOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--vault-name")] string VaultName
) : AwsOptions
{
    [CliOption("--archive-description")]
    public string? ArchiveDescription { get; set; }

    [CliOption("--part-size")]
    public string? PartSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}