using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "run")]
public record AzAcrRunOptions(
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--agent-pool")]
    public string? AgentPool { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--cmd")]
    public string? Cmd { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--log-template")]
    public string? LogTemplate { get; set; }

    [CliFlag("--no-format")]
    public bool? NoFormat { get; set; }

    [CliFlag("--no-logs")]
    public bool? NoLogs { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--set-secret")]
    public string? SetSecret { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--values")]
    public string? Values { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? SOURCELOCATION { get; set; }
}