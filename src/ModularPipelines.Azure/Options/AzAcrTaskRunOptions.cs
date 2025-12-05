using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "task", "run")]
public record AzAcrTaskRunOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--agent-pool")]
    public string? AgentPool { get; set; }

    [CliOption("--arg")]
    public string? Arg { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

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

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secret-arg")]
    public string? SecretArg { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--set-secret")]
    public string? SetSecret { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--update-trigger-token")]
    public string? UpdateTriggerToken { get; set; }
}