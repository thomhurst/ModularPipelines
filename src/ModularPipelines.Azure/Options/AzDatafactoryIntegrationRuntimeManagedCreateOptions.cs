using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "integration-runtime", "managed", "create")]
public record AzDatafactoryIntegrationRuntimeManagedCreateOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--integration-runtime-name")] string IntegrationRuntimeName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--compute-properties")]
    public string? ComputeProperties { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--ssis-properties")]
    public string? SsisProperties { get; set; }
}