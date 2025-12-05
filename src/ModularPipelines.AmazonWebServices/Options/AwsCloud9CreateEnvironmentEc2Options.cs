using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud9", "create-environment-ec2")]
public record AwsCloud9CreateEnvironmentEc2Options(
[property: CliOption("--name")] string Name,
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--image-id")] string ImageId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--automatic-stop-time-minutes")]
    public int? AutomaticStopTimeMinutes { get; set; }

    [CliOption("--owner-arn")]
    public string? OwnerArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--connection-type")]
    public string? ConnectionType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}