using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "workspace", "iot-connector", "fhir-destination", "create")]
public record AzHealthcareapisWorkspaceIotConnectorFhirDestinationCreateOptions(
[property: CommandSwitch("--fhir-destination-name")] string FhirDestinationName,
[property: CommandSwitch("--fhir-service-resource-id")] string FhirServiceResourceId,
[property: CommandSwitch("--iot-connector-name")] string IotConnectorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-identity-resolution-type")] string ResourceIdentityResolutionType,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

