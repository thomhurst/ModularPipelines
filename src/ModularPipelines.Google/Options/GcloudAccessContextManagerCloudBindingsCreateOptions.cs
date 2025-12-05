using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "cloud-bindings", "create")]
public record GcloudAccessContextManagerCloudBindingsCreateOptions(
[property: CliOption("--group-key")] string GroupKey
) : GcloudOptions
{
    [CliOption("--dry-run-level")]
    public string[]? DryRunLevel { get; set; }

    [CliOption("--level")]
    public string[]? Level { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }
}