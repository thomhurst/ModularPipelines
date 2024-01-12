using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "objects", "compose")]
public record GcloudStorageObjectsComposeOptions(
[property: PositionalArgument] string Source,
[property: PositionalArgument] string Destination
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }
}