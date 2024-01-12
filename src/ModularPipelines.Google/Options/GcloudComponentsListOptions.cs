using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("components", "list")]
public record GcloudComponentsListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--only-local-state")]
    public bool? OnlyLocalState { get; set; }

    [BooleanCommandSwitch("--show-platform")]
    public bool? ShowPlatform { get; set; }

    [BooleanCommandSwitch("--show-versions")]
    public bool? ShowVersions { get; set; }
}