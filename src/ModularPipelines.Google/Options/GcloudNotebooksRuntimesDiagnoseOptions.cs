using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "runtimes", "diagnose")]
public record GcloudNotebooksRuntimesDiagnoseOptions(
[property: PositionalArgument] string Runtime,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gcs-bucket")] string GcsBucket
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--enable-copy-home-files")]
    public bool? EnableCopyHomeFiles { get; set; }

    [BooleanCommandSwitch("--enable-packet-capture")]
    public bool? EnablePacketCapture { get; set; }

    [BooleanCommandSwitch("--enable-repair")]
    public bool? EnableRepair { get; set; }

    [CommandSwitch("--relative-path")]
    public string? RelativePath { get; set; }

    [CommandSwitch("--timeout-minutes")]
    public string? TimeoutMinutes { get; set; }
}