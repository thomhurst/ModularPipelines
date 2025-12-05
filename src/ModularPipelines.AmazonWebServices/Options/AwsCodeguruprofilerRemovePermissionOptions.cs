using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "remove-permission")]
public record AwsCodeguruprofilerRemovePermissionOptions(
[property: CliOption("--action-group")] string ActionGroup,
[property: CliOption("--profiling-group-name")] string ProfilingGroupName,
[property: CliOption("--revision-id")] string RevisionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}