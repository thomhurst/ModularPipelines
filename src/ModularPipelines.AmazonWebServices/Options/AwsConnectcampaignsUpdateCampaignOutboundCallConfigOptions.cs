using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcampaigns", "update-campaign-outbound-call-config")]
public record AwsConnectcampaignsUpdateCampaignOutboundCallConfigOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--connect-contact-flow-id")]
    public string? ConnectContactFlowId { get; set; }

    [CommandSwitch("--connect-source-phone-number")]
    public string? ConnectSourcePhoneNumber { get; set; }

    [CommandSwitch("--answer-machine-detection-config")]
    public string? AnswerMachineDetectionConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}