using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("components", "list")]
public record GcloudComponentsListOptions : GcloudOptions
{
    [CliFlag("--only-local-state")]
    public bool? OnlyLocalState { get; set; }

    [CliFlag("--show-platform")]
    public bool? ShowPlatform { get; set; }

    [CliFlag("--show-versions")]
    public bool? ShowVersions { get; set; }
}