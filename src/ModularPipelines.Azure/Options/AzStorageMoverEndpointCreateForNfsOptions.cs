using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage-mover", "endpoint", "create-for-nfs")]
public record AzStorageMoverEndpointCreateForNfsOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--export")] string Export,
[property: CliOption("--host")] string Host,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-mover-name")] string StorageMoverName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--nfs-version")]
    public string? NfsVersion { get; set; }
}