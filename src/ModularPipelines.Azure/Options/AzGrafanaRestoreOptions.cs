using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "restore")]
public record AzGrafanaRestoreOptions(
[property: CliOption("--archive-file")] string ArchiveFile,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--components")]
    public string? Components { get; set; }

    [CliFlag("--remap-data-sources")]
    public bool? RemapDataSources { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}