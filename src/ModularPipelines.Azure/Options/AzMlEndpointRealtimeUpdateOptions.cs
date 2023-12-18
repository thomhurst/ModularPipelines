using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "endpoint", "realtime", "update")]
public record AzMlEndpointRealtimeUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--add-property")]
    public string? AddProperty { get; set; }

    [CommandSwitch("--add-tag")]
    public string? AddTag { get; set; }

    [CommandSwitch("--ae")]
    public string? Ae { get; set; }

    [CommandSwitch("--ai")]
    public string? Ai { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--remove-tag")]
    public string? RemoveTag { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [BooleanCommandSwitch("--token-auth-enabled")]
    public bool? TokenAuthEnabled { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}