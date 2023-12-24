using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "associate-phone-number-contact-flow")]
public record AwsConnectAssociatePhoneNumberContactFlowOptions(
[property: CommandSwitch("--phone-number-id")] string PhoneNumberId,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--contact-flow-id")] string ContactFlowId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}