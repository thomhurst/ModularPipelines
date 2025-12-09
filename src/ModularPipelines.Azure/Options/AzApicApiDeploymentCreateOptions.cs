using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apic", "api", "deployment", "create")]
public record AzApicApiDeploymentCreateOptions(
[property: CliOption("--api")] string Api,
[property: CliOption("--deployment")] string Deployment,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CliOption("--definition-id")]
    public string? DefinitionId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}