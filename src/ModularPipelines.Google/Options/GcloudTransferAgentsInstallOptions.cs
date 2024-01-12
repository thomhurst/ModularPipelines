using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "agents", "install")]
public record GcloudTransferAgentsInstallOptions(
[property: CommandSwitch("--pool")] string Pool
) : GcloudOptions
{
    [CommandSwitch("--count")]
    public string? Count { get; set; }

    [CommandSwitch("--creds-file")]
    public string? CredsFile { get; set; }

    [CommandSwitch("--docker-network")]
    public string? DockerNetwork { get; set; }

    [CommandSwitch("--[no-]enable-multipart")]
    public string[]? NoEnableMultipart { get; set; }

    [CommandSwitch("--id-prefix")]
    public string? IdPrefix { get; set; }

    [CommandSwitch("--logs-directory")]
    public string? LogsDirectory { get; set; }

    [CommandSwitch("--memlock-limit")]
    public string? MemlockLimit { get; set; }

    [CommandSwitch("--mount-directories")]
    public string[]? MountDirectories { get; set; }

    [CommandSwitch("--proxy")]
    public string? Proxy { get; set; }

    [BooleanCommandSwitch("--s3-compatible-mode")]
    public bool? S3CompatibleMode { get; set; }
}