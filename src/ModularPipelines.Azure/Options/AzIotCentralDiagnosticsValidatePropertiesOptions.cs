using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "diagnostics", "validate-properties")]
public record AzIotCentralDiagnosticsValidatePropertiesOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--minimum-severity")]
    public string? MinimumSeverity { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}