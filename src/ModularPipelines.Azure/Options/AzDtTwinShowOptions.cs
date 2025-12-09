using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "twin", "show")]
public record AzDtTwinShowOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--twin-id")] string TwinId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}