using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage-mover", "endpoint", "update-for-smb")]
public record AzStorageMoverEndpointUpdateForSmbOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-mover-name")] string StorageMoverName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--password-uri")]
    public string? PasswordUri { get; set; }

    [CliOption("--username-uri")]
    public string? UsernameUri { get; set; }
}