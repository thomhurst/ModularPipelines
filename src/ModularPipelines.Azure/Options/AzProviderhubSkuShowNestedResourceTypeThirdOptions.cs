using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "sku", "show-nested-resource-type-third")]
public record AzProviderhubSkuShowNestedResourceTypeThirdOptions : AzOptions
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
    public string? Subscription { get; set; }
}