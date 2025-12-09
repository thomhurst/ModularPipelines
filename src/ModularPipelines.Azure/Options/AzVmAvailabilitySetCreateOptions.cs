using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "availability-set", "create")]
public record AzVmAvailabilitySetCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--platform-fault-domain-count")]
    public int? PlatformFaultDomainCount { get; set; }

    [CliOption("--platform-update-domain-count")]
    public int? PlatformUpdateDomainCount { get; set; }

    [CliOption("--ppg")]
    public string? Ppg { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--unmanaged")]
    public bool? Unmanaged { get; set; }

    [CliFlag("--validate")]
    public bool? Validate { get; set; }
}