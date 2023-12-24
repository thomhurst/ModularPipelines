using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-user-phone-config")]
public record AwsConnectUpdateUserPhoneConfigOptions(
[property: CommandSwitch("--phone-config")] string PhoneConfig,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}