using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glacier", "upload-multipart-part")]
public record AwsGlacierUploadMultipartPartOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--upload-id")] string UploadId
) : AwsOptions
{
    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--range")]
    public string? Range { get; set; }

    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}