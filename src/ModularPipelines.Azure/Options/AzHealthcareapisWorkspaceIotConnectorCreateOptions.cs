using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("healthcareapis", "workspace", "iot-connector", "create")]
public record AzHealthcareapisWorkspaceIotConnectorCreateOptions(
[property: CliOption("--iot-connector-name")] string IotConnectorName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--ingestion-endpoint-configuration")]
    public string? IngestionEndpointConfiguration { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-assigned-identities")]
    public string? UserAssignedIdentities { get; set; }
}