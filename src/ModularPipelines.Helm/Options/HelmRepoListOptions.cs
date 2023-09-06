using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("repo", "list")]
[ExcludeFromCodeCoverage]
public record HelmRepoListOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }
}
