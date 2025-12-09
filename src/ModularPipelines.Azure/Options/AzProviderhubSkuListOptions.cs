using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "sku", "list")]
public record AzProviderhubSkuListOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace,
[property: CliOption("--resource-type")] string ResourceType
) : AzOptions
{
    [CliOption("--nested-first")]
    public string? NestedFirst { get; set; }

    [CliOption("--nested-resource-type-second")]
    public string? NestedResourceTypeSecond { get; set; }

    [CliOption("--nested-resource-type-third")]
    public string? NestedResourceTypeThird { get; set; }
}