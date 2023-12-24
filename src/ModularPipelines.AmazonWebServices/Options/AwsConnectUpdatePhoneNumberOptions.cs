using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-phone-number")]
public record AwsConnectUpdatePhoneNumberOptions(
[property: CommandSwitch("--phone-number-id")] string PhoneNumberId
) : AwsOptions
{
    [CommandSwitch("--target-arn")]
    public string? TargetArn { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}