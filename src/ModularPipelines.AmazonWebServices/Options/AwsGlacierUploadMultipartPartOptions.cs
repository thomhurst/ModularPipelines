using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glacier", "upload-multipart-part")]
public record AwsGlacierUploadMultipartPartOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--upload-id")] string UploadId
) : AwsOptions
{
    [CommandSwitch("--checksum")]
    public string? Checksum { get; set; }

    [CommandSwitch("--range")]
    public string? Range { get; set; }

    [CommandSwitch("--body")]
    public string? Body { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}