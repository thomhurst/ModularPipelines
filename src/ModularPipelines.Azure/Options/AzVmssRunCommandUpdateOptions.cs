using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "run-command", "update")]
public record AzVmssRunCommandUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--async-execution")]
    public bool? AsyncExecution { get; set; }

    [CommandSwitch("--command-id")]
    public string? CommandId { get; set; }

    [CommandSwitch("--error-blob-uri")]
    public string? ErrorBlobUri { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--output-blob-uri")]
    public string? OutputBlobUri { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--protected-parameters")]
    public string? ProtectedParameters { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-as-password")]
    public string? RunAsPassword { get; set; }

    [CommandSwitch("--run-as-user")]
    public string? RunAsUser { get; set; }

    [CommandSwitch("--script")]
    public string? Script { get; set; }

    [CommandSwitch("--script-uri")]
    public string? ScriptUri { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--timeout-in-seconds")]
    public string? TimeoutInSeconds { get; set; }

    [CommandSwitch("--vmss-name")]
    public string? VmssName { get; set; }
}