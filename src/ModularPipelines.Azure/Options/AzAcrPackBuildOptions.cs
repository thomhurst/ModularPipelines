using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "pack", "build")]
public record AzAcrPackBuildOptions(
[property: CliOption("--builder")] string Builder,
[property: CliOption("--image")] string Image,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--agent-pool")]
    public string? AgentPool { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliFlag("--no-format")]
    public bool? NoFormat { get; set; }

    [CliFlag("--no-logs")]
    public bool? NoLogs { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pack-image-tag")]
    public string? PackImageTag { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliFlag("--pull")]
    public bool? Pull { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? SOURCELOCATION { get; set; }
}