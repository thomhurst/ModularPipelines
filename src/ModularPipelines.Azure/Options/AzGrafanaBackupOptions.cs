using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "backup")]
public record AzGrafanaBackupOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--components")]
    public string? Components { get; set; }

    [CommandSwitch("--directory")]
    public string? Directory { get; set; }

    [CommandSwitch("--folders-to-exclude")]
    public string? FoldersToExclude { get; set; }

    [CommandSwitch("--folders-to-include")]
    public string? FoldersToInclude { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}