using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "connect-to-serial-port")]
public record GcloudComputeConnectToSerialPortOptions(
[property: CliArgument] string User
) : GcloudOptions
{
    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliOption("--extra-args")]
    public IEnumerable<KeyValue>? ExtraArgs { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CliOption("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}