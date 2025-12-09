using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "create-simulation-job")]
public record AwsRobomakerCreateSimulationJobOptions(
[property: CliOption("--max-job-duration-in-seconds")] long MaxJobDurationInSeconds,
[property: CliOption("--iam-role")] string IamRole
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--output-location")]
    public string? OutputLocation { get; set; }

    [CliOption("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CliOption("--failure-behavior")]
    public string? FailureBehavior { get; set; }

    [CliOption("--robot-applications")]
    public string[]? RobotApplications { get; set; }

    [CliOption("--simulation-applications")]
    public string[]? SimulationApplications { get; set; }

    [CliOption("--data-sources")]
    public string[]? DataSources { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--compute")]
    public string? Compute { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}