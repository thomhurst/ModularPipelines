using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "georecovery-alias", "set")]
public record AzEventhubsGeorecoveryAliasSetOptions(
[property: CliOption("--alias")] string Alias,
[property: CliOption("--partner-namespace")] string PartnerNamespace
) : AzOptions
{
    [CliOption("--alternate-name")]
    public string? AlternateName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}