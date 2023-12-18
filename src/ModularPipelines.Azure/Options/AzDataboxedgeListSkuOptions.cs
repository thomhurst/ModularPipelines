using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge", "list-sku")]
public record AzDataboxedgeListSkuOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}