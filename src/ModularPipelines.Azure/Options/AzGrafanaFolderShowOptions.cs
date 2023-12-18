using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "folder", "show")]
public record AzGrafanaFolderShowOptions(
[property: CommandSwitch("--folder")] string Folder,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--api-key")]
    public string? ApiKey { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}