using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emulators", "firestore", "start")]
public record GcloudEmulatorsFirestoreStartOptions : GcloudOptions
{
    [CommandSwitch("--host-port")]
    public string? HostPort { get; set; }

    [CommandSwitch("--rules")]
    public string? Rules { get; set; }
}