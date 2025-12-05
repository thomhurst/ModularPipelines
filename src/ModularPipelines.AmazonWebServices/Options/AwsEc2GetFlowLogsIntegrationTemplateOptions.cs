using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-flow-logs-integration-template")]
public record AwsEc2GetFlowLogsIntegrationTemplateOptions(
[property: CliOption("--flow-log-id")] string FlowLogId,
[property: CliOption("--config-delivery-s3-destination-arn")] string ConfigDeliveryS3DestinationArn,
[property: CliOption("--integrate-services")] string IntegrateServices
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}