using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "georecovery-alias", "exists")]
public record AzServicebusGeorecoveryAliasExistsOptions(
[property: CliOption("--alias")] string Alias
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}