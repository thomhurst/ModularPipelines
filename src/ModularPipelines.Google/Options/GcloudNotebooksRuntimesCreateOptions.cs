using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "runtimes", "create")]
public record GcloudNotebooksRuntimesCreateOptions(
[property: PositionalArgument] string Runtime,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--runtime-access-type")] string RuntimeAccessType,
[property: CommandSwitch("--runtime-owner")] string RuntimeOwner,
[property: CommandSwitch("--runtime-type")] string RuntimeType,
[property: CommandSwitch("--machine-type")] string MachineType,
[property: CommandSwitch("--interface")] string Interface,
[property: CommandSwitch("--mode")] string Mode,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--type")] string Type
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--custom-gpu-driver-path")]
    public string? CustomGpuDriverPath { get; set; }

    [CommandSwitch("--idle-shutdown-timeout")]
    public string? IdleShutdownTimeout { get; set; }

    [CommandSwitch("--install-gpu-driver")]
    public string? InstallGpuDriver { get; set; }

    [CommandSwitch("--post-startup-script")]
    public string? PostStartupScript { get; set; }

    [CommandSwitch("--post-startup-script-behavior")]
    public string? PostStartupScriptBehavior { get; set; }
}