using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-vpc-endpoint-connection-notification")]
public record AwsEc2CreateVpcEndpointConnectionNotificationOptions(
[property: CliOption("--connection-notification-arn")] string ConnectionNotificationArn,
[property: CliOption("--connection-events")] string[] ConnectionEvents
) : AwsOptions
{
    [CliOption("--service-id")]
    public string? ServiceId { get; set; }

    [CliOption("--vpc-endpoint-id")]
    public string? VpcEndpointId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}