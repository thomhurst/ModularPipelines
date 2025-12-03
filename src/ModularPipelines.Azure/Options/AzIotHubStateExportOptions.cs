using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "state", "export")]
public record AzIotHubStateExportOptions(
[property: CliOption("--state-file")] string StateFile
) : AzOptions
{
    [CliOption("--aspects")]
    public string? Aspects { get; set; }

    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliFlag("--replace")]
    public bool? Replace { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}