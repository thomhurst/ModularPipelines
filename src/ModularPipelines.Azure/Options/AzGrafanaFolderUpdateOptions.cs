using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "folder", "update")]
public record AzGrafanaFolderUpdateOptions(
[property: CommandSwitch("--folder")] string Folder,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--title")] string Title
) : AzOptions
{
    [CommandSwitch("--api-key")]
    public string? ApiKey { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}