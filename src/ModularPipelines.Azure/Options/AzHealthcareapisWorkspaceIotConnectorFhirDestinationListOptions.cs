using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "workspace", "iot-connector", "fhir-destination", "list")]
public record AzHealthcareapisWorkspaceIotConnectorFhirDestinationListOptions(
[property: CommandSwitch("--iot-connector-name")] string IotConnectorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;