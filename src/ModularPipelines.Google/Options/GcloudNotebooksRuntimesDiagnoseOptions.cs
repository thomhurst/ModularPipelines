using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "runtimes", "diagnose")]
public record GcloudNotebooksRuntimesDiagnoseOptions(
[property: CliArgument] string Runtime,
[property: CliArgument] string Location,
[property: CliOption("--gcs-bucket")] string GcsBucket
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--enable-copy-home-files")]
    public bool? EnableCopyHomeFiles { get; set; }

    [CliFlag("--enable-packet-capture")]
    public bool? EnablePacketCapture { get; set; }

    [CliFlag("--enable-repair")]
    public bool? EnableRepair { get; set; }

    [CliOption("--relative-path")]
    public string? RelativePath { get; set; }

    [CliOption("--timeout-minutes")]
    public string? TimeoutMinutes { get; set; }
}