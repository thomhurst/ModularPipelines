using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("dependency", "update")]
[ExcludeFromCodeCoverage]
public record HelmDependencyUpdateOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }

    [BooleanCommandSwitch("--skip-refresh")]
    public virtual bool? SkipRefresh { get; set; }

    [BooleanCommandSwitch("--verify")]
    public virtual bool? Verify { get; set; }
}