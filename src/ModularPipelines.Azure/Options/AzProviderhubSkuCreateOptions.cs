using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("providerhub", "sku", "create")]
public record AzProviderhubSkuCreateOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--sku")] string Sku,
[property: CliOption("--sku-settings")] string SkuSettings
) : AzOptions
{
    [CliOption("--nested-first")]
    public string? NestedFirst { get; set; }

    [CliOption("--nested-resource-type-second")]
    public string? NestedResourceTypeSecond { get; set; }

    [CliOption("--nested-resource-type-third")]
    public string? NestedResourceTypeThird { get; set; }
}