using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "agents", "install")]
public record GcloudTransferAgentsInstallOptions(
[property: CliOption("--pool")] string Pool
) : GcloudOptions
{
    [CliOption("--count")]
    public string? Count { get; set; }

    [CliOption("--creds-file")]
    public string? CredsFile { get; set; }

    [CliOption("--docker-network")]
    public string? DockerNetwork { get; set; }

    [CliOption("--[no-]enable-multipart")]
    public string[]? NoEnableMultipart { get; set; }

    [CliOption("--id-prefix")]
    public string? IdPrefix { get; set; }

    [CliOption("--logs-directory")]
    public string? LogsDirectory { get; set; }

    [CliOption("--memlock-limit")]
    public string? MemlockLimit { get; set; }

    [CliOption("--mount-directories")]
    public string[]? MountDirectories { get; set; }

    [CliOption("--proxy")]
    public string? Proxy { get; set; }

    [CliFlag("--s3-compatible-mode")]
    public bool? S3CompatibleMode { get; set; }
}