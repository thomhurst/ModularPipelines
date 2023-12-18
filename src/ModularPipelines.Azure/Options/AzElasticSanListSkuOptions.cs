using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic-san", "list-sku")]
public record AzElasticSanListSkuOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}