using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcareapis", "workspace", "iot-connector", "fhir-destination", "show")]
public record AzHealthcareapisWorkspaceIotConnectorFhirDestinationShowOptions : AzOptions
{
    [CliOption("--fhir-destination-name")]
    public string? FhirDestinationName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--iot-connector-name")]
    public string? IotConnectorName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}