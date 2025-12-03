using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "cloud-bindings", "update")]
public record GcloudAccessContextManagerCloudBindingsUpdateOptions(
[property: CliOption("--binding")] string Binding,
[property: CliOption("--organization")] string Organization
) : GcloudOptions
{
    [CliOption("--dry-run-level")]
    public string[]? DryRunLevel { get; set; }

    [CliOption("--level")]
    public string[]? Level { get; set; }
}