using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("healthcareapis", "workspace", "iot-connector", "fhir-destination", "update")]
public record AzHealthcareapisWorkspaceIotConnectorFhirDestinationUpdateOptions(
[property: CliOption("--fhir-service-resource-id")] string FhirServiceResourceId,
[property: CliOption("--resource-identity-resolution-type")] string ResourceIdentityResolutionType
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--fhir-destination-name")]
    public string? FhirDestinationName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--iot-connector-name")]
    public string? IotConnectorName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}