using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("healthcareapis", "workspace", "iot-connector", "fhir-destination", "list")]
public record AzHealthcareapisWorkspaceIotConnectorFhirDestinationListOptions(
[property: CliOption("--iot-connector-name")] string IotConnectorName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;