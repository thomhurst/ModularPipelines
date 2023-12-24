using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glacier", "upload-archive")]
public record AwsGlacierUploadArchiveOptions(
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--account-id")] string AccountId
) : AwsOptions
{
    [CommandSwitch("--archive-description")]
    public string? ArchiveDescription { get; set; }

    [CommandSwitch("--checksum")]
    public string? Checksum { get; set; }

    [CommandSwitch("--body")]
    public string? Body { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}