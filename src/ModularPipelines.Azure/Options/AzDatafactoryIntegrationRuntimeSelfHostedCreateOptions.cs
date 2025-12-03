using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "integration-runtime", "self-hosted", "create")]
public record AzDatafactoryIntegrationRuntimeSelfHostedCreateOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--integration-runtime-name")] string IntegrationRuntimeName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--linked-info")]
    public string? LinkedInfo { get; set; }
}