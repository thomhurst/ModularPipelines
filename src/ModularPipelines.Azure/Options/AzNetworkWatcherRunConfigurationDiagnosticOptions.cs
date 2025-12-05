using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "run-configuration-diagnostic")]
public record AzNetworkWatcherRunConfigurationDiagnosticOptions(
[property: CliOption("--resource")] string Resource
) : AzOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--queries")]
    public string? Queries { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}