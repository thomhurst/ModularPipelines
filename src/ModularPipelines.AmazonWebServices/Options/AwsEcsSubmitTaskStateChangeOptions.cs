using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "submit-task-state-change")]
public record AwsEcsSubmitTaskStateChangeOptions : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--task")]
    public string? Task { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--containers")]
    public string[]? Containers { get; set; }

    [CliOption("--attachments")]
    public string[]? Attachments { get; set; }

    [CliOption("--managed-agents")]
    public string[]? ManagedAgents { get; set; }

    [CliOption("--pull-started-at")]
    public long? PullStartedAt { get; set; }

    [CliOption("--pull-stopped-at")]
    public long? PullStoppedAt { get; set; }

    [CliOption("--execution-stopped-at")]
    public long? ExecutionStoppedAt { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}