using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "objects", "describe")]
public record GcloudStorageObjectsDescribeOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--fetch-encrypted-object-hashes")]
    public bool? FetchEncryptedObjectHashes { get; set; }

    [CliFlag("--raw")]
    public bool? Raw { get; set; }

    [CliFlag("--soft-deleted")]
    public bool? SoftDeleted { get; set; }
}