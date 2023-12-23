using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "delete-task-set")]
public record AwsEcsDeleteTaskSetOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--task-set")] string TaskSet
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}