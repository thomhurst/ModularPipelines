using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "create-instance")]
public record GcloudComputeInstanceGroupsManagedCreateInstanceOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [CommandSwitch("--stateful-disk")]
    public string[]? StatefulDisk { get; set; }

    [CommandSwitch("--stateful-external-ip")]
    public string[]? StatefulExternalIp { get; set; }

    [CommandSwitch("--stateful-internal-ip")]
    public string[]? StatefulInternalIp { get; set; }

    [CommandSwitch("--stateful-metadata")]
    public IEnumerable<KeyValue>? StatefulMetadata { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}