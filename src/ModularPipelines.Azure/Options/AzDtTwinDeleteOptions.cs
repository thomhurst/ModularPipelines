using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "twin", "delete")]
public record AzDtTwinDeleteOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--twin-id")] string TwinId
) : AzOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}