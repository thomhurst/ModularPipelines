using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "create-simulation-job")]
public record AwsRobomakerCreateSimulationJobOptions(
[property: CommandSwitch("--max-job-duration-in-seconds")] long MaxJobDurationInSeconds,
[property: CommandSwitch("--iam-role")] string IamRole
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--output-location")]
    public string? OutputLocation { get; set; }

    [CommandSwitch("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CommandSwitch("--failure-behavior")]
    public string? FailureBehavior { get; set; }

    [CommandSwitch("--robot-applications")]
    public string[]? RobotApplications { get; set; }

    [CommandSwitch("--simulation-applications")]
    public string[]? SimulationApplications { get; set; }

    [CommandSwitch("--data-sources")]
    public string[]? DataSources { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--compute")]
    public string? Compute { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}