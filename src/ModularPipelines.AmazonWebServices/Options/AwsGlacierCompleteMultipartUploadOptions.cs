using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glacier", "complete-multipart-upload")]
public record AwsGlacierCompleteMultipartUploadOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--upload-id")] string UploadId
) : AwsOptions
{
    [CommandSwitch("--archive-size")]
    public string? ArchiveSize { get; set; }

    [CommandSwitch("--checksum")]
    public string? Checksum { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}