using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-flow-logs-integration-template")]
public record AwsEc2GetFlowLogsIntegrationTemplateOptions(
[property: CommandSwitch("--flow-log-id")] string FlowLogId,
[property: CommandSwitch("--config-delivery-s3-destination-arn")] string ConfigDeliveryS3DestinationArn,
[property: CommandSwitch("--integrate-services")] string IntegrateServices
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}