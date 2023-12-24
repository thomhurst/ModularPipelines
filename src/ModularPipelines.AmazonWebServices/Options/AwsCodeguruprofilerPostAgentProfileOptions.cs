using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "post-agent-profile")]
public record AwsCodeguruprofilerPostAgentProfileOptions(
[property: CommandSwitch("--agent-profile")] string AgentProfile,
[property: CommandSwitch("--content-type")] string ContentType,
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CommandSwitch("--profile-token")]
    public string? ProfileToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}