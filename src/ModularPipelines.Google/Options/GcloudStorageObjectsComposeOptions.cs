using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "objects", "compose")]
public record GcloudStorageObjectsComposeOptions(
[property: CliArgument] string Source,
[property: CliArgument] string Destination
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }
}