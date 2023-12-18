using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic-san", "list-sku")]
public record AzElasticSanListSkuOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}

