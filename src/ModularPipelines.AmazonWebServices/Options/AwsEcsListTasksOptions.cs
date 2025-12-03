using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "list-tasks")]
public record AwsEcsListTasksOptions : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--container-instance")]
    public string? ContainerInstance { get; set; }

    [CliOption("--family")]
    public string? Family { get; set; }

    [CliOption("--started-by")]
    public string? StartedBy { get; set; }

    [CliOption("--service-name")]
    public string? ServiceName { get; set; }

    [CliOption("--desired-status")]
    public string? DesiredStatus { get; set; }

    [CliOption("--launch-type")]
    public string? LaunchType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}