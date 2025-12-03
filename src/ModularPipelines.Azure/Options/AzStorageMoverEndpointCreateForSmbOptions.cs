using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage-mover", "endpoint", "create-for-smb")]
public record AzStorageMoverEndpointCreateForSmbOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--host")] string Host,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--share-name")] string ShareName,
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