using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "submit-container-state-change")]
public record AwsEcsSubmitContainerStateChangeOptions : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--task")]
    public string? Task { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--runtime-id")]
    public string? RuntimeId { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--exit-code")]
    public int? ExitCode { get; set; }

    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--network-bindings")]
    public string[]? NetworkBindings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}