using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "update")]
public record AzAcrTaskUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--agent-pool")]
    public string? AgentPool { get; set; }

    [CommandSwitch("--arg")]
    public string? Arg { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [BooleanCommandSwitch("--base-image-trigger-enabled")]
    public bool? BaseImageTriggerEnabled { get; set; }

    [CommandSwitch("--base-image-trigger-type")]
    public string? BaseImageTriggerType { get; set; }

    [CommandSwitch("--cmd")]
    public string? Cmd { get; set; }

    [BooleanCommandSwitch("--commit-trigger-enabled")]
    public bool? CommitTriggerEnabled { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--git-access-token")]
    public string? GitAccessToken { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--log-template")]
    public string? LogTemplate { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [BooleanCommandSwitch("--no-push")]
    public bool? NoPush { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [BooleanCommandSwitch("--pull-request-trigger-enabled")]
    public bool? PullRequestTriggerEnabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secret-arg")]
    public string? SecretArg { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--set-secret")]
    public string? SetSecret { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--update-trigger-endpoint")]
    public string? UpdateTriggerEndpoint { get; set; }

    [CommandSwitch("--update-trigger-payload-type")]
    public string? UpdateTriggerPayloadType { get; set; }

    [CommandSwitch("--values")]
    public string? Values { get; set; }
}