using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "policy", "show")]
public record AzIotDpsPolicyShowOptions(
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--pn")] string Pn
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}