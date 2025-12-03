using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastic-san", "list-sku")]
public record AzElasticSanListSkuOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }
}