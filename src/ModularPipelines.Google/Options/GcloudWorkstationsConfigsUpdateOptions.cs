using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "configs", "update")]
public record GcloudWorkstationsConfigsUpdateOptions(
[property: PositionalArgument] string Config,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CommandSwitch("--container-args")]
    public string[]? ContainerArgs { get; set; }

    [CommandSwitch("--container-command")]
    public string[]? ContainerCommand { get; set; }

    [CommandSwitch("--container-env")]
    public string[]? ContainerEnv { get; set; }

    [CommandSwitch("--container-run-as-user")]
    public string? ContainerRunAsUser { get; set; }

    [CommandSwitch("--container-working-dir")]
    public string? ContainerWorkingDir { get; set; }

    [BooleanCommandSwitch("--disable-public-ip-addresses")]
    public bool? DisablePublicIpAddresses { get; set; }

    [BooleanCommandSwitch("--enable-audit-agent")]
    public bool? EnableAuditAgent { get; set; }

    [BooleanCommandSwitch("--enable-confidential-compute")]
    public bool? EnableConfidentialCompute { get; set; }

    [BooleanCommandSwitch("--enable-nested-virtualization")]
    public bool? EnableNestedVirtualization { get; set; }

    [CommandSwitch("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CommandSwitch("--labels")]
    public string[]? Labels { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--network-tags")]
    public string[]? NetworkTags { get; set; }

    [CommandSwitch("--pool-size")]
    public string? PoolSize { get; set; }

    [CommandSwitch("--running-timeout")]
    public string? RunningTimeout { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--service-account-scopes")]
    public string[]? ServiceAccountScopes { get; set; }

    [CommandSwitch("--container-custom-image")]
    public string? ContainerCustomImage { get; set; }

    [CommandSwitch("--container-predefined-image")]
    public string? ContainerPredefinedImage { get; set; }

    [BooleanCommandSwitch("base-image")]
    public bool? BaseImage { get; set; }

    [BooleanCommandSwitch("clion")]
    public bool? Clion { get; set; }

    [BooleanCommandSwitch("codeoss")]
    public bool? Codeoss { get; set; }

    [BooleanCommandSwitch("goland")]
    public bool? Goland { get; set; }

    [BooleanCommandSwitch("intellij")]
    public bool? Intellij { get; set; }

    [BooleanCommandSwitch("phpstorm")]
    public bool? Phpstorm { get; set; }

    [BooleanCommandSwitch("pycharm")]
    public bool? Pycharm { get; set; }

    [BooleanCommandSwitch("rider")]
    public bool? Rider { get; set; }

    [BooleanCommandSwitch("rubymine")]
    public bool? Rubymine { get; set; }

    [BooleanCommandSwitch("webstorm")]
    public bool? Webstorm { get; set; }
}