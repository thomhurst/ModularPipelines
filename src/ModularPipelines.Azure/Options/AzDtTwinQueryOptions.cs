using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "twin", "query")]
public record AzDtTwinQueryOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--query-command")] string QueryCommand
) : AzOptions
{
    [CliFlag("--cost")]
    public bool? Cost { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}