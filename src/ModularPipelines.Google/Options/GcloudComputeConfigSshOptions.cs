using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "config-ssh")]
public record GcloudComputeConfigSshOptions : GcloudOptions
{
    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliFlag("--remove")]
    public bool? Remove { get; set; }

    [CliOption("--ssh-config-file")]
    public string? SshConfigFile { get; set; }

    [CliOption("--ssh-key-file")]
    public string? SshKeyFile { get; set; }
}