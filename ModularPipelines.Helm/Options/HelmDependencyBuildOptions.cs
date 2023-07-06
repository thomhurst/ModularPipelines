using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("dependency", "build")]
public record HelmDependencyBuildOptions : HelmOptions
{
    [CommandLongSwitch("keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }

    [BooleanCommandSwitch("skip-refresh")]
    public bool? SkipRefresh { get; set; }

    [BooleanCommandSwitch("verify")]
    public bool? Verify { get; set; }

}
