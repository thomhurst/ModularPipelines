using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "host", "group", "create")]
public record AzVmHostGroupCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--platform-fault-domain-count")] int PlatformFaultDomainCount,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--automatic-placement")]
    public bool? AutomaticPlacement { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}