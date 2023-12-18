using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "sku", "list")]
public record AzProviderhubSkuListOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions
{
    [CommandSwitch("--nested-first")]
    public string? NestedFirst { get; set; }

    [CommandSwitch("--nested-resource-type-second")]
    public string? NestedResourceTypeSecond { get; set; }

    [CommandSwitch("--nested-resource-type-third")]
    public string? NestedResourceTypeThird { get; set; }
}