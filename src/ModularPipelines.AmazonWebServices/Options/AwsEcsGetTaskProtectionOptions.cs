using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "get-task-protection")]
public record AwsEcsGetTaskProtectionOptions(
[property: CommandSwitch("--cluster")] string Cluster
) : AwsOptions
{
    [CommandSwitch("--tasks")]
    public string[]? Tasks { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}