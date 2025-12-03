using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("adp", "account", "data-pool", "create")]
public record AzAdpAccountDataPoolCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--data-pool-name")] string DataPoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--locations")]
    public string? Locations { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}