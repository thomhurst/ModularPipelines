using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "sign-url")]
public record GcloudStorageSignUrlOptions(
[property: PositionalArgument] string Url
) : GcloudOptions
{
    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--headers")]
    public IEnumerable<KeyValue>? Headers { get; set; }

    [CommandSwitch("--http-verb")]
    public string? HttpVerb { get; set; }

    [CommandSwitch("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CommandSwitch("--private-key-password")]
    public string? PrivateKeyPassword { get; set; }

    [CommandSwitch("--query-params")]
    public IEnumerable<KeyValue>? QueryParams { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}