using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "service", "list")]
public record AzMlServiceListOptions(
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--compute-type")]
    public string? ComputeType { get; set; }

    [CommandSwitch("--image-digest")]
    public string? ImageDigest { get; set; }

    [CommandSwitch("--model-id")]
    public string? ModelId { get; set; }

    [CommandSwitch("--model-name")]
    public string? ModelName { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--property")]
    public string? Property { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}