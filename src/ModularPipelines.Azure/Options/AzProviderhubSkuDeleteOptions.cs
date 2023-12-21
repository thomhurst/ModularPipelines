using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "sku", "delete")]
public record AzProviderhubSkuDeleteOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--nested-first")]
    public string? NestedFirst { get; set; }

    [CommandSwitch("--nested-resource-type-second")]
    public string? NestedResourceTypeSecond { get; set; }

    [CommandSwitch("--nested-resource-type-third")]
    public string? NestedResourceTypeThird { get; set; }

    [CommandSwitch("--provider-namespace")]
    public string? ProviderNamespace { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}