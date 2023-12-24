using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "deactivate-mfa-device")]
public record AwsIamDeactivateMfaDeviceOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--serial-number")] string SerialNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}