using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "endpoint", "realtime", "update")]
public record AzMlEndpointRealtimeUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--add-property")]
    public string? AddProperty { get; set; }

    [CliOption("--add-tag")]
    public string? AddTag { get; set; }

    [CliOption("--ae")]
    public string? Ae { get; set; }

    [CliOption("--ai")]
    public string? Ai { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--remove-tag")]
    public string? RemoveTag { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliFlag("--token-auth-enabled")]
    public bool? TokenAuthEnabled { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("-v")]
    public string? V { get; set; }
}