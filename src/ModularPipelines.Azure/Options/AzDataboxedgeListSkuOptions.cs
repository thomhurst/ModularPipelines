using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databoxedge", "list-sku")]
public record AzDataboxedgeListSkuOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }
}