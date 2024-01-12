using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "cat")]
public record GcloudStorageCatOptions(
[property: PositionalArgument] string Url
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--display-url")]
    public bool? DisplayUrl { get; set; }

    [CommandSwitch("--range")]
    public string? Range { get; set; }
}