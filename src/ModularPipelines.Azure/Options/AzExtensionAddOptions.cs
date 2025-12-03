using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("extension", "add")]
public record AzExtensionAddOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--pip-extra-index-urls")]
    public string? PipExtraIndexUrls { get; set; }

    [CliOption("--pip-proxy")]
    public string? PipProxy { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--system")]
    public string? System { get; set; }

    [CliOption("--upgrade")]
    public string? Upgrade { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--yes")]
    public bool? Yes { get; set; } = true;
}