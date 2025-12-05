using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "endpoint", "realtime", "get-logs")]
public record AzMlEndpointRealtimeGetLogsOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--init")]
    public bool? Init { get; set; }

    [CliOption("--num_lines")]
    public string? NumLines { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("-v")]
    public string? V { get; set; }
}