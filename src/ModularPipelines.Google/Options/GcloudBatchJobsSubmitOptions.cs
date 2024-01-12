using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "jobs", "submit")]
public record GcloudBatchJobsSubmitOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--config")] string Config,
[property: CommandSwitch("--container-commands-file")] string ContainerCommandsFile,
[property: CommandSwitch("--container-entrypoint")] string ContainerEntrypoint,
[property: CommandSwitch("--container-image-uri")] string ContainerImageUri,
[property: CommandSwitch("--script-file-path")] string ScriptFilePath,
[property: CommandSwitch("--script-text")] string ScriptText
) : GcloudOptions
{
    [CommandSwitch("--job-prefix")]
    public string? JobPrefix { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }

    [BooleanCommandSwitch("--no-external-ip-address")]
    public bool? NoExternalIpAddress { get; set; }
}