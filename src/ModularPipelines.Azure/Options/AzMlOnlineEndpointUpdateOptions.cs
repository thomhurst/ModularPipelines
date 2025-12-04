using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "online-endpoint", "update")]
public record AzMlOnlineEndpointUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--local")]
    public bool? Local { get; set; }

    [CliOption("--mirror-traffic")]
    public string? MirrorTraffic { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--traffic")]
    public string? Traffic { get; set; }

    [CliFlag("--web")]
    public bool? Web { get; set; }
}