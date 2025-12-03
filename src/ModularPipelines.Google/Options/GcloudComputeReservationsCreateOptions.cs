using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "reservations", "create")]
public record GcloudComputeReservationsCreateOptions(
[property: CliArgument] string Reservation,
[property: CliOption("--vm-count")] string VmCount,
[property: CliFlag("--require-specific-reservation")] bool RequireSpecificReservation,
[property: CliOption("--resource-policies")] IEnumerable<KeyValue> ResourcePolicies,
[property: CliOption("--source-instance-template")] string SourceInstanceTemplate,
[property: CliOption("--machine-type")] string MachineType,
[property: CliOption("--accelerator")] string[] Accelerator,
[property: CliFlag("count")] bool Count,
[property: CliFlag("type")] bool Type,
[property: CliOption("--local-ssd")] string[] LocalSsd,
[property: CliFlag("interface")] bool Interface,
[property: CliFlag("size")] bool Size,
[property: CliOption("--min-cpu-platform")] string MinCpuPlatform
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--share-setting")]
    public string? ShareSetting { get; set; }

    [CliOption("--share-with")]
    public string[]? ShareWith { get; set; }
}