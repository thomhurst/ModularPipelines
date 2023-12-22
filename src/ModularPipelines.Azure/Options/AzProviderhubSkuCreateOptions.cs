using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "sku", "create")]
public record AzProviderhubSkuCreateOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--sku")] string Sku,
[property: CommandSwitch("--sku-settings")] string SkuSettings
) : AzOptions
{
    [CommandSwitch("--nested-first")]
    public string? NestedFirst { get; set; }

    [CommandSwitch("--nested-resource-type-second")]
    public string? NestedResourceTypeSecond { get; set; }

    [CommandSwitch("--nested-resource-type-third")]
    public string? NestedResourceTypeThird { get; set; }
}