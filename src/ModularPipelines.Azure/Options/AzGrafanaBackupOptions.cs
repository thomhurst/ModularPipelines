using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "backup")]
public record AzGrafanaBackupOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--components")]
    public string? Components { get; set; }

    [CliOption("--directory")]
    public string? Directory { get; set; }

    [CliOption("--folders-to-exclude")]
    public string? FoldersToExclude { get; set; }

    [CliOption("--folders-to-include")]
    public string? FoldersToInclude { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}