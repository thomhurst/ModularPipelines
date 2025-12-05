using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "region", "list")]
public record AzKeyvaultRegionListOptions(
[property: CliOption("--hsm-name")] string HsmName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}