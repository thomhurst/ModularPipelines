using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "run", "submit-script")]
public record AzMlRunSubmitScriptOptions(
[property: CommandSwitch("--run")] string Run
) : AzOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--conda-dependencies")]
    public string? CondaDependencies { get; set; }

    [CommandSwitch("--ct")]
    public string? Ct { get; set; }

    [CommandSwitch("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CommandSwitch("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-configuration-name")]
    public string? RunConfigurationName { get; set; }

    [CommandSwitch("--source-directory")]
    public string? SourceDirectory { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? UserScriptAndArguments { get; set; }
}

