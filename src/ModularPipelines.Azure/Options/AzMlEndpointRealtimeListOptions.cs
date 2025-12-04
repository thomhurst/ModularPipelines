using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "endpoint", "realtime", "list")]
public record AzMlEndpointRealtimeListOptions : AzOptions
{
    [CliOption("--compute-type")]
    public string? ComputeType { get; set; }

    [CliOption("--model-id")]
    public string? ModelId { get; set; }

    [CliOption("--model-name")]
    public string? ModelName { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--property")]
    public string? Property { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("-v")]
    public string? V { get; set; }
}