using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "run-task")]
public record AwsEcsRunTaskOptions(
[property: CommandSwitch("--task-definition")] string TaskDefinition
) : AwsOptions
{
    [CommandSwitch("--capacity-provider-strategy")]
    public string[]? CapacityProviderStrategy { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--count")]
    public int? Count { get; set; }

    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--launch-type")]
    public string? LaunchType { get; set; }

    [CommandSwitch("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CommandSwitch("--overrides")]
    public string? Overrides { get; set; }

    [CommandSwitch("--placement-constraints")]
    public string[]? PlacementConstraints { get; set; }

    [CommandSwitch("--placement-strategy")]
    public string[]? PlacementStrategy { get; set; }

    [CommandSwitch("--platform-version")]
    public string? PlatformVersion { get; set; }

    [CommandSwitch("--propagate-tags")]
    public string? PropagateTags { get; set; }

    [CommandSwitch("--reference-id")]
    public string? ReferenceId { get; set; }

    [CommandSwitch("--started-by")]
    public string? StartedBy { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}