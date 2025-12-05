using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "sku", "show-nested-resource-type-third")]
public record AzProviderhubSkuShowNestedResourceTypeThirdOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--nested-first")]
    public string? NestedFirst { get; set; }

    [CliOption("--nested-resource-type-second")]
    public string? NestedResourceTypeSecond { get; set; }

    [CliOption("--nested-resource-type-third")]
    public string? NestedResourceTypeThird { get; set; }

    [CliOption("--provider-namespace")]
    public string? ProviderNamespace { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}