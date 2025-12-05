using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emulators", "firestore", "start")]
public record GcloudEmulatorsFirestoreStartOptions : GcloudOptions
{
    [CliOption("--host-port")]
    public string? HostPort { get; set; }

    [CliOption("--rules")]
    public string? Rules { get; set; }
}