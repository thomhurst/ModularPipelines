using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "dps", "policy", "update")]
public record AzIotDpsPolicyUpdateOptions(
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--pn")] string Pn
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--primary-key")]
    public string? PrimaryKey { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rights")]
    public string? Rights { get; set; }

    [CliOption("--secondary-key")]
    public string? SecondaryKey { get; set; }
}