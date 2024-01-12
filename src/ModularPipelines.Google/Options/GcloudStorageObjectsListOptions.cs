using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "objects", "list")]
public record GcloudStorageObjectsListOptions(
[property: PositionalArgument] string Urls
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--exhaustive")]
    public bool? Exhaustive { get; set; }

    [BooleanCommandSwitch("--fetch-encrypted-object-hashes")]
    public bool? FetchEncryptedObjectHashes { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [BooleanCommandSwitch("--raw")]
    public bool? Raw { get; set; }

    [BooleanCommandSwitch("--soft-deleted")]
    public bool? SoftDeleted { get; set; }

    [BooleanCommandSwitch("--stat")]
    public bool? Stat { get; set; }
}