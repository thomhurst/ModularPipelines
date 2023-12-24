using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "remove-permission")]
public record AwsCodeguruprofilerRemovePermissionOptions(
[property: CommandSwitch("--action-group")] string ActionGroup,
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName,
[property: CommandSwitch("--revision-id")] string RevisionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}