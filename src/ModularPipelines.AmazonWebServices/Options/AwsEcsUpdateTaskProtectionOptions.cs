using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "update-task-protection")]
public record AwsEcsUpdateTaskProtectionOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--tasks")] string[] Tasks
) : AwsOptions
{
    [CommandSwitch("--expires-in-minutes")]
    public int? ExpiresInMinutes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}