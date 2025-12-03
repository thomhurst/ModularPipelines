using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "twin", "component", "show")]
public record AzDtTwinComponentShowOptions(
[property: CliOption("--component")] string Component,
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--twin-id")] string TwinId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}