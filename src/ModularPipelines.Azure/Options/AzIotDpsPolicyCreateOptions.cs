using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "policy", "create")]
public record AzIotDpsPolicyCreateOptions(
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--pn")] string Pn,
[property: CliOption("--rights")] string Rights
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--primary-key")]
    public string? PrimaryKey { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secondary-key")]
    public string? SecondaryKey { get; set; }
}