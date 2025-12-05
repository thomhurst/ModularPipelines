using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "twin", "component", "update")]
public record AzDtTwinComponentUpdateOptions(
[property: CliOption("--component")] string Component,
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--json-patch")] string JsonPatch,
[property: CliOption("--twin-id")] string TwinId
) : AzOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}