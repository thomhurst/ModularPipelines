using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "build")]
public record AzAcrBuildOptions(
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--agent-pool")]
    public string? AgentPool { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--build-arg")]
    public IEnumerable<KeyValue>? BuildArg { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--log-template")]
    public string? LogTemplate { get; set; }

    [CliFlag("--no-format")]
    public bool? NoFormat { get; set; }

    [CliFlag("--no-logs")]
    public bool? NoLogs { get; set; }

    [CliFlag("--no-push")]
    public bool? NoPush { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secret-build-arg")]
    public string? SecretBuildArg { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? SOURCELOCATION { get; set; }
}