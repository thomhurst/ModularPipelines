using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "start-task")]
public record AwsEcsStartTaskOptions(
[property: CliOption("--container-instances")] string[] ContainerInstances,
[property: CliOption("--task-definition")] string TaskDefinition
) : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--group")]
    public string? Group { get; set; }

    [CliOption("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CliOption("--overrides")]
    public string? Overrides { get; set; }

    [CliOption("--propagate-tags")]
    public string? PropagateTags { get; set; }

    [CliOption("--reference-id")]
    public string? ReferenceId { get; set; }

    [CliOption("--started-by")]
    public string? StartedBy { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}