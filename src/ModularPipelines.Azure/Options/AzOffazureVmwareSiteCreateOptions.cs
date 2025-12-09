using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("offure", "vmware", "site", "create")]
public record AzOffazureVmwareSiteCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--agent")]
    public string? Agent { get; set; }

    [CliOption("--appliance-name")]
    public string? ApplianceName { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--solution-id")]
    public string? SolutionId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}