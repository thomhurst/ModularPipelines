using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "associate-phone-number-contact-flow")]
public record AwsConnectAssociatePhoneNumberContactFlowOptions(
[property: CliOption("--phone-number-id")] string PhoneNumberId,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-flow-id")] string ContactFlowId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}