using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "post-agent-profile")]
public record AwsCodeguruprofilerPostAgentProfileOptions(
[property: CliOption("--agent-profile")] string AgentProfile,
[property: CliOption("--content-type")] string ContentType,
[property: CliOption("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CliOption("--profile-token")]
    public string? ProfileToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}