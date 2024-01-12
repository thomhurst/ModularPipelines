using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "products", "list")]
public record GcloudApigeeProductsListOptions : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}