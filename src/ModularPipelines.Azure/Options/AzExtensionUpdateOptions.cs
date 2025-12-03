using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("extension", "update")]
public record AzExtensionUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--pip-extra-index-urls")]
    public string? PipExtraIndexUrls { get; set; }

    [CliOption("--pip-proxy")]
    public string? PipProxy { get; set; }
}