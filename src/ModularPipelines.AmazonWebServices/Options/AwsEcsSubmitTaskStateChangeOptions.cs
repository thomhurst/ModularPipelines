using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "submit-task-state-change")]
public record AwsEcsSubmitTaskStateChangeOptions : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--task")]
    public string? Task { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--containers")]
    public string[]? Containers { get; set; }

    [CommandSwitch("--attachments")]
    public string[]? Attachments { get; set; }

    [CommandSwitch("--managed-agents")]
    public string[]? ManagedAgents { get; set; }

    [CommandSwitch("--pull-started-at")]
    public long? PullStartedAt { get; set; }

    [CommandSwitch("--pull-stopped-at")]
    public long? PullStoppedAt { get; set; }

    [CommandSwitch("--execution-stopped-at")]
    public long? ExecutionStoppedAt { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}