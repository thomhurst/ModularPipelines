using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "pipeline-run", "cancel")]
public record AzDatafactoryPipelineRunCancelOptions : AzOptions
{
    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--is-recursive")]
    public bool? IsRecursive { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}