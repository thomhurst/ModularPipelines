using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage-mover", "endpoint", "create-for-smb")]
public record AzStorageMoverEndpointCreateForSmbOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--host")] string Host,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-name")] string ShareName,
[property: CommandSwitch("--storage-mover-name")] string StorageMoverName
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--password-uri")]
    public string? PasswordUri { get; set; }

    [CommandSwitch("--username-uri")]
    public string? UsernameUri { get; set; }
}