using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "folder", "update")]
public record AzGrafanaFolderUpdateOptions(
[property: CliOption("--folder")] string Folder,
[property: CliOption("--name")] string Name,
[property: CliOption("--title")] string Title
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}