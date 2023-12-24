using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "start-task")]
public record AwsEcsStartTaskOptions(
[property: CommandSwitch("--container-instances")] string[] ContainerInstances,
[property: CommandSwitch("--task-definition")] string TaskDefinition
) : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CommandSwitch("--overrides")]
    public string? Overrides { get; set; }

    [CommandSwitch("--propagate-tags")]
    public string? PropagateTags { get; set; }

    [CommandSwitch("--reference-id")]
    public string? ReferenceId { get; set; }

    [CommandSwitch("--started-by")]
    public string? StartedBy { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}