using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "list-tasks")]
public record AwsEcsListTasksOptions : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--container-instance")]
    public string? ContainerInstance { get; set; }

    [CommandSwitch("--family")]
    public string? Family { get; set; }

    [CommandSwitch("--started-by")]
    public string? StartedBy { get; set; }

    [CommandSwitch("--service-name")]
    public string? ServiceName { get; set; }

    [CommandSwitch("--desired-status")]
    public string? DesiredStatus { get; set; }

    [CommandSwitch("--launch-type")]
    public string? LaunchType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}