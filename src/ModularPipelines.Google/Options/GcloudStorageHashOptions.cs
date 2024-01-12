using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "hash")]
public record GcloudStorageHashOptions(
[property: PositionalArgument] string Urls
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--hex")]
    public bool? Hex { get; set; }

    [BooleanCommandSwitch("--skip-crc32c")]
    public bool? SkipCrc32c { get; set; }

    [BooleanCommandSwitch("--skip-md5")]
    public bool? SkipMd5 { get; set; }
}