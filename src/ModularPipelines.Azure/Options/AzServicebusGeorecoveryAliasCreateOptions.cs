using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "georecovery-alias", "create")]
public record AzServicebusGeorecoveryAliasCreateOptions(
[property: CommandSwitch("--alias")] string Alias,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--alternate-name")]
    public string? AlternateName { get; set; }

    [CommandSwitch("--partner-namespace")]
    public string? PartnerNamespace { get; set; }
}