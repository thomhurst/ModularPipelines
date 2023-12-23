using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "enable-mfa-device")]
public record AwsIamEnableMfaDeviceOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--serial-number")] string SerialNumber,
[property: CommandSwitch("--authentication-code1")] string AuthenticationCode1,
[property: CommandSwitch("--authentication-code2")] string AuthenticationCode2
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}