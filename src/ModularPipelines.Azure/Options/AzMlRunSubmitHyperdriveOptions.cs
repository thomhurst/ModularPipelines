using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "run", "submit-hyperdrive")]
public record AzMlRunSubmitHyperdriveOptions(
[property: CliOption("--hyperdrive-configuration-name")] string HyperdriveConfigurationName
) : AzOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--conda-dependencies")]
    public string? CondaDependencies { get; set; }

    [CliOption("--ct")]
    public string? Ct { get; set; }

    [CliOption("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--run-configuration-name")]
    public string? RunConfigurationName { get; set; }

    [CliOption("--source-directory")]
    public string? SourceDirectory { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? USERSCRIPTANDARGUMENTS { get; set; }
}