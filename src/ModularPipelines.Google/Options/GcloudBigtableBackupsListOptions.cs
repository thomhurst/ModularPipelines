using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "backups", "list")]
public record GcloudBigtableBackupsListOptions : GcloudOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--instance")]
    public string? Instance { get; set; }
}