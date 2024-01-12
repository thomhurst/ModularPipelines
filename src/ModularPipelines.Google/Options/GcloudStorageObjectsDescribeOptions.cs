using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "objects", "describe")]
public record GcloudStorageObjectsDescribeOptions(
[property: PositionalArgument] string Url
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--fetch-encrypted-object-hashes")]
    public bool? FetchEncryptedObjectHashes { get; set; }

    [BooleanCommandSwitch("--raw")]
    public bool? Raw { get; set; }

    [BooleanCommandSwitch("--soft-deleted")]
    public bool? SoftDeleted { get; set; }
}