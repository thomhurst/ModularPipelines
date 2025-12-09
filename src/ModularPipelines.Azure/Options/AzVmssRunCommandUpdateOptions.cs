using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "run-command", "update")]
public record AzVmssRunCommandUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--async-execution")]
    public bool? AsyncExecution { get; set; }

    [CliOption("--command-id")]
    public string? CommandId { get; set; }

    [CliOption("--error-blob-uri")]
    public string? ErrorBlobUri { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--output-blob-uri")]
    public string? OutputBlobUri { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--protected-parameters")]
    public string? ProtectedParameters { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--run-as-password")]
    public string? RunAsPassword { get; set; }

    [CliOption("--run-as-user")]
    public string? RunAsUser { get; set; }

    [CliOption("--script")]
    public string? Script { get; set; }

    [CliOption("--script-uri")]
    public string? ScriptUri { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--timeout-in-seconds")]
    public string? TimeoutInSeconds { get; set; }

    [CliOption("--vmss-name")]
    public string? VmssName { get; set; }
}