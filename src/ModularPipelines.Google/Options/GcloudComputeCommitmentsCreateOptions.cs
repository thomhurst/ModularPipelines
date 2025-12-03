using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "commitments", "create")]
public record GcloudComputeCommitmentsCreateOptions(
[property: CliArgument] string Commitment,
[property: CliOption("--plan")] string Plan,
[property: CliOption("--resources")] string[] Resources,
[property: CliFlag("memory")] bool Memory,
[property: CliFlag("vcpu")] bool Vcpu,
[property: CliFlag("local-ssd")] bool LocalSsd,
[property: CliOption("--resources-accelerator")] string[] ResourcesAccelerator,
[property: CliFlag("count")] bool Count,
[property: CliFlag("type")] bool Type
) : GcloudOptions
{
    [CliFlag("--auto-renew")]
    public bool? AutoRenew { get; set; }

    [CliOption("--merge-source-commitments")]
    public string? MergeSourceCommitments { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--split-source-commitment")]
    public string? SplitSourceCommitment { get; set; }

    [CliOption("--reservations-from-file")]
    public string? ReservationsFromFile { get; set; }

    [CliOption("--reservation")]
    public string? Reservation { get; set; }

    [CliOption("--reservation-zone")]
    public string? ReservationZone { get; set; }

    [CliOption("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CliFlag("interface")]
    public bool? Interface { get; set; }

    [CliFlag("size")]
    public bool? Size { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [CliFlag("--require-specific-reservation")]
    public bool? RequireSpecificReservation { get; set; }

    [CliOption("--resource-policies")]
    public IEnumerable<KeyValue>? ResourcePolicies { get; set; }

    [CliOption("--vm-count")]
    public string? VmCount { get; set; }

    [CliOption("--share-setting")]
    public string? ShareSetting { get; set; }

    [CliOption("--share-with")]
    public string[]? ShareWith { get; set; }
}