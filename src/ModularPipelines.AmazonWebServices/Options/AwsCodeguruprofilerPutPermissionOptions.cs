using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "put-permission")]
public record AwsCodeguruprofilerPutPermissionOptions(
[property: CommandSwitch("--action-group")] string ActionGroup,
[property: CommandSwitch("--principals")] string[] Principals,
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}