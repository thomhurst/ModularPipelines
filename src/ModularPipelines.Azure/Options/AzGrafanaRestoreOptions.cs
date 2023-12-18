using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "restore")]
public record AzGrafanaRestoreOptions(
[property: CommandSwitch("--archive-file")] string ArchiveFile,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--components")]
    public string? Components { get; set; }

    [BooleanCommandSwitch("--remap-data-sources")]
    public bool? RemapDataSources { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}