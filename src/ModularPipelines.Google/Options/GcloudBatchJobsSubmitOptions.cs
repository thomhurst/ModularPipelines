using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "jobs", "submit")]
public record GcloudBatchJobsSubmitOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location,
[property: CliOption("--config")] string Config,
[property: CliOption("--container-commands-file")] string ContainerCommandsFile,
[property: CliOption("--container-entrypoint")] string ContainerEntrypoint,
[property: CliOption("--container-image-uri")] string ContainerImageUri,
[property: CliOption("--script-file-path")] string ScriptFilePath,
[property: CliOption("--script-text")] string ScriptText
) : GcloudOptions
{
    [CliOption("--job-prefix")]
    public string? JobPrefix { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CliFlag("--no-external-ip-address")]
    public bool? NoExternalIpAddress { get; set; }
}