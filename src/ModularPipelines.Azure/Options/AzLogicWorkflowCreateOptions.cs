using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logic", "workflow", "create")]
public record AzLogicWorkflowCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--access-control")]
    public string? AccessControl { get; set; }

    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--endpoints-configuration")]
    public string? EndpointsConfiguration { get; set; }

    [CliOption("--integration-account")]
    public int? IntegrationAccount { get; set; }

    [CliOption("--integration-service-environment")]
    public string? IntegrationServiceEnvironment { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}