using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcareapis", "workspace", "iot-connector", "fhir-destination", "create")]
public record AzHealthcareapisWorkspaceIotConnectorFhirDestinationCreateOptions(
[property: CliOption("--fhir-destination-name")] string FhirDestinationName,
[property: CliOption("--fhir-service-resource-id")] string FhirServiceResourceId,
[property: CliOption("--iot-connector-name")] string IotConnectorName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-identity-resolution-type")] string ResourceIdentityResolutionType,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}