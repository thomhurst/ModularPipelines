using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "products", "list")]
public record GcloudApigeeProductsListOptions : GcloudOptions
{
    [CliOption("--organization")]
    public string? Organization { get; set; }
}