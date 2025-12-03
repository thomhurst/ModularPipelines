using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "submit-container-state-change")]
public record AwsEcsSubmitContainerStateChangeOptions : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--task")]
    public string? Task { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--runtime-id")]
    public string? RuntimeId { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--exit-code")]
    public int? ExitCode { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--network-bindings")]
    public string[]? NetworkBindings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}