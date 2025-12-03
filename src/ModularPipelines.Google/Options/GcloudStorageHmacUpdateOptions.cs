using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "hmac", "update")]
public record GcloudStorageHmacUpdateOptions(
[property: CliArgument] string AccessId,
[property: CliFlag("--activate")] bool Activate,
[property: CliFlag("--deactivate")] bool Deactivate
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}