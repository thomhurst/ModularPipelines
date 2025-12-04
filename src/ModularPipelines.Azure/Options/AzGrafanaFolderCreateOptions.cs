using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "folder", "create")]
public record AzGrafanaFolderCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--title")] string Title
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}