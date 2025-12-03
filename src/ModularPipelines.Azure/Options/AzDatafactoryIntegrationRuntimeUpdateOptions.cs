using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "integration-runtime", "update")]
public record AzDatafactoryIntegrationRuntimeUpdateOptions : AzOptions
{
    [CliOption("--auto-update")]
    public string? AutoUpdate { get; set; }

    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--integration-runtime-name")]
    public string? IntegrationRuntimeName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--update-delay-offset")]
    public string? UpdateDelayOffset { get; set; }
}