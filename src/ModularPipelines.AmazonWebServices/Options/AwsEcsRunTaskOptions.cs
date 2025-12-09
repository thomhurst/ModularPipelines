using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "run-task")]
public record AwsEcsRunTaskOptions(
[property: CliOption("--task-definition")] string TaskDefinition
) : AwsOptions
{
    [CliOption("--capacity-provider-strategy")]
    public string[]? CapacityProviderStrategy { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--count")]
    public int? Count { get; set; }

    [CliOption("--group")]
    public string? Group { get; set; }

    [CliOption("--launch-type")]
    public string? LaunchType { get; set; }

    [CliOption("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CliOption("--overrides")]
    public string? Overrides { get; set; }

    [CliOption("--placement-constraints")]
    public string[]? PlacementConstraints { get; set; }

    [CliOption("--placement-strategy")]
    public string[]? PlacementStrategy { get; set; }

    [CliOption("--platform-version")]
    public string? PlatformVersion { get; set; }

    [CliOption("--propagate-tags")]
    public string? PropagateTags { get; set; }

    [CliOption("--reference-id")]
    public string? ReferenceId { get; set; }

    [CliOption("--started-by")]
    public string? StartedBy { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}