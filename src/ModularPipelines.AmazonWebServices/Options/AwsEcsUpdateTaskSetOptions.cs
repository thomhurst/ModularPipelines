using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "update-task-set")]
public record AwsEcsUpdateTaskSetOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--task-set")] string TaskSet,
[property: CommandSwitch("--scale")] string Scale
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}