using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "hash")]
public record GcloudStorageHashOptions(
[property: CliArgument] string Urls
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--hex")]
    public bool? Hex { get; set; }

    [CliFlag("--skip-crc32c")]
    public bool? SkipCrc32c { get; set; }

    [CliFlag("--skip-md5")]
    public bool? SkipMd5 { get; set; }
}