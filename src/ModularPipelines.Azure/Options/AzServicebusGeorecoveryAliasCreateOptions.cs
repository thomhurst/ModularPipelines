using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "georecovery-alias", "create")]
public record AzServicebusGeorecoveryAliasCreateOptions(
[property: CliOption("--alias")] string Alias,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--alternate-name")]
    public string? AlternateName { get; set; }

    [CliOption("--partner-namespace")]
    public string? PartnerNamespace { get; set; }
}