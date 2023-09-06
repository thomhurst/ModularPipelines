using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("plugin", "install")]
[ExcludeFromCodeCoverage]
public record HelmPluginInstallOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }
}
