using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "describe-task-sets")]
public record AwsEcsDescribeTaskSetsOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--service")] string Service
) : AwsOptions
{
    [CommandSwitch("--task-sets")]
    public string[]? TaskSets { get; set; }

    [CommandSwitch("--include")]
    public string[]? Include { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}