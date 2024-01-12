using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "reservations", "create")]
public record GcloudComputeReservationsCreateOptions(
[property: PositionalArgument] string Reservation,
[property: CommandSwitch("--vm-count")] string VmCount,
[property: BooleanCommandSwitch("--require-specific-reservation")] bool RequireSpecificReservation,
[property: CommandSwitch("--resource-policies")] IEnumerable<KeyValue> ResourcePolicies,
[property: CommandSwitch("--source-instance-template")] string SourceInstanceTemplate,
[property: CommandSwitch("--machine-type")] string MachineType,
[property: CommandSwitch("--accelerator")] string[] Accelerator,
[property: BooleanCommandSwitch("count")] bool Count,
[property: BooleanCommandSwitch("type")] bool Type,
[property: CommandSwitch("--local-ssd")] string[] LocalSsd,
[property: BooleanCommandSwitch("interface")] bool Interface,
[property: BooleanCommandSwitch("size")] bool Size,
[property: CommandSwitch("--min-cpu-platform")] string MinCpuPlatform
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--share-setting")]
    public string? ShareSetting { get; set; }

    [CommandSwitch("--share-with")]
    public string[]? ShareWith { get; set; }
}