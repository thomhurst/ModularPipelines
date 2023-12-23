using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "start-session")]
public record AwsAthenaStartSessionOptions(
[property: CommandSwitch("--work-group")] string WorkGroup,
[property: CommandSwitch("--engine-configuration")] string EngineConfiguration
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--notebook-version")]
    public string? NotebookVersion { get; set; }

    [CommandSwitch("--session-idle-timeout-in-minutes")]
    public int? SessionIdleTimeoutInMinutes { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}