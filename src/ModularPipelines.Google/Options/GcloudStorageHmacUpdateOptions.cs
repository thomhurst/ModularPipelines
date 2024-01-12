using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "hmac", "update")]
public record GcloudStorageHmacUpdateOptions(
[property: PositionalArgument] string AccessId,
[property: BooleanCommandSwitch("--activate")] bool Activate,
[property: BooleanCommandSwitch("--deactivate")] bool Deactivate
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}