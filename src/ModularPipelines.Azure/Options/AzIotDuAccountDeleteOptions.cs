using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "account", "delete")]
public record AzIotDuAccountDeleteOptions(
[property: CliOption("--account")] int Account
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}