using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "policy", "delete")]
public record AzIotDpsPolicyDeleteOptions(
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--pn")] string Pn
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}