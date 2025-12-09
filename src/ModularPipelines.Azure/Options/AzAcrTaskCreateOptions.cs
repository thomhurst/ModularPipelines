using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "task", "create")]
public record AzAcrTaskCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--agent-pool")]
    public string? AgentPool { get; set; }

    [CliOption("--arg")]
    public string? Arg { get; set; }

    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliFlag("--base-image-trigger-enabled")]
    public bool? BaseImageTriggerEnabled { get; set; }

    [CliOption("--base-image-trigger-name")]
    public string? BaseImageTriggerName { get; set; }

    [CliOption("--base-image-trigger-type")]
    public string? BaseImageTriggerType { get; set; }

    [CliOption("--cmd")]
    public string? Cmd { get; set; }

    [CliFlag("--commit-trigger-enabled")]
    public bool? CommitTriggerEnabled { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--git-access-token")]
    public string? GitAccessToken { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliFlag("--is-system-task")]
    public bool? IsSystemTask { get; set; }

    [CliOption("--log-template")]
    public string? LogTemplate { get; set; }

    [CliFlag("--no-cache")]
    public bool? NoCache { get; set; }

    [CliFlag("--no-push")]
    public bool? NoPush { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliFlag("--pull-request-trigger-enabled")]
    public bool? PullRequestTriggerEnabled { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--secret-arg")]
    public string? SecretArg { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--set-secret")]
    public string? SetSecret { get; set; }

    [CliOption("--source-trigger-name")]
    public string? SourceTriggerName { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--update-trigger-endpoint")]
    public string? UpdateTriggerEndpoint { get; set; }

    [CliOption("--update-trigger-payload-type")]
    public string? UpdateTriggerPayloadType { get; set; }

    [CliOption("--values")]
    public string? Values { get; set; }
}