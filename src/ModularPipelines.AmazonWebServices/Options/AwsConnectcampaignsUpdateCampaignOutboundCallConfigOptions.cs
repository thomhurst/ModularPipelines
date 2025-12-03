using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "update-campaign-outbound-call-config")]
public record AwsConnectcampaignsUpdateCampaignOutboundCallConfigOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--connect-contact-flow-id")]
    public string? ConnectContactFlowId { get; set; }

    [CliOption("--connect-source-phone-number")]
    public string? ConnectSourcePhoneNumber { get; set; }

    [CliOption("--answer-machine-detection-config")]
    public string? AnswerMachineDetectionConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}