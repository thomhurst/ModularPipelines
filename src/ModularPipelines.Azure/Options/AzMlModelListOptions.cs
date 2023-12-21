using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "model", "list")]
public record AzMlModelListOptions : AzOptions
{
    [CommandSwitch("--dataset-id")]
    public string? DatasetId { get; set; }

    [CommandSwitch("--latest")]
    public string? Latest { get; set; }

    [CommandSwitch("--model-name")]
    public string? ModelName { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--property")]
    public string? Property { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}