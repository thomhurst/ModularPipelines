using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "update", "delete")]
public record AzIotDuUpdateDeleteOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--un")] string Un,
[property: CliOption("--up")] string Up,
[property: CliOption("--update-version")] string UpdateVersion
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}