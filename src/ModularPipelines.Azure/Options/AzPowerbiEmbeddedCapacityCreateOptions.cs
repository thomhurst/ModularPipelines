using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("powerbi", "embedded-capacity", "create")]
public record AzPowerbiEmbeddedCapacityCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku-name")] string SkuName
) : AzOptions
{
    [CliOption("--administration-members")]
    public string? AdministrationMembers { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku-tier")]
    public string? SkuTier { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}