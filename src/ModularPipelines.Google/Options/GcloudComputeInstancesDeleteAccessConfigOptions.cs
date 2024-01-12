using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "delete-access-config")]
public record GcloudComputeInstancesDeleteAccessConfigOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--access-config-name")]
    public string? AccessConfigName { get; set; }

    [CommandSwitch("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}