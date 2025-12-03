using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logic", "integration-account", "create")]
public record AzLogicIntegrationAccountCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--integration-service-environment")]
    public string? IntegrationServiceEnvironment { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}