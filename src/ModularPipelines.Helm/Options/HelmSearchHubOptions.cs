using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("search", "hub")]
[ExcludeFromCodeCoverage]
public record HelmSearchHubOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--endpoint", SwitchValueSeparator = " ")]
    public string? Endpoint { get; set; }

    [BooleanCommandSwitch("--list-repo-url")]
    public bool? ListRepoUrl { get; set; }

    [CommandEqualsSeparatorSwitch("--max-col-width", SwitchValueSeparator = " ")]
    public string? MaxColWidth { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }
}
