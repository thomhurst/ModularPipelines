using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud9", "create-environment-ec2")]
public record AwsCloud9CreateEnvironmentEc2Options(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--image-id")] string ImageId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--automatic-stop-time-minutes")]
    public int? AutomaticStopTimeMinutes { get; set; }

    [CommandSwitch("--owner-arn")]
    public string? OwnerArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--connection-type")]
    public string? ConnectionType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}