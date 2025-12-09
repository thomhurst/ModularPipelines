using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpc-endpoint-connection-notification")]
public record AwsEc2ModifyVpcEndpointConnectionNotificationOptions(
[property: CliOption("--connection-notification-id")] string ConnectionNotificationId
) : AwsOptions
{
    [CliOption("--connection-notification-arn")]
    public string? ConnectionNotificationArn { get; set; }

    [CliOption("--connection-events")]
    public string[]? ConnectionEvents { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}