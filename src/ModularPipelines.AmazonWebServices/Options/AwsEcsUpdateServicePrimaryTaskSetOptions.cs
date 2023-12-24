using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "update-service-primary-task-set")]
public record AwsEcsUpdateServicePrimaryTaskSetOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--primary-task-set")] string PrimaryTaskSet
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}