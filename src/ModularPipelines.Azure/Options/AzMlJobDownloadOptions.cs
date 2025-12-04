using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "job", "download")]
public record AzMlJobDownloadOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--download-path")]
    public string? DownloadPath { get; set; }

    [CliOption("--output-name")]
    public string? OutputName { get; set; }
}