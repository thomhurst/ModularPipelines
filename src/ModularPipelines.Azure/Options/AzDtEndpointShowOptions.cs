using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "endpoint", "show")]
public record AzDtEndpointShowOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--en")] string En
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}