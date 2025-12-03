using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "copy-files")]
public record GcloudComputeCopyFilesOptions(
[property: CliArgument] string User
) : GcloudOptions
{
    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliFlag("--plain")]
    public bool? Plain { get; set; }

    [CliOption("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CliOption("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CliOption("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}