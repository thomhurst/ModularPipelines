using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "integration-runtime", "linked-integration-runtime", "create")]
public record AzDatafactoryIntegrationRuntimeLinkedIntegrationRuntimeCreateOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--integration-runtime-name")] string IntegrationRuntimeName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--data-factory-name")]
    public string? DataFactoryName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }
}