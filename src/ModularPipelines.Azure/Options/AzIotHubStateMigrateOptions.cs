using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "state", "migrate")]
public record AzIotHubStateMigrateOptions : AzOptions
{
    [CliOption("--aspects")]
    public string? Aspects { get; set; }

    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--destination-hub")]
    public string? DestinationHub { get; set; }

    [CliOption("--destination-hub-login")]
    public string? DestinationHubLogin { get; set; }

    [CliOption("--destination-resource-group")]
    public string? DestinationResourceGroup { get; set; }

    [CliOption("--og")]
    public string? Og { get; set; }

    [CliOption("--oh")]
    public string? Oh { get; set; }

    [CliOption("--ol")]
    public string? Ol { get; set; }

    [CliFlag("--replace")]
    public bool? Replace { get; set; }
}