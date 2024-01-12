using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "commitments", "create")]
public record GcloudComputeCommitmentsCreateOptions(
[property: PositionalArgument] string Commitment,
[property: CommandSwitch("--plan")] string Plan,
[property: CommandSwitch("--resources")] string[] Resources,
[property: BooleanCommandSwitch("memory")] bool Memory,
[property: BooleanCommandSwitch("vcpu")] bool Vcpu,
[property: BooleanCommandSwitch("local-ssd")] bool LocalSsd,
[property: CommandSwitch("--resources-accelerator")] string[] ResourcesAccelerator,
[property: BooleanCommandSwitch("count")] bool Count,
[property: BooleanCommandSwitch("type")] bool Type
) : GcloudOptions
{
    [BooleanCommandSwitch("--auto-renew")]
    public bool? AutoRenew { get; set; }

    [CommandSwitch("--merge-source-commitments")]
    public string? MergeSourceCommitments { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--split-source-commitment")]
    public string? SplitSourceCommitment { get; set; }

    [CommandSwitch("--reservations-from-file")]
    public string? ReservationsFromFile { get; set; }

    [CommandSwitch("--reservation")]
    public string? Reservation { get; set; }

    [CommandSwitch("--reservation-zone")]
    public string? ReservationZone { get; set; }

    [CommandSwitch("--accelerator")]
    public string[]? Accelerator { get; set; }

    [BooleanCommandSwitch("interface")]
    public bool? Interface { get; set; }

    [BooleanCommandSwitch("size")]
    public bool? Size { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [BooleanCommandSwitch("--require-specific-reservation")]
    public bool? RequireSpecificReservation { get; set; }

    [CommandSwitch("--resource-policies")]
    public IEnumerable<KeyValue>? ResourcePolicies { get; set; }

    [CommandSwitch("--vm-count")]
    public string? VmCount { get; set; }

    [CommandSwitch("--share-setting")]
    public string? ShareSetting { get; set; }

    [CommandSwitch("--share-with")]
    public string[]? ShareWith { get; set; }
}