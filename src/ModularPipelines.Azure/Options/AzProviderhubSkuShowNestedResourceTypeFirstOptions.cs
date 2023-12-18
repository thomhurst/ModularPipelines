using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "sku", "show-nested-resource-type-first")]
public record AzProviderhubSkuShowNestedResourceTypeFirstOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--nested-first")]
    public string? NestedFirst { get; set; }

    [CommandSwitch("--provider-namespace")]
    public string? ProviderNamespace { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}