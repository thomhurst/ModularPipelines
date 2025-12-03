using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "device-template", "list")]
public record AzIotCentralDeviceTemplateListOptions(
[property: CliOption("--app-id")] string AppId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliFlag("--compact")]
    public bool? Compact { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}