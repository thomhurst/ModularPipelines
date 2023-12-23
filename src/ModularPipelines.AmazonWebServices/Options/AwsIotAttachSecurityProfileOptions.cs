using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "attach-security-profile")]
public record AwsIotAttachSecurityProfileOptions(
[property: CommandSwitch("--security-profile-name")] string SecurityProfileName,
[property: CommandSwitch("--security-profile-target-arn")] string SecurityProfileTargetArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}