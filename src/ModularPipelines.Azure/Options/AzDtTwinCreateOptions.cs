using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "twin", "create")]
public record AzDtTwinCreateOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--dtmi")] string Dtmi,
[property: CliOption("--twin-id")] string TwinId
) : AzOptions
{
    [CliFlag("--if-none-match")]
    public bool? IfNoneMatch { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}