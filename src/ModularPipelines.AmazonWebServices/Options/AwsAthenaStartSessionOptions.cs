using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "start-session")]
public record AwsAthenaStartSessionOptions(
[property: CliOption("--work-group")] string WorkGroup,
[property: CliOption("--engine-configuration")] string EngineConfiguration
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--notebook-version")]
    public string? NotebookVersion { get; set; }

    [CliOption("--session-idle-timeout-in-minutes")]
    public int? SessionIdleTimeoutInMinutes { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}