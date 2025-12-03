using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "runtimes", "create")]
public record GcloudNotebooksRuntimesCreateOptions(
[property: CliArgument] string Runtime,
[property: CliArgument] string Location,
[property: CliOption("--runtime-access-type")] string RuntimeAccessType,
[property: CliOption("--runtime-owner")] string RuntimeOwner,
[property: CliOption("--runtime-type")] string RuntimeType,
[property: CliOption("--machine-type")] string MachineType,
[property: CliOption("--interface")] string Interface,
[property: CliOption("--mode")] string Mode,
[property: CliOption("--source")] string Source,
[property: CliOption("--type")] string Type
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--custom-gpu-driver-path")]
    public string? CustomGpuDriverPath { get; set; }

    [CliOption("--idle-shutdown-timeout")]
    public string? IdleShutdownTimeout { get; set; }

    [CliOption("--install-gpu-driver")]
    public string? InstallGpuDriver { get; set; }

    [CliOption("--post-startup-script")]
    public string? PostStartupScript { get; set; }

    [CliOption("--post-startup-script-behavior")]
    public string? PostStartupScriptBehavior { get; set; }
}