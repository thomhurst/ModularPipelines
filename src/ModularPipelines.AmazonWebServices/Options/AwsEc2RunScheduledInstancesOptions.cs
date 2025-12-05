using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "run-scheduled-instances")]
public record AwsEc2RunScheduledInstancesOptions(
[property: CliOption("--launch-specification")] string LaunchSpecification,
[property: CliOption("--scheduled-instance-id")] string ScheduledInstanceId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}